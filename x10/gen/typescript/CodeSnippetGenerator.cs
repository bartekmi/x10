using System;

using x10.compiler;
using x10.formula;
using x10.model.definition;
using x10.ui.composition;
using x10.utils;
using x10.ui.platform;
using x10.gen.typescript.generate;

namespace x10.gen.typescript {
  internal class CodeSnippetGenerator {
    private Action<TypeScriptCodeGenerator, int, PlatformClassDef, Instance> _shippetGenerator;

    internal CodeSnippetGenerator(Action<TypeScriptCodeGenerator, int, PlatformClassDef, Instance> snippetGenerator) {
      _shippetGenerator = snippetGenerator;
    }

    internal void Generate(TypeScriptCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      _shippetGenerator(generator, level, platClassDef, instance);
    }
  }
}