using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class FlexportSpecialLibrary {

    private readonly static List<ClassDef> _classDefs = new List<ClassDef>() {
      new ClassDefNative() {
        Name = "Status",
        Description = "Display one of several statuses as a colored 'Pill' with an icon",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            IsPrimary = true, 
            Name = "text",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "intent",
            DataType = new DataTypeEnum("StatusIntent", new string[] { "pending", "active", "error", "done", "complete", "due-soon"} ),
          },
        }
      },
    };

    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = new UiLibrary(_classDefs) {
          Name = "Flexport Special Library",
        };

      return _singleton;
    }

  }
}
