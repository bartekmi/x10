using System;
using System.Collections.Generic;
using System.Text;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;

namespace x10.ui.metadata {
  public class ClassDefNative : ClassDef {
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
              IsMandatory = false,
              Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
                string typeName = xmlScalar.Value.ToString();
                allEnums.FindDataTypeByNameWithError(typeName, xmlScalar);
              }
            },
            new UiAttributeDefinitionAtomic() {
              Name = "model",
              DataType = DataTypes.Singleton.String,
              IsMandatory = false,
              Pass1Action = (messages, allEntities, allEnums, xmlScalar, uiComponent) => {
                string typeName = xmlScalar.Value.ToString();
                allEntities.FindEntityByNameWithError(typeName, xmlScalar);
              }
            },
            new UiAttributeDefinitionAtomic() {
              Name = "default",
              DataType = DataTypes.Singleton.String,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "many",
              DataType = DataTypes.Singleton.Boolean,
            },
          }
    };

    // The fact that I have to create this class is lame!
    // -- extra code + nasty departure from the DRY principle.
    // Ideally, we should have a way to reflect a C# class definition and directly generate the
    // corresponding ClassDefNative definition as above
    public class StateClass {
      public string Variable { get; private set; }
      public DataType DataType { get; private set; }
      public Entity Entity { get; private set; }
      public bool IsMany { get; private set; }
      public string Default { get; private set; } // This is iffy - could by any type or formula - just like on the model side

      public static StateClass FromInstance(AllEntities allEntities, AllEnums allEnums, Instance instance) {
        string dataType = instance.FindValue<string>("dataType");
        string model = instance.FindValue<string>("model");

        return new StateClass() {
          Variable = instance.FindValue<string>("variable"),
          DataType = dataType == null ? null : allEnums.FindDataTypeByName(dataType),
          Entity = model == null ? null : allEntities.FindEntityByName(model),
          IsMany = instance.FindValue<bool>("many"),
          Default = instance.FindValue<string>("default"),
        };
      }

      public X10DataType ToX10DataType() {
        if (Entity != null)
          return new X10DataType(Entity, IsMany);
        else if (DataType != null)
          return new X10DataType(DataType);
        else
          return X10DataType.ERROR;
      }
    }

    public const string STATE_ATTRIBUTE = "State";
    public static ClassDefNative UiClassDefClassDef = new ClassDefNative() {
      Name = "UiClassDef",
      Description = "This 'meta' class definition actually specifies what attributes are allowed on a UI Class Def",
      InheritsFrom = Object,
      LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              Name = STATE_ATTRIBUTE,
              ComplexAttributeType = State,
              IsMany = true,
            },
          }
    };
    #endregion
  }
}
