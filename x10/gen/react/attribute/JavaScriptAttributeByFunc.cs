using System;

using x10.ui.composition;
using x10.ui.platform;
using x10.gen.react.generate;

namespace x10.gen.react.attribute {
  // This type of attribute allowed you to define that value of the code-generated
  // attribute via code.
  public class JavaScriptAttributeByFunc : PlatformAttribute {
    // The value of the attribute is directly given by this function
    public Func<ReactCodeGenerator, Instance, object> Function { get; set; }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = IsCodeSnippet;
      return Function((ReactCodeGenerator)generator, instance);
    }
  }
}
