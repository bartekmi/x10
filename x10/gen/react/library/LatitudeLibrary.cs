using System.IO;
using System.Collections.Generic;
using System.Linq;

using x10.compiler.ui;
using x10.model.definition;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.metadata;
using x10.gen.react.attribute;
using x10.gen.react.generate;
using x10.gen.react.placeholder;

namespace x10.gen.react.library {
  internal class LatitudeLibrary {

    internal const string VISIBILITY_CONTROL = "VisibilityControl";

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      #region Primordial / Special Components
      new PlatformClassDef() {
        LogicalName = ClassDefNative.VisibilityControl.Name,
        PlatformName = VISIBILITY_CONTROL,
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
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
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "display"),
          new PlatformAttributeStatic("weight", "bold"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "children",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading2",
        PlatformName = "Text",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "headline"),
          new PlatformAttributeStatic("weight", "bold"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "children",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading3",
        PlatformName = "Text",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("scale", "title"),
          new PlatformAttributeStatic("weight", "bold"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "children",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "HorizontalDivider",
        PlatformName = "Separator",
        ImportDir = "react_lib",
      },
      new PlatformClassDef() {
        LogicalName = "VerticalDivider",
        PlatformName = "Separator",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("orientation", "vertical"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Bullet",
        PlatformName = "Text",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("children", "•"),
        },
      },
      #endregion

      #region Atomic Display Components
      new PlatformClassDef() {
        LogicalName = "TextualDisplay",
        PlatformName = "TextualDisplay",
        IsAbstract = true,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "weight",
            PlatformName = "weight",
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("normal", null),
              new EnumConversion("bold", "bold"),
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Text",
        PlatformName = "TextDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "text",
            PlatformName = "value",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntDisplay",
        // For now, we don't have a special component just for ints
        PlatformName = "FloatDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatDisplay",
        PlatformName = "FloatDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TimestampDisplay",
        PlatformName = "TimestampDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateDisplay",
        PlatformName = "DateDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "EnumDisplay",
        PlatformName = "EnumDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
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
      new PlatformClassDef() {
        LogicalName = "BooleanBanner",
        PlatformName = "BooleanBanner",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeDynamic("icon", "icon"),
        },
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextInput",
        ImportDir = "react_lib/latitude_wrappers",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
            AttributeUnnecessaryWhen = false,
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextareaInput",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new PlatformAttributeStatic("rows", 3),
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
            AttributeUnnecessaryWhen = false,
          },
          // Must explicitly write "onChange={() => { } }" when read-only since TextareaInput requires it
          // When writeable, this is handled by JavaScriptAttributeDynamic
          new JavaScriptAttributeByFunc() {
            PlatformName = "onChange",
            IsCodeSnippet = true,
            Function = (generator, instance) =>
              UiCompilerUtils.IsReadOnly(instance) ? "() => { }" : null,
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntEdit",
        // TODO: Use something more specific
        PlatformName = "FloatInput",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
            AttributeUnnecessaryWhen = false,
          },
          new PlatformAttributeStatic("decimalPrecision", 0),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        PlatformName = "FloatInput",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
            AttributeUnnecessaryWhen = false,
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "Checkbox",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
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
            AttributeUnnecessaryWhen = false,
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "onChange",
            PlatformName = "onChange",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateEditor",
        PlatformName = "CalendarDateInput",
        ImportDir = "react_lib/latitude_wrappers",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "readOnly",
            AttributeUnnecessaryWhen = false,
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

      #region for Enums
      // TODO: There are many subtleties around code-gen for this...
      // Binding to enum (with possible search)
      // Allow vs. not allow empty
      // Ordering (see logical 'order' attribute)
      // Not clear where the code for this belongs, as this is already a large file
      new PlatformClassDef() {
        LogicalName = "EnumSelection",
        PlatformName = "EnumSelection",
        IsAbstract = true,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "value",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "disabled",
            AttributeUnnecessaryWhen = false,
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "excludeItems",
            PlatformName = "excludeItems",
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
      new PlatformClassDef() {
        LogicalName = "DropDown",
        PlatformName = "SelectInput",
        InheritsFromName = "EnumSelection",
        ImportDir = "latitude/select",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
        }
      },
      new PlatformClassDef() {
        LogicalName = "RadioButtonGroup",
        PlatformName = "RadioGroup",
        InheritsFromName = "EnumSelection",
        ImportDir = "react_lib/enum",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("layout", "isInline") {
            TranslationFunc = (value) => value?.ToString() == "horizontal",
          },
        }
      },
      #endregion
      #endregion

      #region Layout Components
      #region 1-Dimensional
      new PlatformClassDef() {
        LogicalName = "VerticalStackPanel",
        PlatformName = "Group",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("flexDirection", "column"),
          new JavaScriptAttributeDynamic("gap", "gap"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "align",
            PlatformName = "alignItems",
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("left", null),
              new EnumConversion("center", "center"),
              new EnumConversion("right", "flex-end"),
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Row",
        PlatformName = "Group",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("gap", "gap"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "RepellingRow",
        PlatformName = "Group",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("justifyContent", "space-between"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Expander",
        PlatformName = "Expander",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "headerFunc",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              UiAttributeValueComplex headerAttr = instance.FindAttributeValue("Header") as UiAttributeValueComplex;
              Instance headerInstance = headerAttr.Instances.Single();
              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                generator.WriteLine(indent, "headerFunc={ () => (");
                generator.GenerateComponentRecursively(OutputType.React, indent + 1, headerInstance);
                generator.WriteLine(indent, ") }");
              });
            },
          },
        },
      },
      #endregion

      #region 2-Dimensional
      // TODO
      #endregion
      #endregion

      #region List / MultiStacker
      new PlatformClassDef() {
        LogicalName = "List",
        PlatformName = "MultiStacker",
        ImportDir = "react_lib/multi",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "items",
          },
          new JavaScriptAttributePrimaryAsProp() {
            CodeSnippet = (generator, indent, platClassDef, instance) => {
              Instance inner = instance.PrimaryValueInstance;

              generator.WriteLine(indent, "itemDisplayFunc={ (data, onChange) => (");

              generator.PushSourceVariableName("data");
              generator.GenerateComponentRecursively(OutputType.React, indent + 1, inner);
              generator.PopSourceVariableName();

              generator.WriteLine(indent, ") }");
            },
          },
          new JavaScriptAttributeDynamic("addItemLabel", "addItemLabel"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "addNewItem",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              generator.ImportsPlaceholder.ImportCreateDefaultFunc(instance.DataModelEntity);
              return ReactCodeGenerator.CreateDefaultFuncName(instance.DataModelEntity);
            },
          },
        },
      },
      #endregion

      #region Tabbed Pane
      new PlatformClassDef() {
        LogicalName = "TabbedPane",
        PlatformName = "TabbedPane",
        ImportDir = "react_lib/tab",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributePrimaryAsProp() {
            CodeSnippet = (generator, indent, platClassDef, instance) => {
              UiAttributeValueComplex primaryValue = (UiAttributeValueComplex)instance.PrimaryValue;
              generator.WriteLine(indent, "tabs={");
              generator.RenderComplexAttrAsJavascript(indent + 1, primaryValue);
              generator.WriteLine(indent, "}");
            },
          }
        },
      },
      new PlatformClassDef() {
        LogicalName = "Tab",
        PlatformName = "",  // Just a JavaScript object
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "id",
            Function = (instance) => instance.Index,
          },
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "displayFunc",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Instance inner = instance.PrimaryValueInstance;
              
              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                generator.WriteLine(indent, "displayFunc: () =>");
                generator.GenerateComponentRecursively(OutputType.React, indent + 1, inner);
                generator.WriteLine(indent, ",");
              });
            },
          },
        },
      },
      #endregion

      #region Table
      new PlatformClassDef() {
        LogicalName = "Table",
        PlatformName = "Table",
        ImportDir = "react_lib/table",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "data",
          },
          new JavaScriptAttributePrimaryAsProp() {
            CodeSnippet = (generator, indent, platClassDef, instance) => {
              UiAttributeValueComplex primaryValue = (UiAttributeValueComplex)instance.PrimaryValue;
              generator.WriteLine(indent, "columns={");
              generator.RenderComplexAttrAsJavascript(indent + 1, primaryValue);
              generator.WriteLine(indent, "}");
            },
          }
        }
      },
      new PlatformClassDef() {
        LogicalName = "TableColumn",
        PlatformName = "",  // Just a JavaScript object
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "id",
            Function = (instance) => instance.Index,
          },
          new JavaScriptAttributeDynamic("label", "Header"),
          new JavaScriptAttributeDynamic("width", "width") {
            DefaultValue = 140,
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "accessor",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Instance inner = instance.PrimaryValueInstance;

              if (inner is InstanceModelRef) {
                generator.PushSourceVariableName("data", true);
                string path = generator.GetReadOnlyBindingPath(instance);
                generator.PopSourceVariableName();

                return string.Format("(data) => {0}", path);
              } else 
                // If the instance is not a model-reference but just a full-on component,
                // we pass the entire row object to the "Cell" property - it may extract
                // several attributes/members.
                return "(data) => data";
            },
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "Cell",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Instance inner = instance.PrimaryValueInstance;
              
              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                generator.WriteLine(indent, "Cell: ({ value }) =>");
      
                generator.PushSourceVariableName("value", inner is InstanceModelRef);
                generator.GenerateComponentRecursively(OutputType.React, indent + 1, inner);
                generator.PopSourceVariableName();

                generator.WriteLine(indent, ",");
              });
            },
          },
        },
      },
      #endregion

      #region Button / Actions
      new PlatformClassDef() {
        LogicalName = "Button",
        PlatformName = "Button",
        ImportDir = "react_lib/latitude_wrappers",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "label",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "url",
            PlatformName = "url",
          },
        },
      },
      #endregion

      #region Menu
      new PlatformClassDef() {
        LogicalName = "Menu",
        PlatformName = "Menu",
        ImportDir = "react_lib/menu",
      },
      new PlatformClassDef() {
        LogicalName = "MenuItem",
        PlatformName = "MenuItem",
        ImportDir = "react_lib/menu",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "label",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "url",
            PlatformName = "href",
          },
        },
      },
      #endregion

      #region Display Form
      new PlatformClassDef() {
        LogicalName = "DisplayForm",
        PlatformName = "DisplayForm",
        ImportDir = "react_lib/form",
      },
      new PlatformClassDef() {
        LogicalName = "Label",
        PlatformName = "DisplayField",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "toolTip",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "label",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "maxWidth",
            PlatformName = "maxWidth",
          },
        },
      },
      #endregion

      #region (Edit) Form
      new PlatformClassDef() {
        LogicalName = "Form",
        PlatformName = "FormProvider",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "value",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Entity entity = instance.DataModelEntity;
              generator.ImportsPlaceholder.ImportCalculateErrorsFunc(entity);
              return string.Format("{{ errors: {0}({1}) }}",
                ReactCodeGenerator.CalculateErrorsFuncName(entity),
                generator.SourceVariableName);
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormSection",
        PlatformName = "FormSection",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("label", "label"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormField",
        PlatformName = "FormField",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "editorFor",
            Function = (generator, instance) => CodeGenUtils.GetBindingPathAsString(instance),
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "indicateRequired",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "toolTip",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "label",
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "maxWidth",
            PlatformName = "maxWidth",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormErrorDisplay",
        PlatformName = "FormErrorDisplay",
        ImportDir = "react_lib/form",
      },
      new PlatformClassDef() {
        LogicalName = "SubmitButton",
        PlatformName = "FormSubmitButton",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "onClick",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              return string.Format("() => save({0})", generator.SourceVariableName);
            },
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "Action",
            PlatformName = "action",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Action",
        PlatformName = "",  // Just a JavaScript object
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "successUrl",
            PlatformName = "successUrl",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormRow",
        PlatformName = "Group",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("gap", 40),
        },
      },
      #endregion

      #region Misc
      new PlatformClassDef() {
        LogicalName = "Dialog",
        PlatformName = "Dialog",
        ImportDir = "react_lib/modal",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("title", "title"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "openButton",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              UiAttributeValueComplex headerAttr = instance.FindAttributeValue("OpenButton") as UiAttributeValueComplex;
              Instance buttonInstance = headerAttr.Instances.Single();
              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                generator.WriteLine(indent, "openButton={");
                generator.GenerateComponentRecursively(OutputType.React, indent + 1, buttonInstance);
                generator.WriteLine(indent, "}");
              });
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "CancelDialogButton",
        PlatformName = "CancelDialogButton",
        ImportDir = "react_lib/modal",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("label", "label"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "AssociationEditor",
        PlatformName = "AssociationEditor",
        ImportDir = "react_lib/multi",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "id",
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "isNullable",
            Function = (generator, instance) => instance.ModelMember.IsMandatory == false,
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "query",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Entity refedEntity = (instance.ModelMember as Association).ReferencedEntity;
              string varName = ReactCodeGenerator.VariableName(refedEntity, true);
              generator.GqlPlaceholder.AddGqlQueryForAssociationEditor(refedEntity);
              return varName + "Query"; // Must match query name in GqlPlaceholder
            }
          },
          new PlatformAttributeStatic() {
            PlatformName = "toString",
            IsCodeSnippet = true,
            Value = "x => x.toStringRepresentation",
          },
          new JavaScriptAttributeDynamic("order", "order"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "AssociationDisplay",
        PlatformName = "TextDisplay",
        ImportDir = "react_lib/display",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "value",
            PlatformName = "value",
          },
        },
      },

      new PlatformClassDef() {
        LogicalName = "HelpIcon",
        PlatformName = "HelpTooltip",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "text",
          },
        },
      },

      new JavaScriptPlatformClassDef() {
        LogicalName = "SpaContent",
        PlatformName = "SpaContent",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "rootComponent",
            Function = (generator, instance) => {
              string rootComponent = instance.FindValue("rootComponent").ToString();
              return rootComponent + "Interface";
            },
            IsCodeSnippet = true,
          },
        },
        ProgrammaticallyGenerateChildren = (generatorGeneric, indent, PlatformClassDef, instance) => {
          ReactCodeGenerator generator = (ReactCodeGenerator)generatorGeneric;
          generator.ImportsPlaceholder.Import("Route", "react-router-dom", ImportLevel.ThirdParty);

          foreach (ClassDefX10 classDef in generator.AllUiDefinitions.All) {
            if (classDef.Url == null)
              continue;

            string compInterface = classDef.Name + "Interface";
            generator.ImportsPlaceholder.ImportDefault(classDef, "Interface");

            if (classDef.IsMany)
              generator.WriteLine(indent, "<Route exact path='{0}' component={ {1} } />",
                classDef.Url,
                compInterface);
            else if (ReactCodeGenerator.IsForm(classDef)) {
              generator.WriteLine(indent, "<Route exact path='{0}/edit/:id' component={ {1} } />",
                classDef.Url,
                compInterface);

              generator.WriteLine(indent, "<Route exact path='{0}/new' component={ {1} } />",
                classDef.Url,
                compInterface);
            } else
              generator.WriteLine(indent, "<Route exact path='{0}/:id' component={ {1} } />",
                classDef.Url,
                compInterface);
          }
        }
      },
      # endregion
    };

    #region Singleton
    private static PlatformLibrary _singleton;
    public static PlatformLibrary Singleton() {
      if (_singleton == null)
        _singleton = new PlatformLibrary(BaseLibrary.Singleton(), definitions) {
          Name = "Latitude",
          ImportPath = "latitude",
        };
      return _singleton;
    }
    #endregion
  }
}
