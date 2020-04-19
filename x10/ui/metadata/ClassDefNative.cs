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
              DataType = DataTypes.Singleton.Boolean,
              DefaultValue = false,
            },
            new UiAttributeDefinitionAtomic() {
              Name = "editable",
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
    #endregion
  }
}
