﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using x10.model;

namespace x10.compiler {
  public class UiCompilerPass2Test {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly AllEnums _allEnums;

    public UiCompilerPass2Test(ITestOutputHelper output) {
      _output = output;
      _allEnums = new AllEnums(_messages);
  }

  [Fact]
    public void RehydrateAssociation() {
      Entity apartment = CompilePass1(@"name: Apartment");
      Entity building = CompilePass1(@"
name: Building
associations:
  - name: apartments
    dataType: Apartment
");

      CompilePass2(building, apartment);
      Assert.Same(apartment, building.Associations.Single().ReferencedEntity);
    }

    [Fact]
    public void RehydrateInheritanceParent() {
      Entity child = CompilePass1(@"{name: Child, inheritsFrom: Parent}");
      Entity parent = CompilePass1(@"name: Parent");

      CompilePass2(child, parent);
      Assert.Same(parent, child.InheritsFrom);
    }

    [Fact]
    public void WrongDefaultValueType() {
      RunTest(@"
name: Tmp
attributes:
  - name: myAttribute
    dataType: Boolean
    default: 7
",
        "Error parsing attribute 'default': could not parse a(n) Boolean from '7'. Examples of valid data of this type: True, False.", 6, 14);
    }

    [Fact]
    public void WrongDefaultValueForEnum() {
      RunTest(@"
name: Tmp
attributes:
  - name: myAttribute
    dataType: MyEnum
    default: four

enums:
  - name: MyEnum
    values: one, two, three
",
        "Error parsing attribute 'default': 'four' is not a valid member of the Enumerated Type 'MyEnum'. Valid values are: one, two, three.", 6, 14);
    }

    [Fact]
    public void CorrectDefaultValueForEnum() {
      Entity entity = CompilePass1(@"
name: Tmp
description: Desc...
attributes:
  - name: myAttribute
    description: Desc...
    dataType: MyEnum
    default: three

enums:
  - name: MyEnum
    description: Desc...
    values: one, two, three
");
      CompilePass2(entity);

      Assert.Empty(_messages.Messages);
    }

    [Fact]
    public void InheritsFromNotFound() {
      RunTest(@"
name: Tmp
inheritsFrom: DoesNotExist
",
        "Entity 'DoesNotExist' not found", 3, 15);
    }

    [Fact]
    public void MultipleReferencesFound() {
      Entity entity = CompilePass1(@"
name: Ancestor
inheritsFrom: Duplicate");
      Entity duplicate1 = CompilePass1("name: Duplicate");
      Entity duplicate2 = CompilePass1("name: Duplicate");


      RunTest("Multiple entities with the name 'Duplicate' exist", 3, 15,
        entity, duplicate1, duplicate2);
    }

    [Fact]
    public void UniquenessOfEntityNames() {
      Entity duplicate1 = CompilePass1("name: Duplicate");
      Entity unique = CompilePass1("name: Unique");
      Entity duplicate2 = CompilePass1("name: Duplicate");

      RunTest("The Entity name 'Duplicate' is not unique.", 1, 7,
        duplicate1, unique, duplicate2);
    }

    [Fact]
    public void UniquenessOfEnumNames() {
      CompilePass1(@"
name: Dummy
enums:
  - name: Duplicate
    values: a, b, c
  - name: Duplicate
    values: d, e, f
");

      RunTest("The Enum name 'Duplicate' is not unique.", 4, 11);
    }

    #region Utilities

    private Entity CompilePass1(string yaml) {
      // Parse
      ParserYaml parser = new ParserYaml(_messages);
      TreeNode rootNode = parser.ParseFromString(yaml);
      if (rootNode == null)
        throw new Exception("Unalbe to parse yaml from: " + yaml);

      rootNode.SetFileInfo("Tmp.yaml");

      // Pass 1
      AttributeReader attrReader = new AttributeReader(_messages);
      EnumsCompiler enums = new EnumsCompiler(_messages, _allEnums, attrReader);
      EntityCompilerPass1 pass1 = new EntityCompilerPass1(_messages, enums, attrReader);
      Entity entity = pass1.CompileEntity(rootNode);

      return entity;
    }

    private void CompilePass2(params Entity[] entities) {

      // Pass 2
      AllEntities allEntities = new AllEntities(entities, _messages);
      EntityCompilerPass2 pass2 = new EntityCompilerPass2(_messages, allEntities, _allEnums);
      pass2.CompileAllEntities();

      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(string yaml, string expectedErrorMessage, int expectedLine, int expectedChar) {
      Entity entity = CompilePass1(yaml);
      RunTest(expectedErrorMessage, expectedLine, expectedChar, entity);
    }

    private void RunTest(string expectedErrorMessage, int expectedLine, int expectedChar, params Entity[] entities) {
      CompilePass2(entities);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.ParseElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.ParseElement.Start.CharacterPosition);
    }

    #endregion
  }
}