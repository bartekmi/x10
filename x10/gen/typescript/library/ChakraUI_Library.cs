using System;
using System.Linq;
using System.Collections.Generic;

using x10.compiler;
using x10.compiler.ui;
using x10.ui;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.definition;
using x10.model.metadata;
using x10.gen.typescript.attribute;
using x10.gen.typescript.generate;
using x10.gen.typescript.placeholder;

namespace x10.gen.typescript.library {
  internal class ChakraUI_Library {

    private readonly static List<PlatformClassDef> definitions = new List<PlatformClassDef>() {

      #region Primordial / Special Components
      new PlatformClassDef() {
        LogicalName = ClassDefNative.StyleControl.Name,
        PlatformName = "StyleControl",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic(ClassDefNative.ATTR_VISIBLE, "visible"),
          new JavaScriptAttributeDynamic("width", "width"),
          new JavaScriptAttributeDynamic("maxWidth", "maxWidth"),
          new JavaScriptAttributeDynamic("margin", "margin"),
          new JavaScriptAttributeDynamic("marginTop", "marginTop"),
          new JavaScriptAttributeDynamic("marginRight", "marginRight"),
          new JavaScriptAttributeDynamic("marginBottom", "marginBottom"),
          new JavaScriptAttributeDynamic("marginLeft", "marginLeft"),
          new JavaScriptAttributeDynamic("padding", "padding"),
          new JavaScriptAttributeDynamic("paddingTop", "paddingTop"),
          new JavaScriptAttributeDynamic("paddingRight", "paddingRight"),
          new JavaScriptAttributeDynamic("paddingBottom", "paddingBottom"),
          new JavaScriptAttributeDynamic("paddingLeft", "paddingLeft"),
          new JavaScriptAttributeDynamic("borderColor", "borderColor"),
          new JavaScriptAttributeDynamic("borderWidth", "borderWidth"),
          new JavaScriptAttributeDynamic("fillColor", "fillColor"),
        },
      },
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
          new JavaScriptAttributeDynamic() {
            LogicalName = "textColor",
            PlatformName = "textColor",
          },
        },
      },
      #endregion

      #region No-Data Formatting Components
      // https://chakra-ui.com/docs/components/heading
      new PlatformClassDef() {
        LogicalName = "Heading1",
        PlatformName = "Heading",
        IsNonDefaultImport = true,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("as", "h1"),
          new PlatformAttributeStatic("size", "4xl"),
          new PlatformAttributeStatic("noOfLines", 1),
          new JavaScriptAttributeDynamic("text", "children"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading2",
        PlatformName = "Heading",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("as", "h2"),
          new PlatformAttributeStatic("size", "2xl"),
          new JavaScriptAttributeDynamic("text", "children"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Heading3",
        PlatformName = "Heading",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("as", "h3"),
          new PlatformAttributeStatic("size", "lg"),
          new JavaScriptAttributeDynamic("text", "children"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "HorizontalDivider",
        PlatformName = "Separator",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeDynamic("color", "color"),
        },
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
          new JavaScriptAttributeDynamic("decimalPlaces", "decimalPlaces"),
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
          new JavaScriptAttributeDynamic("hideLabelIfIconPresent", "hideLabelIfIconPresent"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "options",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              DataTypeEnum dataType = (instance.ModelMember as X10Attribute)?.DataType as DataTypeEnum;
              if (dataType == null) {
                generator.Messages.AddError(instance.XmlElement, "Expected data type to be an enum");
                return null;
              }

              string pairsConstant = TypeScriptCodeGenerator.EnumToPairsConstant(dataType);
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
      new PlatformClassDef() {
        LogicalName = "Icon",
        PlatformName = "Icon",
        ImportDir = "chakra",
        InheritsFromName = "TextualDisplay",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("icon", "iconName"),
          new JavaScriptAttributeDynamic("color", "color") {
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("white", "white"),
              new EnumConversion("silver", "grey40"),
              new EnumConversion("gray", "grey60"),
              new EnumConversion("black", "black"),
              new EnumConversion("red", "red40"),
              new EnumConversion("maroon", "orange60"),
              new EnumConversion("yellow", "orange20"),
              new EnumConversion("olive", "green60"),
              new EnumConversion("lime", "green40"),    // No actual lime color
              new EnumConversion("green", "green40"),
              new EnumConversion("aqua", "green30"),
              new EnumConversion("teal", "greey60"),
              new EnumConversion("blue", "indigo40"),
              new EnumConversion("navy", "blue60"),
              new EnumConversion("fuchsia", "purple40"),
              new EnumConversion("purple", "purple60"),

              // Non-Web colors
              new EnumConversion("orange", "orange30"),
            },
          },
        },
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextInput",
        ImportDir = "react_lib/chakra_wrappers",
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
          new JavaScriptAttributeDynamic("prefix", "prefix"),
          new JavaScriptAttributeDynamic("suffix", "suffix"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextareaInput",
        ImportDir = "react_lib/chakra_wrappers",

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
          // Must explicitly write "onChange={() => { } }" when read-only since TextareaInput requires it
          // When writeable, this is handled by JavaScriptAttributeDynamic
          // new JavaScriptAttributeByFunc() {
          //   PlatformName = "onChange",
          //   IsCodeSnippet = true,
          //   Function = (generator, instance) =>
          //     UiCompilerUtils.IsReadOnly(instance) ? "() => { }" : null,
          // },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntEdit",
        // TODO: Use something more specific
        PlatformName = "FloatInput",
        ImportDir = "react_lib/chakra_wrappers",

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
          new JavaScriptAttributeDynamic("prefix", "prefix"),
          new JavaScriptAttributeDynamic("suffix", "suffix"),
          new PlatformAttributeStatic("decimalPrecision", 0),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        PlatformName = "FloatInput",
        ImportDir = "react_lib/chakra_wrappers",

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
          new JavaScriptAttributeDynamic("prefix", "prefix"),
          new JavaScriptAttributeDynamic("suffix", "suffix"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "Checkbox",
        ImportDir = "react_lib/chakra_wrappers",

        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "checked",
            PlatformName = "checked",
          },
          new JavaScriptAttributeDynamic("checkboxLabel", "label"),
          new JavaScriptAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "disabled",
            AttributeUnnecessaryWhen = false,
          },
          new JavaScriptAttributeDynamic("onChange", "onChange"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateEditor",
        PlatformName = "CalendarDateInput",
        ImportDir = "react_lib/chakra_wrappers",
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
        LogicalName = "TimeEditor",
        PlatformName = "TimeInput",
        ImportDir = "react_lib/chakra_wrappers",
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
          new JavaScriptAttributeDynamic("excludeItems", "excludeItems"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "options",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              UiAttributeValueAtomic itemsSource = instance.FindAttributeValue("itemsSource") as UiAttributeValueAtomic;
              if (itemsSource == null) {
                DataTypeEnum dataType = (instance.ModelMember as X10Attribute)?.DataType as DataTypeEnum;
                if (dataType == null) {
                  generator.Messages.AddError(instance.XmlElement, "Expected data type to be an enum");
                  return null;
                }

                string pairsConstant = TypeScriptCodeGenerator.EnumToPairsConstant(dataType);
                generator.ImportsPlaceholder.Import(pairsConstant, dataType);

                return pairsConstant;
              } else {
                // See Issue #39
                return string.Format("{0}.{1}", generator.SourceVariableName, itemsSource.Value);
              }
            },
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DropDown",
        PlatformName = "SelectInput",
        InheritsFromName = "EnumSelection",
        ImportDir = "react_lib/chakra_wrappers",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
        }
      },
      new PlatformClassDef() {
        LogicalName = "RadioButtonGroup",
        PlatformName = "RadioGroup",
        InheritsFromName = "EnumSelection",
        ImportDir = "react_lib/chakra_wrappers",
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
        PlatformName = "VerticalStackPanel",
        ImportDir = "react_lib/layout",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("gap", "gap"),
          new JavaScriptAttributeDynamic("align", "align"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Row",
        PlatformName = "Flex",
        IsNonDefaultImport = true,

        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("align", "alignItems") {
            EnumConversions = new List<EnumConversion>() {
              new EnumConversion("top", "flex-start"),
              new EnumConversion("center", "center"),
              new EnumConversion("bottom", "flexEnd"),
            },
            DefaultValue = "center",
          },
          new JavaScriptAttributeDynamic("gap", "gap"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "RepellingRow",
        PlatformName = "Flex",
        IsNonDefaultImport = true,

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
              Instance headerInstance = instance.FindSingleComplexAttributeInstance("Header");
              if (headerInstance == null)
                return null;              

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

              generator.WriteLine(indent, "itemDisplayFunc={ (data, onChange, inListIndex) => (");

              generator.PushSourceVariableName("data");
              generator.GenerateComponentRecursively(OutputType.React, indent + 1, inner);
              generator.PopSourceVariableName();

              generator.WriteLine(indent, ") }");
            },
          },
          new JavaScriptAttributeDynamic("addItemLabel", "addItemLabel"),
          new JavaScriptAttributeDynamic("layout", "layout"),
          new JavaScriptAttributeByFunc() {
            PlatformName = "addNewItem",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              generator.ImportsPlaceholder.ImportCreateDefaultFunc(instance.DataModelEntity);
              return TypeScriptCodeGenerator.CreateDefaultFuncName(instance.DataModelEntity);
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
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "expandedContentFunc",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Instance expandedInstance = instance.FindSingleComplexAttributeInstance("ExpandedContent");
              if (expandedInstance == null)
                return null;              

              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                generator.WriteLine(indent, "expandedContentFunc={ (data) => (");

                generator.PushSourceVariableName("data");
                generator.GenerateComponentRecursively(OutputType.React, indent + 1, expandedInstance);
                generator.PopSourceVariableName();

                generator.WriteLine(indent, ") }");              
              });
            },
          },
        }
      },
      new PlatformClassDef() {
        LogicalName = "TableColumn",
        PlatformName = "",  // Just a JavaScript object
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "id",
            Function = (instance) => "_" + instance.Index,
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
                string path = generator.GetReadOnlyBindingPath(inner);
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
        ImportDir = "react_lib/chakra_wrappers",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeDynamic("url", "url"),
          new JavaScriptAttributeDynamic("style", "style"),
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
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeDynamic("url", "href"),
        },
      },
      #endregion

      #region Display Form
      new PlatformClassDef() {
        LogicalName = "DisplayForm",
        PlatformName = "DisplayForm",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("gap", "gap"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Label",
        PlatformName = "DisplayField",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("toolTip", "toolTip"),
          new JavaScriptAttributeDynamic("label", "label"),
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
            PlatformName = "context",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              Entity entity = instance.DataModelEntity;
              generator.ImportsPlaceholder.ImportCalculateErrorsFunc(entity);
              return string.Format("{{ errors: {0}({1} as {2}) }}",
                TypeScriptCodeGenerator.CalculateErrorsFuncName(entity),
                generator.SourceVariableName,
                entity.Name);
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
            Function = (generator, instance) => CodeGenUtils.GetBindingPathAsString(instance.Unwrap(), false),
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "inListIndex",
            Function = (generator, instance) => {
              bool isInList = UiUtils.ListSelfAndAncestors(instance)
                .Any(x => x.RenderAs.Name == BaseLibrary.CLASS_DEF_LIST);
              return isInList ? "inListIndex" : null;
            },
            IsCodeSnippet = true,
          },
          new JavaScriptAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "indicateRequired",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new JavaScriptAttributeDynamic("toolTip", "toolTip"),
          new JavaScriptAttributeDynamic("label", "label"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormErrorDisplay",
        PlatformName = "FormErrorDisplay",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("paths", "paths"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "SubmitButton",
        PlatformName = "FormSubmitButton",
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic("mutation", "mutation") {
            IsCodeSnippet = true,
          },
          new JavaScriptAttributeByFunc() {
            PlatformName = "variables",
            IsCodeSnippet = true,
            Function = (generator, instance) => {
              return new CodeSnippetGenerator((generator, indent, PlatformClassDef, instance) => {
                MemberWrapper dataInventory = UiComponentDataCalculator.ExtractData(generator.CurrentClassDef);

                generator.WriteLine(indent, "variables={");
                generator.WriteLine(indent + 1, "{");
                generator.WriteLine(indent + 2, "{0}: {", generator.SourceVariableName);
                generator.WriteLine(indent + 3, "id: {0}.id,", generator.SourceVariableName);

                // This is deliberately not recursive - we strike a compromise between
                // not returning reams of unnecessary data if only a few fields are being
                // edited, nor do we want to generate tricky recursive code for (possibly
                // lists of) nested elements
                foreach (MemberWrapper wrapper in dataInventory.Children) {
                  Member member = wrapper.Member;
                  // TODO: Remove - under typescript, conversion of enum types is not needed
                  // if (member is X10RegularAttribute regular && regular.IsEnum) {
                  //   generator.ImportsPlaceholder.ImportFunction(HelperFunctions.ToGraphqlEnum);
                  //   generator.WriteLine(indent + 3, "{0}: {1}({2}.{0}),", 
                  //     member.Name, 
                  //     HelperFunctions.ToGraphqlEnum.Name,
                  //     generator.SourceVariableName);
                  // } else
                    generator.WriteLine(indent + 3, "{0}: {1}.{0},", 
                      member.Name, 
                      generator.SourceVariableName);
                }

                generator.WriteLine(indent + 2, "}");
                generator.WriteLine(indent + 1, "}");
                generator.WriteLine(indent, "}");
              });
            },
          },
          new JavaScriptAttributeDynamic("label", "label"),
          new JavaScriptAttributeDynamic("successMessage", "successMessage"),
          new JavaScriptAttributeDynamic("errorMessage", "errorMessage"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "Action",
        PlatformName = "",  // Just a JavaScript object
        ImportDir = "react_lib/form",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("successUrl", "successUrl"),
        },
      },
      new PlatformClassDef() {
        LogicalName = "FormRow",
        PlatformName = "Flex",
        IsNonDefaultImport = true,

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
              Instance buttonInstance = instance.FindSingleComplexAttributeInstance("OpenButton");
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
              string varName = TypeScriptCodeGenerator.VariableName(refedEntity, true);
              generator.GqlPlaceholder.AddGqlQueryForAssociationEditor(refedEntity);
              return varName + "Query"; // Must match query name in GqlPlaceholder
            }
          },
          // TODO: How to deal with this - ideally, we'd like the top
          // level x10 definition to specify a derived attribute for this
          // new PlatformAttributeStatic() {
          //   PlatformName = "toString",
          //   IsCodeSnippet = true,
          //   Value = "x => x.toStringRepresentation",
          // },
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
          new JavaScriptAttributeDynamic("text", "text"),
        },
      },

      new PlatformClassDef() {
        LogicalName = "Embed",
        PlatformName = "Embed",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeDynamic("url", "url"),
        },
      },

      new PlatformClassDef() {
        LogicalName = "SpaContent",
        PlatformName = "SpaContent",
        ImportDir = "react_lib",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new JavaScriptAttributeByFunc() {
            PlatformName = "rootComponent",
            Function = (generator, instance) => {
              string rootComponent = instance.FindValue("rootComponent").ToString();
              return string.Format("<{0}Interface/>", rootComponent);
            },
            IsCodeSnippet = true,
          },
        },
        ProgrammaticallyGenerateChildren = (generatorGeneric, indent, PlatformClassDef, instance) => {
          TypeScriptCodeGenerator generator = (TypeScriptCodeGenerator)generatorGeneric;
          generator.ImportsPlaceholder.Import("Route", "react-router-dom", ImportLevel.ThirdParty);

          foreach (ClassDefX10 classDef in generator.AllUiDefinitions.All) {
            if (classDef.Url == null)
              continue;

            string compInterface = classDef.Name + "Interface";
            generator.ImportsPlaceholder.ImportDefault(classDef, "Interface");

            if (classDef.IsMany)
              generator.WriteLine(indent, "<Route path='{0}' element={ <{1}/> } />",
                classDef.Url,
                compInterface);
            else if (TypeScriptCodeGenerator.IsForm(classDef)) {
              generator.WriteLine(indent, "<Route path='{0}/edit/:id' element={ <{1}/> } />",
                classDef.Url,
                compInterface);

              generator.WriteLine(indent, "<Route path='{0}/new' element={ <{1}/> } />",
                classDef.Url,
                compInterface);
            } else
              generator.WriteLine(indent, "<Route path='{0}/:id' element={ <{1}/> } />",
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
          Name = "Chakra UI",
          DefaultImportPath = "@chakra-ui/react",
        };
      return _singleton;
    }
    #endregion
  }
}
