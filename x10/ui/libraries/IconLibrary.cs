using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class IconLibrary {

    // Source: https://www.flexport.com/design/components/iconography/system-icons
    private static readonly string[] iconNames = new string[] {
      "airport",
      "attach",
      "attention",
      "callout",
      "check",
      "credit-card",
      "dollar-sign",
      "dont",
      "draft",
      "factory",
      "human",
      "lightning",
      "plane",
      "port",
      "question",
      "quotation-mark",
      "rail",
      "ship",
      "square-with-bubble",
      "star",
      "task",
      "thumbs-up",
      "ticket",
      "truck",
      "upload",
      "warehouse",
    };

    private readonly static List<ClassDef> _classDefs = new List<ClassDef>() {
      // If there were components, they would go here.
      // I left this here to give an example of how an extran library would look like.
    };

    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = new UiLibrary(_classDefs) {
          Name = "Icon Library",
        };

      // Inject icon names into the existing type... The idea is that the actual 
      // icons may be project-specific
      x10.model.libraries.BaseLibrary.ICON_DATA_TYPE.EnumValueValues = iconNames;

      return _singleton;
    }

  }
}
