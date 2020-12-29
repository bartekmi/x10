using System;

using x10.compiler;
using x10.formula;
using x10.model.definition;
using x10.ui.composition;
using x10.utils;
using x10.ui.platform;

namespace x10.gen.react {
  internal class CodeSnippetGenerator {
    private Action<ReactCodeGenerator, int, PlatformClassDef, Instance> _shippetGenerator;

    internal CodeSnippetGenerator(Action<ReactCodeGenerator, int, PlatformClassDef, Instance> snippetGenerator) {
      _shippetGenerator = snippetGenerator;
    }

    internal void Generate(ReactCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      _shippetGenerator(generator, level, platClassDef, instance);
    }
  }
}