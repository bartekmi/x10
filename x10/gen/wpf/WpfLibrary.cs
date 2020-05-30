using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using x10.parsing;
using x10.ui.composition;

namespace x10.gen.wpf {
  // The WPF Implementation of the x10 Base Library
  public class WpfLibrary {
    // Mappings: 
    private readonly static List<UiComponentImplementation> definitions = new List<UiComponentImplementation>() {
      new UiComponentImplementation() {
        X10ComponentName = "Heading1",
        NativeComponentName = "TextBlock",
        ConstantAttributes = "Style=\"{StaticResource Heading1}\"",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping("text", "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "Heading2",
        NativeComponentName = "TextBlock",
        ConstantAttributes = "Style=\"{StaticResource Heading2}\"",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping("text", "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "Heading3",
        NativeComponentName = "TextBlock",
        ConstantAttributes = "Style=\"{StaticResource Heading3}\"",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping("text", "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "HorizontalDivider",
        NativeComponentName = "Separator",
      },
      new UiComponentImplementation() {
        X10ComponentName = "Row",
        NativeComponentName = "StackPanel",
      },
      new UiComponentImplementation() {
        X10ComponentName = "VerticalStackPanel",
        NativeComponentName = "StackPanel",
        ConstantAttributes = "Orientation=\"Horizontal\"",
      },
      new UiComponentImplementation() {
        X10ComponentName = "TextEdit",
        NativeComponentName = "TextBox",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "FloatEdit",
        NativeComponentName = "TextBox",  // TODO
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "IntEdit",
        NativeComponentName = "TextBox",  // TODO
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "Text"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "Checkbox",
        NativeComponentName = "CheckBox",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping("label", "Content"),
          new AttributeMapping("checked", "IsChecked"),
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "IsChecked"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "DateEditor",
        NativeComponentName = "DatePicker",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "SelectedDate"),
        },
      },
      new UiComponentImplementation() {
        X10ComponentName = "DropDown",
        NativeComponentName = "ComboBox",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping(AttributeMapping.PRIMARY_BINDING, "SelectedItem"),
        },
      },

      new UiComponentImplementation() {
        X10ComponentName = "SelectableButton",
        NativeComponentName = "ToggleButton",
        AttributeMappings = new List<AttributeMapping>() {
          new AttributeMapping("label", "Content"),
        },
      },

      new UiComponentImplementation() {
        X10ComponentName = "Form",
        NativeComponentName = "Form",
        LibraryName = "lib",
      },
      new UiComponentImplementation() {
        X10ComponentName = "FormSection",
        NativeComponentName = "FormSection",
        LibraryName = "lib",
      },
      new UiComponentImplementation() {
        X10ComponentName = "Label",
        NativeComponentName = "Label",
        LibraryName = "lib",
      },
      new UiComponentImplementation() {
        X10ComponentName = "Label",
        NativeComponentName = "CardSelector",
        LibraryName = "lib",
      },
    };

    #region Glue it Together
    private static NativeComponentLibary _singleton;
    public static NativeComponentLibary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private static NativeComponentLibary CreateLibrary() {
      NativeComponentLibary library = new NativeComponentLibary(definitions) {
        Name = "Base Wpf Library",
      };

      return library;
    }
    #endregion

    //private static string GetAttr(MessageBucket errors, Instance instance, string attrName) {
    //  string value = instance.FindValue<string>(attrName);
    //  if (value == null) {
    //    errors.AddError(instance.XmlElement, "No data for attribute '{0}'", attrName);
    //    return "NO DATA";
    //  }
    //  return value;
    //}
  }

  public class NativeComponentLibary {
    public string Name { get; set; }
    public List<UiComponentImplementation> Definitions { get; private set; }

    public NativeComponentLibary(List<UiComponentImplementation> definitions) {
      Definitions = definitions;
    }
  }

  public class UiComponentImplementation {
    public string X10ComponentName { get; set; }
    public string NativeComponentName { get; set; }
    public string LibraryName { get; set; }
    public string ConstantAttributes { get; set; }
    public List<AttributeMapping> AttributeMappings { get; set; }
  }

  public class AttributeMapping {
    public const string PRIMARY_BINDING = "PRIMARY_BINDING";

    public string X10_AttributeName { get; set; }
    public string NativeAttributeName { get; set; }

    // Derived
    public bool IsPrimaryBinding {
      get { return X10_AttributeName == PRIMARY_BINDING; }
    }

    public AttributeMapping(string x10_Name, string nativeName) {
      X10_AttributeName = x10_Name;
      NativeAttributeName = nativeName;
    }
  }
}
