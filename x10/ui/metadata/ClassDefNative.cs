using System;
using System.Collections.Generic;
using System.Text;


using x10.model.metadata;

namespace x10.ui.metadata {
  public class ClassDefNative : ClassDef {
    public string ImportParth { get; set; }
    public string HelpUrl { get; set; }

    public ClassDefNative() {
      // Do nothing
    }

    #region Primordial Components
    public static ClassDefNative Object = new ClassDefNative() {
      Name = "ClassDefObject",
    };

    public static ClassDefNative Visual = new ClassDefNative() {
      Name = "ClassDefVisual",
      InheritsFrom = Object,
      LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "visible",
              Description = "Is the object visible on the UI?",
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = false,
            },
          }
    };

    public static ClassDefNative Editable = new ClassDefNative() {
      Name = "ClassDefVisual",
      InheritsFrom = Visual,
      LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "editable",
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = true,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "mandatory",
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = true,
            },
          }
    };

    public static ClassDefNative RawHtml = new ClassDefNative() {
      Name = "RawHtml",
      Description = "A placeholder within which you can put raw HTML which will be rendered in the UI. No validation will be performed by the compiler.",
      InheritsFrom = Visual,
    };

    public static ClassDefNative State = new ClassDefNative() {
      Name = "State",
      Description = "Defines a single state variable on a UI Component",
      InheritsFrom = Object,
      LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "variable",
              DataType = DataTypes.Singleton.String,
              IsMandatory = true,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "dataType",
              DataType = DataTypes.Singleton.String,
              IsMandatory = true,
              Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
                string typeName = xmlScalar.Value.ToString();
                allEnums.FindDataTypeByNameWithError(typeName, xmlScalar);
              }
            },
            new UiAttributeDefinitionAtomic() {
              Name = "default",
              DataType = DataTypes.Singleton.String,
              DefaultValue = true,
            },
          }
    };

    public static ClassDefNative UiClassDefClassDef = new ClassDefNative() {
      Name = "UiClassDef",
      Description = "This 'meta' class definition actually specifies what attributes are allowed on a UI Class Def",
      InheritsFrom = Object,
      LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = "state",
              ComplexAttributeType = State,
              IsMany = true,
            },
          }
    };
    #endregion
  }
}
