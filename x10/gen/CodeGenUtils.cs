using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.compiler.ui;
using x10.ui.composition;
using x10.formula;
using x10.parsing;

namespace x10.gen {
  public static class CodeGenUtils {


    public static string GetBindingPathAsString(Instance instance) {
      return string.Join(".", UiCompilerUtils.GetBindingPath(instance).Select(x => x.Name));
    }

    public static ExpBase PathToExpression(IEnumerable<Member> path) {
      MessageBucket messages = new MessageBucket();
      FormulaParser parser = new FormulaParser(messages, null, new model.AllEnums(messages), null);
      string pathAsString = string.Join(".", path.Select(x => x.Name));
      return parser.Parse(null, pathAsString, new X10DataType(path.First().Owner, false));
    }
  }
}