using System;

using x10.ui.composition;
using x10.ui.platform;
using x10.gen.typescript;
using x10.gen.typescript.generate;

namespace x10.gen.typescript.attribute {
  public class JavaScriptAttributePrimaryAsProp : PlatformAttribute {

    public Action<TypeScriptCodeGenerator, int, PlatformClassDef, Instance> CodeSnippet { get; set; }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      isCodeSnippet = true;
      return new CodeSnippetGenerator(CodeSnippet);
    }
  }
}