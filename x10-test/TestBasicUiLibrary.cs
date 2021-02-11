using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.metadata;
using x10.ui.composition;
using x10.ui.metadata;
using x10.compiler;
using x10.model.definition;
using x10.model;
using x10.ui.libraries;

namespace x10 {
  internal class TestBasicUiLibrary {
    internal UiLibrary UiLibrary {get; private set;}

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    internal TestBasicUiLibrary(ITestOutputHelper output) {
      _output = output;

      List<ClassDef> definitions = new List<ClassDef>() {
        new ClassDefNative() {
          Name = "Text",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
              IsPrimary = true,
              IsMandatory = true,
            },
          }
        },
        new ClassDefNative() {
          Name = "TextEdit",
          AtomicDataModel = DataTypes.Singleton.String,
          InheritsFrom = ClassDefNative.Editable,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "weight",
              DataType = new DataTypeEnum() {
                Name = "FontWeight",
                EnumValueValues = new string[] { "normal", "bold" },
              }
            },
          }
        },
        new ClassDefNative() {
          Name = "IntegerEdit",
          AtomicDataModel = DataTypes.Singleton.Integer,
          InheritsFrom = ClassDefNative.Editable,
        },
        new ClassDefNative() {
          Name = "Checkbox",
          AtomicDataModel = DataTypes.Singleton.Boolean,
          InheritsFrom = ClassDefNative.Editable,
        },
        new ClassDefNative() {
          Name = "DropDown",
          InheritsFrom = ClassDefNative.Editable,
          AtomicDataModel = new DataTypeEnum(),
        },

        new ClassDefNative() {
          Name = "Group",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          }
        },

      };

      UiLibrary = new UiLibrary(definitions) {
        Name = "Test Library",
      };

      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "IntegerEdit", UseMode.ReadWrite);
      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit", UseMode.ReadWrite);
      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox", UseMode.ReadWrite);

      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "IntegerEdit", UseMode.ReadOnly);
      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit", UseMode.ReadOnly);
      UiLibrary.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox", UseMode.ReadOnly);

      UiLibrary.SetComponentForEnums("DropDown");

      if (UiLibrary.HydrateAndValidate(_messages)) {
        TestUtils.DumpMessages(_messages, _output);
        Assert.Empty(_messages.Messages);
      }
    }

    internal ClassDefX10 CompileClassDef(string xml) {
      // Set up Basic Entities
      TestBasicEntities basicEntities = new TestBasicEntities(_output);
      AllEntities allEntities = basicEntities.AllEntities;
      AllEnums allEnums = basicEntities.AllEnums;

      // UI Compiler Pass 1
      UiAttributeReader attrReader = new UiAttributeReader(_messages, allEntities, allEnums, null);
      UiCompilerPass1 _compilerPass1 = new UiCompilerPass1(_messages, attrReader);
      ClassDefX10 classDef = TestUtils.UiCompilePass1(xml, _messages, _compilerPass1, _output);

      // Entity Compile Pass 3
      AllUiDefinitions allUiDefinitions = new AllUiDefinitions(_messages, null, new UiLibrary[] { UiLibrary });
      foreach (Entity entity in allEntities.All)
        (new EntityCompilerPass3(allUiDefinitions)).CompileEntity(entity);

      // UI Compiler Pass 2
      TestUtils.UiCompilePass2(_messages, allEntities, allEnums, UiLibrary, classDef);
      TestUtils.DumpMessages(_messages, _output);

      return classDef;
    }
  }
}