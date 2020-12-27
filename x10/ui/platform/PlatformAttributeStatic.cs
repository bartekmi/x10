using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.gen;
using x10.ui.composition;
using x10.utils;

namespace x10.ui.platform {
  // A static attribute always added to an element when code-generating
  public class PlatformAttributeStatic : PlatformAttribute {

    // Value of the attribute
    public object Value { get; set; }

    public PlatformAttributeStatic() {
      // Do nothing
    }

    // Convenience constructor for most common use
    public PlatformAttributeStatic(string platformName, object value, bool isCodeSnippet = false) {
      PlatformName = platformName;
      Value = value;
      IsCodeSnippet = isCodeSnippet;
    }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = IsCodeSnippet;
      return Value;
    }
  }
}
