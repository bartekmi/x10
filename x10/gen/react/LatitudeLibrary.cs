using System.Collections.Generic;
using System.Linq;

using x10.parsing;
using x10.gen.wpf.codelet;
using x10.model.definition;

using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.metadata;


namespace x10.gen.react {
  internal class LatitudeLibrary {

    internal const string VISIBILITY_CONTROL = "VisibilityControl";

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      #region Primordial / Special Components
      new PlatformClassDef() {
        LogicalName = ClassDefNative.VisibilityControl.Name,
        PlatformName = VISIBILITY_CONTROL,
        ImportDir = "react_lib",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = ClassDefNative.ATTR_VISIBLE,
            PlatformName = "visible",
          },
        },
      },
      #endregion

      #region No-Data Formatting Components
      new PlatformClassDef() {
        LogicalName = "Heading1",
        PlatformName = "Text",
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "display"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "children",
          },
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
        ImportDir = "react_lib",
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
          new JavaScriptAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "indicateRequired",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "helpTooltip",
          },
          new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            LogicalName = "weight",
            PlatformName = "weight",
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("normal", null),
              new EnumConversion("bold", "bold"),
            },
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "children",
          },
        },
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextareaInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new PlatformAttributeStatic("rows", 3),
          new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
          new PlatformAttributeStatic("decimalPrecision", 0),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        PlatformName = "FloatInput",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "Checkbox",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "checked",
            PlatformName = "checked",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "checkboxLabel",
            PlatformName = "label",
          },
          new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "options",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              DataTypeEnum dataType = (instance.ModelMember as X10Attribute)?.DataType as DataTypeEnum;
              if (dataType == null) {
                generator.Messages.AddError(instance.XmlElement, "Expected data type to be an enum");
                return null;
              }

              string pairsConstant = ReactCodeGenerator.EnumToPairsConstant(dataType);

              generator.ImportsPlaceholder.Import(pairsConstant, dataType);

              return pairsConstant;
            },
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
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "data",
          },
          new PlatformAttributeStatic("getUniqueRowId", "row => row.id", true),
        },
      },
      new PlatformClassDef() {
        LogicalName = "TableColumn",
        PlatformName = "",  // Just a JavaScript object
        PlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "id",
            Function = (instance) => instance.FindValue("label")?.ToString(),
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "render",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Instance inner = (instance.PrimaryValue as UiAttributeValueComplex).Instances.Single();      
              Member member = inner.ModelMember;
              if (member == null) {
                return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                  generator.WriteLine(indent, "render: (data) =>");
                  generator.GenerateComponentRecursively(ReactCodeGenerator.OutputType.React, indent + 1, inner);
                  generator.WriteLine(indent, ",");
                });
              }

              generator.ImportsPlaceholder.ImportDefault("latitude/table/TextCell");

              return string.Format("(data) => <TextCell value={{ data.{0} }} />",
                PlatformUtils.ComposePath(instance)
              );
            },
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "header",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "width",
            PlatformName = "width",
            DefaultValue = 140,
          },
        },
      },
      #endregion

      #region Misc
      new PlatformClassDef() {
        LogicalName = "HelpIcon",
        PlatformName = "HelpTooltip",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "text",
          },
          // new PlatformAttributeStatic() {
          //   PlatformName = "Click",
          //   Value = "NavigateToUrlInTag",
          // },
          // new JavaScriptAttributeDynamic() {
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
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "label",
            PlatformName = "Header",
          },
          new JavaScriptAttributeDynamic() {
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
          new PlatformAttributeStatic("value", "[]", true),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormSection",
        PlatformName = "FormSection",
        ImportDir = "react_lib/form",
        PlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("label", "label"),
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
          new PlatformAttributeStatic("onClick","() => save(editedObject)", true),
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
