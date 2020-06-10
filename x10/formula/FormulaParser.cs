using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public static class FormulaParser {

    public const string CONTEXT_NAME = "__Context__";

    public static ExpBase Parse(MessageBucket errors, IParseElement element, string formula, Entity context, ExpDataType rootType) {
      ExpBase expression = MicrosoftCsParser.Parse(errors, element, formula);
      expression.DetermineType(errors, context, rootType);

      return expression;
    }
  }
}
