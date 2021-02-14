using System.Collections.Generic;

using x10.ui.libraries;
using x10.ui.platform;
using x10.gen.react.attribute;

namespace x10.gen.react.library {
  internal class LatitudeFlexportSpecialLibrary {

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      new PlatformClassDef() {
        LogicalName = "Status",
        PlatformName = "Status",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "intent",
            PlatformName = "intent",
          },
        },
      },
    };

    #region Singleton
    private static PlatformLibrary _singleton;
    public static PlatformLibrary Singleton() {
      if (_singleton == null)
        _singleton = new PlatformLibrary(FlexportSpecialLibrary.Singleton(), definitions) {
          Name = "Latitude Flexport Special",
          ImportPath = "latitude",
        };
      return _singleton;
    }
    #endregion
  }
}
