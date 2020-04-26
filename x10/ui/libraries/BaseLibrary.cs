using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
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
        new ClassDefNative() {
          Name = "Label",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "Content",
              IsPrimary = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "mandatoryIndicator",
              DataType = new DataTypeEnum(new string[] {"none", "mandatory", "optional" }),
              DefaultValue = "none",
            },
            new UiAttributeDefinitionAtomic() {
              Name = "toolTip",
              DataType = DataTypes.Singleton.String,
            },
          }
        },
      #endregion

      #region  Atomic Display Components
      new ClassDefNative() {
          Name = "Text",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "text",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
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
          InheritsFromName = "Text",
          IsMany = false,
        },
      #endregion

      #region Atomic Edit Components
      new ClassDefNative() {
        Name = "TextEdit",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.String,
      },
      new ClassDefNative() {
        Name = "TextArea",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.String,   
      },
      new ClassDefNative() {
        Name = "IntEdit",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Integer,
      },
      new ClassDefNative() {
        Name = "FloatEdit",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Integer,
      },
      new ClassDefNative() {
        Name = "Checkbox",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Boolean,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "checked",
            DataType = DataTypes.Singleton.Boolean,
          },
        },
      },
      new ClassDefNative() {
        Name = "BooleanViaButtons",
        InheritsFrom = ClassDefNative.Visual,
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Boolean,
      },
      new ClassDefNative() {
        Name = "DateEditor",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Date,
      },
      new ClassDefNative() {
        Name = "TimestampEditor",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = DataTypes.Singleton.Timestamp,
      },
      new ClassDefNative() {
        Name = "DropDown",
        InheritsFromName = "Text",
        IsMany = false,
        AtomicDataModel = new DataTypeEnum(),
      },
      #endregion

      #region Association Edit Components
      new ClassDefNative() {
        Name = "AssociationEditor",
        InheritsFrom = ClassDefNative.Visual,
        IsMany = false,
      },

      #endregion

      #region Layout Components

      #region Vertical
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
      #endregion
      
      #region Horizontal
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

      #region 2-Dimensional
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

      #region Grid
      new ClassDefNative() {
          Name = "Grid",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionComplex() {
              Name = "Columns",
              IsMany = true,
              ComplexAttributeTypeName = "GridColumn",
            },
            new UiAttributeDefinitionComplex() {
              Name = "Rows",
              IsMany = true,
              ComplexAttributeTypeName = "GridRow",
            },
            new UiAttributeDefinitionAtomic() {
              Name = "column",
              IsAttached = true,
              DataType = DataTypes.Singleton.Integer,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "row",
              IsAttached = true,
              DataType = DataTypes.Singleton.Integer,
            },
          }
        },
        new ClassDefNative() {
          Name = "GridColumn",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "width",
              DataType = DataTypes.Singleton.Float,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "fraction",
              DataType = DataTypes.Singleton.Float,
            },
          },
        },
        new ClassDefNative() {
          Name = "GridRow",
          InheritsFrom = ClassDefNative.Object,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "height",
              DataType = DataTypes.Singleton.Float,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "fraction",
              DataType = DataTypes.Singleton.Float,
            },
          },
        },
      #endregion
      #endregion

      #endregion

      #region List
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
            new UiAttributeDefinitionAtomic() {
              Name = "addItemLabel",
              DataType = DataTypes.Singleton.String,
              DefaultValue = "Add item",
            },
          },
        },
      #endregion

      #region Table
      new ClassDefNative() {
          Name = "Table",
          ComponentDataModel = Entity.Object,
          IsMany = true,
          InheritsFrom = ClassDefNative.Visual,
          ModelRefWrapperComponentName = "TableColumn",
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
          Name = "TableSelectionColumn",
          InheritsFromName = "TableColumn",
        },
        new ClassDefNative() {
          Name = "TablePageControls",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
      #endregion

      #region Button / Actions
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
          Name = "ActionWithDialog",
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
              Name = "danger",
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = false,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "successUrl",
              DataType = DataTypes.Singleton.String,
            },
            // TODO: Add optional confirm and cancel text
          },
        },
        new ClassDefNative() {
          Name = "UploadAction",
          InheritsFromName = "ActionWithDialog",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
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
          InheritsFromName = "ActionWithDialog",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
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
            new UiAttributeDefinitionAtomic() {
              Name = "url",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionComplex() {
              Name = "Action",
              ComplexAttributeTypeName = "Action",
            },
          },
          // TODO: Add cross-validation... Either action or url must be provided
        },
        new ClassDefNative() {
          Name = "SelectableButton",
          InheritsFromName = "Button",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "selected",
              DataType = DataTypes.Singleton.Boolean,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "onSelect",
              // TODO: Consider introducing a new data-type: EventHandler
              DataType = DataTypes.Singleton.String,
            },
          },
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
      #endregion

      #region Menu
        new ClassDefNative() {
          Name = "Menu",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "Children",
              IsPrimary = true,
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

      #region Form
        new ClassDefNative() {
          Name = "Form",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "Child",
              IsPrimary = true,
              IsMandatory = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          },
        },
        new ClassDefNative() {
          Name = "FormSection",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "Child",
              IsPrimary = true,
              IsMandatory = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "label",
              DataType = DataTypes.Singleton.String,
              IsMandatory = true,
            },
          },
        },
        new ClassDefNative() {
          Name = "FormButton",
          InheritsFromName = "Button",
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "validate",
              IsMandatory = true,
              DataType = DataTypes.Singleton.Boolean,
            },
          },
        },

      #endregion

      #region Primordial Components (Not Abstract)
      ClassDefNative.RawHtml,
      ClassDefNative.State,
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
