using System.IO;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.metadata;

namespace x10.gen.react {
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
