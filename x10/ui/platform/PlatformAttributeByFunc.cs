﻿using System;

using x10.gen;
using x10.ui.composition;
namespace x10.ui.platform {
  // This type of attribute allowed you to define that value of the code-generated
  // attribute via code.
  public class PlatformAttributeByFunc : PlatformAttribute {
    // The value of the attribute is directly given by this function
    public Func<Instance, object> Function { get; set; }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = IsCodeSnippet;
      return Function(instance);
    }
  }
}
