using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class BaseLibrary {

    private readonly static List<ClassDef> definitions = new List<ClassDef>() {
      #region No-Data Formatting Components
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
          Name = "Heading2",
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
          Name = "Heading3",
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
        new ClassDefNative() {
          Name = "VerticalDivider",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Bullet",
          InheritsFrom = ClassDefNative.Visual,
        },
      #endregion

      #region  Atomic Display Components
      new ClassDefNative() {
          Name = "Text",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "weight",
              DataType = new DataTypeEnum() {
                Name = "FontWeight",
                EnumValueValues = new string[] { "normal", "bold" },
              }
            },
          }
        },
      #endregion

      #region Atomic Edit Components
      new ClassDefNative() {
          Name = "TextEdit",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
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
          Name = "Pill",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
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
          Name = "TimestampEditor",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.Timestamp,
        },
        new ClassDefNative() {
          Name = "DropDown",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = new DataTypeEnum(),
        },
      #endregion

      #region Layout Components
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
          Name = "PackingLayout",
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
      #endregion

      #region Complex Components
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
          Name = "List",
          ComponentDataModel = Entity.Object,
          IsMany = true,
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "ItemTemplate",
              IsMandatory = true,
              ReducesManyToOne = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          },
        },
      #endregion

      #region Button / Menu
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
          Name = "Action",
          InheritsFrom = ClassDefNative.Object,
        },
        new ClassDefNative() {
          Name = "UploadAction",
          InheritsFromName = "Action",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "dialogTitle",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "fileFilter",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "backEndTarget",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
          },
        },
        new ClassDefNative() {
          Name = "BackEndAction",
          InheritsFromName = "Action",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "dialogTitle",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "dialogText",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "backEndTarget",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
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
            new UiAttributeDefinitionComplex() {
              Name = "Action",
              ComplexAttributeTypeName = "Action",
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
        new ClassDefNative() {
          Name = "LinkButton",
          InheritsFromName = "Button",
        },
        new ClassDefNative() {
          Name = "SpaContent",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Menu",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "Children",
              IsMandatory = true,
              IsMany = true,
              ComplexAttributeTypeName = "MenuItem",
            },
          },
        },
        new ClassDefNative() {
          Name = "MenuItem",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              IsMandatory = true,
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "url",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionComplex() {
              Name = "Children",
              IsPrimary = true,
              IsMany = true,
              ComplexAttributeTypeName = "MenuItem",
            },
          },
        },
      #endregion
    };

    #region Glue it Together
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
      library.AddDataTypeToComponentAssociation(DataTypes.Singleton.Timestamp, "TimestampEditor");

      library.SetComponentForEnums("DropDown");

      return library;
    }
    #endregion
  }
}
