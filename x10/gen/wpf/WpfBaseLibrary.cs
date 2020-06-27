using System;
using System.Collections.Generic;
using System.Text;

using x10.ui.libraries;
using x10.ui.platform;

namespace x10.gen.wpf {
  internal class WpfBaseLibrary {
    #region Base Classes: Visual
    readonly static PlatformClassDef Visual = new PlatformClassDef() {
      LogicalName = "ClassDefVisual",
      PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "visible",
            PlatformName = "Visibility",
            Converter = "BooleanToVisibilityConverter",
          },
        },
    };
    #endregion

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      Visual,

      #region No-Data Formatting Components
      new PlatformClassDef() {
        LogicalName = "Heading1",
        PlatformName = "TextBlock",
        StyleInfo = "Heading1",
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
        LogicalName = "Heading2",
        PlatformName = "TextBlock",
        StyleInfo = "Heading2",
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
        LogicalName = "Heading3",
        PlatformName = "TextBlock",
        StyleInfo = "Heading3",
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
        LogicalName = "HorizontalDivider",
        PlatformName = "Separator",
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
        LogicalName = "Bullet",
        PlatformName = "TextBlock",
        InheritsFrom = Visual,
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Text",
            Value = "•",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Label",
        PlatformName = "lib:Label",
        InheritsFrom = Visual,
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "IsMandatory",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "MyToolTip",
          },
          // TODO: Deal with the fact that in the logical model, the Label element
          // "wraps" its content.
          // Perhaps an attribute on PlatformClassDef that says "Unwrap Content"???
          // Or else a custom code-generation Action<>
        },
      },
      #endregion

      #region  Atomic Text Display Components
      new PlatformClassDef() {
        LogicalName = "Text",
        PlatformName = "TextBlock",
        InheritsFrom = Visual,
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "weight",
            PlatformName = "FontWeight",
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("normal", null),
              // Note that it would be nice to use the actual WPF object here, but that
              // would require splitting this code off into its own project and MEF'ing
              // it in for the actual code generation (or trigger the code generation from
              // that project)
              new EnumConversion("bold", "Bold"),
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Pill",
        InheritsFrom = Visual,
        // TODO... Create the actual pill component in lib
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "Text",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "Text",
          },
          new PlatformAttributeStatic() {
            PlatformName = "TextWrapping",
            Value = "Wrap",
          },
          new PlatformAttributeStatic() {
            PlatformName = "AcceptsReturn",
            Value = "True",
          },
          new PlatformAttributeStatic() {
            PlatformName = "MinLines",
            Value = "3",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntEdit",
        // TODO: Use something more specific
        PlatformName = "TextBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "Text",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        // TODO: Use something more specific
        PlatformName = "TextBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "Text",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "CheckBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            LogicalName = "checked",
            PlatformName = "IsChecked",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "BooleanViaButtons",
        PlatformName = "lib:BooleanViaButtons",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "Selected",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateEditor",
        PlatformName = "DatePicker",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "SelectedDate",
          },
        },
      },
      // Need to create my own control in lib which converts the Text property to DateTime
      // https://stackoverflow.com/questions/10658472/datetimepicker-for-wpf-4-0
      //new PlatformClassDef() {
      //  LogicalName = "TimestampEditor",
      //  PlatformName = "DateTimePicker",
      //  PlatformAttributes = new List<PlatformAttribute>() {
      //    new PlatformAttributeDataBind() {
      //      PlatformName = "SelectedDate",
      //    },
      //  },
      //},

      // TODO: There are many subtleties around code-gen for this...
      // Binding to enum (with possible search)
      // Allow vs. not allow empty
      // Ordering (see logical 'order' attribute)
      // Note clear where the code for this belongs, as this is already a large file
      new PlatformClassDef() {
        LogicalName = "DropDown",
        PlatformName = "ComboBox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "SelectedItem",
          },
        },
      },
      #endregion

      #region Layout Components
      #region Vertical
      new PlatformClassDef() {
        LogicalName = "VerticalStackPanel",
        PlatformName = "StackPanel",
      },
      #endregion

      #region Horizontal
      new PlatformClassDef() {
        LogicalName = "Row",
        PlatformName = "StackPanel",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Orientation",
            Value = "Horizontal",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "RepellingRow",
        PlatformName = "Grid",
        // TODO: Add generation code 
      },
      #endregion

      #region 2-Dimensional
      new PlatformClassDef() {
        LogicalName = "Grid",
        PlatformName = "Grid",
        // TODO: Add generation code 
      },
      #endregion
      #endregion

      #region Button / Actions
      new PlatformClassDef() {
        LogicalName = "HelpIcon",
        PlatformName = "TextBlock",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Text",
            Value = "(?)",
          },
          new PlatformAttributeStatic() {
            PlatformName = "Padding",
            Value = "10,0,0,0",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "ToolTip",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Button",
        PlatformName = "Button",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "Content",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "SelectableButton",
        PlatformName = "ToggleButton",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            LogicalName = "selected",
            PlatformName = "IsChecked",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "onSelect",
            PlatformName = "Checked",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "onDeselect",
            PlatformName = "OnUnchecked",
          },
        },
      },
      #endregion

      #region Menu
      new PlatformClassDef() {
        LogicalName = "Menu",
        PlatformName = "Menu",
      },
      new PlatformClassDef() {
        LogicalName = "MenuItem",
        PlatformName = "MenuItem",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            LogicalName = "label",
            PlatformName = "Header",
          },
        },
      },
      #endregion

      #region Form
      new PlatformClassDef() {
        LogicalName = "Form",
        PlatformName = "lib:Form",
      },
      new PlatformClassDef() {
        LogicalName = "FormSection",
        PlatformName = "lib:FormSection",
      },
      #endregion
    };

    #region Glue it Together
    private static PlatformLibrary _singleton;
    public static PlatformLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static PlatformLibrary CreateLibrary() {
      PlatformLibrary library = new PlatformLibrary(BaseLibrary.Singleton(), definitions) {
        ImportPath = "xmlns:lib=\"clr-namespace:wpf_lib.lib;assembly=wpf_lib\"",
      };

      return library;
    }
    #endregion
  }
}
