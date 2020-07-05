using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class IconLibrary {

    private static readonly string[] iconNames = new string[] {
      "dollar-sign",
      "draft",
      "square-with-bubble",
      "ticket",
      "boat",
      "airplane",
      "truck",
      "lightning",
      "port",
      "airport",
      "factory",
      "quotation-mark",
      "question-mark",
      "callout",
      "thumbs-up",
      "dont",
      "upload",
      "task",
      "human",
      "credit-card",
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
      model.libraries.BaseLibrary.ICON_DATA_TYPE.EnumValueValues = iconNames;

      return _singleton;
    }

  }
}
