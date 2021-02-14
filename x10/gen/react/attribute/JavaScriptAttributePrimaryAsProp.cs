using System;

using x10.ui.composition;
using x10.ui.platform;
using x10.gen.react;
using x10.gen.react.generate;

namespace x10.gen.react.attribute {
  public class JavaScriptAttributePrimaryAsProp : PlatformAttribute {

    public Action<ReactCodeGenerator, int, PlatformClassDef, Instance> CodeSnippet { get; set; }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = true;
      return new CodeSnippetGenerator(CodeSnippet);
    }
  }
}