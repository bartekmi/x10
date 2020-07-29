﻿using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.platform {
  public class PlatformAttributeDynamic : PlatformAttribute {
    // Name of equivalent attribute in the logical "UiAttributeDefinition" world
    public string LogicalName { get; set; }

    // Optional translation function from the logical value to the actual
    // value to be used in code-generation
    public Func<object, object> TranslationFunc { get; set; }

    // If all is required is a simple enum-to-enum coversion, use this
    public IEnumerable<EnumConversion> EnumConversions { get; set; }

    // Optional converter function for the attribute. Note that converter
    // may need to be bi-directional for editable properties.
    // The interpretation of 'Converter' is platform-specific.
    public string Converter { get; set; }
  }
}
