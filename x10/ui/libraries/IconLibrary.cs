using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.ui.libraries {
  public class IconLibrary {

    private readonly static DataType _iconType = new DataTypeEnum(new string[] {
      "dollar-sign",
      "draft",
      "square-with-bubble",
      "ticket",
    }) {
      Name = "Icon",
    };

    private readonly static List<ClassDef> _classDefs = new List<ClassDef>() {
        new ClassDefNative() {
          Name = "Icon",
          InheritsFrom = ClassDefNative.Visual,
          IsMany = false,
          AtomicDataModel = DataTypes.Singleton.String,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionAtomic() {
              Name = "icon",
              DataType = _iconType,
            },
          }
        },
    };

    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static UiLibrary CreateLibrary() {
      UiLibrary library = new UiLibrary(_classDefs) {
        Name = "Icon Library",
      };

      library.AddDataTypeToComponentAssociation(_iconType, "Icon");

      return library;
    }
  }
}
