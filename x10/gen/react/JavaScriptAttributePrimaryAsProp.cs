using System;

using x10.ui.composition;
using x10.ui.platform;

namespace x10.gen.react {
  public class JavaScriptAttributePrimaryAsProp : PlatformAttribute {

    public Action<ReactCodeGenerator, int, PlatformClassDef, Instance> CodeSnippet { get; set; }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = true;
      return new CodeSnippetGenerator(CodeSnippet);
    }
  }
}