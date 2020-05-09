using System;
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
  public class EntitiesCompilerPass2Test {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly AllEnums _allEnums;

    public EntitiesCompilerPass2Test(ITestOutputHelper output) {
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

    [Fact]
    public void DuplicateMemberNameInDerived() {
      Entity child = CompilePass1(@"
name: Child
inheritsFrom: Parent
attributes:
  - name: myAttr
    dataType: String
");
      Entity parent = CompilePass1(@"
name: Parent
associations:
  - name: myAttr
    dataType: Parent
");
      RunTest("The name 'myAttr' is not unique among all the attributes and association of this Entity (possibly involving the entire inheritance hierarchy).", 4, 11, child, parent);
    }

    [Fact]
      public void DuplicateMemberNameInSameClass() {
        Entity entity = CompilePass1(@"
name: Child
attributes:
  - name: myAttr
    dataType: String
  - name: myAttr
    dataType: String
");

        RunTest("The name 'myAttr' is not unique among all the attributes and association of this Entity (possibly involving the entire inheritance hierarchy).", 4, 11, entity);
    }

    #region Utilities

    private Entity CompilePass1(string yaml) {
      return TestUtils.EntityCompilePass1(_messages, _allEnums, yaml);
    }

    private void CompilePass2(params Entity[] entities) {
      TestUtils.EntityCompilePass2(_messages, _allEnums, entities);
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