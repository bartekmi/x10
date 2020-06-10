using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public static class FormulaParser {
    public static ExpBase Parse(MessageBucket errors, IParseElement element, string formula, Entity contextEntity) {
      ExpBase expression = MicrosoftCsParser.Parse(errors, element, formula);
      ProcessDataErrors(errors, expression, contextEntity);

      return expression;
    }

    private static void ProcessDataErrors(MessageBucket errors, ExpBase expression, Entity contextEntity) {
      // TODO
    }
  }
}
