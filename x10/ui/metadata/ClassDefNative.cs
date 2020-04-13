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
    };
    #endregion
  }
}
