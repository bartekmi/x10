using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class BaseLibrary {

    private readonly static List<ClassDef> definitions = new List<ClassDef>() {
      // No-Data Formatting Components
        new ClassDefNative() {
          Name = "Heading1",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              // IsPrimary = true,    FUTURE: This is a prime candidate for a "primary" atomic attribute that receives text
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
          }
        },
        new ClassDefNative() {
          Name = "HorizontalDivider",
          InheritsFrom = ClassDefNative.Visual,
        },

        // Atomic Components
        new ClassDefNative() {
          Name = "TextEdit",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
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
          Name = "TextArea",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
        },
        new ClassDefNative() {
          Name = "IntEdit",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Integer,
        },
        new ClassDefNative() {
          Name = "FloatEdit",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Integer,
        },
        new ClassDefNative() {
          Name = "Checkbox",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Boolean,
        },
        new ClassDefNative() {
          Name = "BooleanViaButtons",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Boolean,
        },
        new ClassDefNative() {
          Name = "DateEditor",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Date,
        },
        new ClassDefNative() {
          Name = "DropDown",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = new DataTypeEnum(),
        },

        // Layout Components
        new ClassDefNative() {
          Name = "VerticalStackPanel",
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
        new ClassDefNative() {
          Name = "Row",
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
        new ClassDefNative() {
          Name = "RepellingRow",
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

        // Complex Components
        new ClassDefNative() {
          Name = "Table",
          ComponentDataModel = Entity.Object,
          IsMany = true,
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Columns",
              IsMany = true,
              IsMandatory = true,
              ReducesManyToOne = true,
              ComplexAttributeTypeName = "TableColumn",
            },
            new UiAttributeDefinitionComplex() {
              Name = "Header",
              IsMandatory = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          },
        },
        new ClassDefNative() {
          Name = "TableColumn",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Renderer",
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              DataType = DataTypes.Singleton.String,
            },
          },
        },
        new ClassDefNative() {
          Name = "HelpIcon",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "difficulty",
              DataType = DataTypes.Singleton.Integer,
            },
          },
        },
        new ClassDefNative() {
          Name = "Button",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "action",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "url",
              DataType = DataTypes.Singleton.String,
            },
          },
          // TODO: Add cross-validation... Either action or url must be provided
        },
        new ClassDefNative() {
          Name = "HollowButton",
          InheritsFromName = "Button",
        },

        // Model-Specific... TODO: will be moved to a separate library
        new ClassDefNative() {
          Name = "Metadata",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "MetadataEditor",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
      };


    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static UiLibrary CreateLibrary() {
      UiLibrary library = new UiLibrary(definitions) {
        Name = "Base Library",
      };

      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.String, "TextEdit");
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Integer, "FloatEdit");
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Float, "IntEdit");
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Boolean, "Checkbox");
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Date, "DateEditor");

      library.SetComponentForEnums("DropDown");

      return library;
    }
  }
}
