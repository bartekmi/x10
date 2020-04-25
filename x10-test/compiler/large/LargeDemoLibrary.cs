using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.metadata;

namespace x10.compiler {
  public class LargeDemoLibrary {

    private readonly static List<ClassDef> definitions = new List<ClassDef>() {
        new ClassDefNative() {
          Name = "Metadata",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "MetadataEditor",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "Map",
          InheritsFrom = ClassDefNative.Visual,
          // ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "Calendar",
          InheritsFrom = ClassDefNative.Visual,
          // TODO: ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "BookmarkEditor",
          InheritsFrom = ClassDefNative.Visual,
          // TODO: ComponentDataModel...
        },
        new ClassDefNative() {
          Name = "FlexportLogo",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Messages",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Avatar",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "SearchBox",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Alerts",
          InheritsFrom = ClassDefNative.Visual,
        },
        new ClassDefNative() {
          Name = "Filters",
          InheritsFrom = ClassDefNative.Visual,
          LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
            new UiAttributeDefinitionComplex() {
              IsPrimary = true,
              Name = "Children",
              IsMany = true,
              ComplexAttributeType = ClassDefNative.Visual,
            },
          }
        },
        new ClassDefNative() {
          Name = "ShipmentProgressIndicator",
          InheritsFrom = ClassDefNative.Visual,
        },

      #region Filter Stuff
      new ClassDefNative() {
        Name = "FilterMultiSelect",
        InheritsFrom = ClassDefNative.Visual,
        LocalAttributeDefinitions = new List<UiAttributeDefinition>() {
          new UiAttributeDefinitionAtomic() {
            Name = "label",
            IsMany = true,
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "path",
            DataType = DataTypes.Singleton.String,
          },
          new UiAttributeDefinitionAtomic() {
            Name = "choices",
            IsMany = true,
            DataType = DataTypes.Singleton.String,
          },
        },
      },
      new ClassDefNative() {
        Name = "FilterSingleSelect",
        InheritsFrom = ClassDefNative.Visual,
      },
      new ClassDefNative() {
        Name = "FilterRadioButtons",
        InheritsFrom = ClassDefNative.Visual,
      },
      new ClassDefNative() {
        Name = "FilterSearchableProperty",
        InheritsFrom = ClassDefNative.Visual,
      },
      #endregion
    };

    private static UiLibrary _singleton;
    public static UiLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static UiLibrary CreateLibrary() {
      UiLibrary library = new UiLibrary(definitions) {
        Name = "Large Demo Library",
      };

      return library;
    }
  }
}
