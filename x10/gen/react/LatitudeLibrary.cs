using System.Collections.Generic;

using x10.parsing;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.gen.wpf.codelet;
using x10.model.definition;

namespace x10.gen.react {
  internal class LatitudeLibrary {

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      #region No-Data Formatting Components
      new PlatformClassDef() {
        LogicalName = "Heading1",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "display"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading2",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "headline"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading3",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "title"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "HorizontalDivider",
        PlatformName = "Separator",
      },
      new PlatformClassDef() {
        LogicalName = "Bullet",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("children", "•"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Label",
        PlatformName = "Label",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "indicateRequired",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "helpTooltip",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "value",
          },
        },
      },
      #endregion

      #region  Atomic Text Display Components
      new PlatformClassDef() {
        LogicalName = "Text",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "weight",
            PlatformName = "weight",
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("normal", null),
              new EnumConversion("bold", "bold"),
            },
          },
        },
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextareaInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeStatic("rows", "{3}"),
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntEdit",
        // TODO: Use something more specific
        PlatformName = "FloatInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
          new PlatformAttributeStatic() {
            PlatformName = "decimalPrecision",
            Value = "0",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        PlatformName = "FloatInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "Checkbox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            LogicalName = "checked",
            PlatformName = "checked",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "checkboxLabel",
            PlatformName = "label",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "disabled",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateEditor",
        PlatformName = "X10_CalendarDateInput",
        ImportDir = "react_lib",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      // Need to create my own control in lib which converts the Text property to DateTime
      //new PlatformClassDef() {
      //  LogicalName = "TimestampEditor",
      //  PlatformName = "DateTimeInput",
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
      // Not clear where the code for this belongs, as this is already a large file
      new PlatformClassDef() {
        LogicalName = "DropDown",
        PlatformName = "SelectInput",
        ImportDir = "latitude/select",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "value",
          },
          new PlatformAttributeByFunc() {
            PlatformName = "options",
            Function = (instance) => {
              string dataType = (instance.ModelMember as X10Attribute)?.DataType?.Name;
              return string.Format("{0}EnumPairs", dataType);
            }
          },
        },
      },
      #endregion

      #region Layout Components
      #region 1-Dimensional
      new PlatformClassDef() {
        LogicalName = "VerticalStackPanel",
        PlatformName = "Group",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("flexDirection", "column"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Row",
        PlatformName = "Group",
      },
      new PlatformClassDef() {
        LogicalName = "RepellingRow",
        PlatformName = "Group",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("justifyContent", "space-between"),
        },
      },
      #endregion

      #region 2-Dimensional
      // TODO
      #endregion
      #endregion

      #region Table
      new PlatformClassDef() {
        LogicalName = "Table",
        PlatformName = "Table",
        ImportDir = "latitude/table",
        PrimaryAttributeWrapperProperty = "columnDefinitions",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "data",
          },
          new PlatformAttributeStatic("getUniqueRowId", "{row => row.id}"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "TableColumn",
        PlatformName = "",  // Just a JavaScript object
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDataBind() {
            PlatformName = "render",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "header",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "width",
            PlatformName = "width",
          },
        },
      },
      #endregion

      # region Misc
      new PlatformClassDef() {
        LogicalName = "HelpIcon",
        PlatformName = "HelpTooltip",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "text",
          },
        },
      },
      # endregion

      #region Button / Actions
      new PlatformClassDef() {
        LogicalName = "Button",
        PlatformName = "Button",
        ImportDir = "latitude/button",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "text",
          },
          // new PlatformAttributeStatic() {
          //   PlatformName = "Click",
          //   Value = "NavigateToUrlInTag",
          // },
          // new PlatformAttributeDynamic() {
          //   LogicalName = "url",
          //   PlatformName = "Tag",
          // },
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
          new PlatformAttributeByFunc() {
            PlatformName = "Click",
            Function = (instance) => instance.HasAttributeValue("url") ? "NavigateToUrlInTag" : null
          },
          new PlatformAttributeDataBind() {
            LogicalName = "label",
            PlatformName = "Header",
          },
          new PlatformAttributeDynamic() {
            LogicalName = "url",
            PlatformName = "Tag",
          },
        },
      },
      #endregion

      #region Form
      new PlatformClassDef() {
        LogicalName = "Form",
        PlatformName = "FormProvider",
        ImportDir = "react_lib/form",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("value", "{[]}"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormSection",
        PlatformName = "FormSection",
        ImportDir = "react_lib/form",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeDynamic("label", "label"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormErrorDisplay",
        PlatformName = "FormErrorDisplay",
        ImportDir = "react_lib/form",
      },
      new PlatformClassDefWithCodelet() {
        LogicalName = "SubmitButton",
        PlatformName = "FormSubmitButton",
        ImportDir = "react_lib/form",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("onClick","{() => save(editedObject)}"),
        },
      },
      #endregion
    };

    #region Glue it Together
    private static PlatformLibrary _singleton;
    public static PlatformLibrary Singleton(MessageBucket errors, UiLibrary logicalLibrary) {
      if (_singleton == null) 
        _singleton = CreateLibrary(errors, logicalLibrary);
      return _singleton;
    }

    private static PlatformLibrary CreateLibrary(MessageBucket errors, UiLibrary logicalLibrary) {
      PlatformLibrary library = new PlatformLibrary(BaseLibrary.Singleton(), definitions) {
        ImportPath = "latitude",
      };

      library.HydrateAndValidate(errors, logicalLibrary);

      return library;
    }
    #endregion
  }
}
