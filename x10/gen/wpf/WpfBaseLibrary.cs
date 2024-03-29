﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using x10.parsing;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.ui.platform;
using x10.ui.composition;
using x10.gen.wpf.codelet;
using x10.model.definition;
using x10.utils;

namespace x10.gen.wpf {
  internal class WpfBaseLibrary {
    #region Base Classes: Visual
    readonly static PlatformClassDef Visual = new PlatformClassDef() {
      LogicalName = "ClassDefVisual",
      IsAbstract = true,
      LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
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
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Text",
            Value = "•",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Label",
        PlatformName = "lib:EditElementWrapper",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            LogicalName = "mandatoryIndicator",
            PlatformName = "IsMandatory",
            TranslationFunc = (value) => value?.ToString() == "mandatory",
          },
          new WpfAttributeDynamic() {
            LogicalName = "toolTip",
            PlatformName = "MyToolTip",
          },
          new WpfAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "Label",
          },
          new PlatformAttributeByFunc() {
            PlatformName = "EditorFor",
            Function = (instance) => WpfGenUtils.GetBindingPath(instance.ChildInstances.Single()),
          },
        },
      },
      #endregion

      #region  Atomic Text Display Components
      new PlatformClassDef() {
        LogicalName = "Text",
        PlatformName = "TextBlock",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
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
        PlatformName = "Pill",
        InheritsFrom = Visual,
        // TODO... Create the actual pill component in lib
      },
      #endregion

      #region Edit Components
      new PlatformClassDef() {
        LogicalName = "TextEdit",
        PlatformName = "TextBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "Text",
          },
          new WpfAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "IsReadOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TextArea",
        PlatformName = "TextBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
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
          new WpfAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "IsReadOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "IntEdit",
        // TODO: Use something more specific
        PlatformName = "TextBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "Text",
          },
          new WpfAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "IsReadOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "FloatEdit",
        // TODO: Use something more specific
        PlatformName = "TextBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "Text",
          },
          new WpfAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "IsReadOnly",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Checkbox",
        PlatformName = "CheckBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "checked",
            PlatformName = "IsChecked",
          },
          new WpfAttributeDynamic() {
            LogicalName = "checkboxLabel",
            PlatformName = "Content",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "BooleanViaButtons",
        PlatformName = "lib:BooleanViaButtons",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "Selected",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "DateEditor",
        PlatformName = "DatePicker",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "SelectedDate",
          },
          new WpfAttributeDynamic() {
            LogicalName = "readOnly",
            PlatformName = "IsReadOnly",
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
      // Not clear where the code for this belongs, as this is already a large file
      new PlatformClassDef() {
        LogicalName = "DropDown",
        PlatformName = "ComboBox",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "SelectedValue",
          },
          new PlatformAttributeStatic() {
            PlatformName = "SelectedValuePath",
            Value = "Value",
          },
          new PlatformAttributeByFunc() {
            PlatformName = "ItemsSource",
            Function = (instance) => {
              string dataType = (instance.ModelMember as X10Attribute)?.DataType?.Name;
              return string.Format("{{ Binding {0} }}",
                NameUtils.Pluralize(dataType)
              );
            }
          },
        },
      },
      #endregion

      #region Layout Components
      #region Vertical
      new PlatformClassDef() {
        LogicalName = "VerticalStackPanel",
        PlatformName = "StackPanel",
        InheritsFrom = Visual,
      },
      #endregion

      #region Horizontal
      new PlatformClassDef() {
        LogicalName = "Row",
        PlatformName = "StackPanel",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Orientation",
            Value = "Horizontal",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "RepellingRow",
        PlatformName = "Grid",
        InheritsFrom = Visual,
        // TODO: Add generation code 
      },
      #endregion

      #region 2-Dimensional
      new PlatformClassDef() {
        LogicalName = "Grid",
        PlatformName = "Grid",
        InheritsFrom = Visual,
        // TODO: Add generation code 
      },
      #endregion
      #endregion

      #region Table
      new PlatformClassDef() {
        LogicalName = "Table",
        PlatformName = "DataGrid",
        InheritsFrom = Visual,
        PrimaryAttributeWrapperProperty = "Columns",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "AutoGenerateColumns",
            Value = "False",
          },
          new PlatformAttributeStatic() {
            PlatformName = "CanUserAddRows",
            Value = "False",
          },
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            PlatformName = "ItemsSource",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "TableColumn",
        PlatformName = "DataGridTemplateColumn",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "Header",
          },
        },
        NestedClassDef = new PlatformClassDef() {
          PlatformName = "DataGridTemplateColumn.CellTemplate",
          NestedClassDef = new PlatformClassDef() {
            PlatformName = "DataTemplate",
          }
        }
      },
      #endregion

      #region Button / Actions
      new PlatformClassDef() {
        LogicalName = "HelpIcon",
        PlatformName = "TextBlock",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Text",
            Value = "(?)",
          },
          new PlatformAttributeStatic() {
            PlatformName = "Padding",
            Value = "10,0,0,0",
          },
          new WpfAttributeDynamic() {
            LogicalName = "text",
            PlatformName = "ToolTip",
          },
        },
      },
      new PlatformClassDef() {
        LogicalName = "Button",
        PlatformName = "Button",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeStatic() {
            PlatformName = "Click",
            Value = "NavigateToUrlInTag",
          },
          new WpfAttributeDynamic() {
            LogicalName = "label",
            PlatformName = "Content",
          },
          new WpfAttributeDynamic() {
            LogicalName = "url",
            PlatformName = "Tag",
          },
        },
      },
      new PlatformClassDefWithCodelet() {
        LogicalName = "SubmitButton",
        InheritsFromName = "Button",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "Click",
            // TODO: Need a more robust way to get unique element name for code-gen purposes
            Function = (instance) => instance.FindValue("label") + "Click",
          },
        },
        Codelet = new CodeletRecipee() {
          Comment = "Submit Method(s)",
          GenerateInXamlCs = (generator, instance) => {
            generator.WriteLine(2, "private void {0}Click(object sender, RoutedEventArgs e) {", instance.FindValue("label"));
            generator.WriteLine(3, "ViewModel.SubmitData(() => AppStatics.Singleton.DataSource.CreateOrUpdate(ViewModel.Model),");
            generator.WriteLine(4, "\"Saved\");");
            generator.WriteLine(2, "}");
          }
        },
      },
      new PlatformClassDef() {
      LogicalName = "SelectableButton",
        PlatformName = "ToggleButton",
        InheritsFromName = "Button",
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "selected",
            PlatformName = "IsChecked",
          },
          new WpfAttributeDynamic() {
            LogicalName = "onSelect",
            PlatformName = "Checked",
          },
          new WpfAttributeDynamic() {
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
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
      LogicalName = "MenuItem",
        PlatformName = "MenuItem",
        InheritsFrom = Visual,
        LocalPlatformAttributes = new List<PlatformAttribute>() {
          new PlatformAttributeByFunc() {
            PlatformName = "Click",
            Function = (instance) => instance.HasAttributeValue("url") ? "NavigateToUrlInTag" : null
          },
          new WpfAttributeDynamic() {
            IsMainDatabindingAttribute = true,
            LogicalName = "label",
            PlatformName = "Header",
          },
          new WpfAttributeDynamic() {
            LogicalName = "url",
            PlatformName = "Tag",
          },
        },
      },
      #endregion

      #region Form
      new PlatformClassDef() {
      LogicalName = "Form",
        PlatformName = "lib:Form",
        InheritsFrom = Visual,
      },
      new PlatformClassDef() {
      LogicalName = "FormSection",
        PlatformName = "lib:FormSection",
        InheritsFrom = Visual,
      },
      #endregion
    };

    #region Glue it Together
    private static PlatformLibrary _singleton;
    public static PlatformLibrary Singleton(MessageBucket errors, UiLibrary logicalLibrary) {
      if (_singleton == null)
        _singleton = new PlatformLibrary(BaseLibrary.Singleton(), definitions) {
          DefaultImportPath = "xmlns:lib=\"clr-namespace:wpf_lib.lib;assembly=wpf_lib\"",
        };
      return _singleton;
    }
    #endregion
  }
}
