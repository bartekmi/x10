using System;
using System.Collections.Generic;
using System.Text;
using x10.model;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class FormulaParser {

    public const string CONTEXT_NAME = "__Context__";

    internal MessageBucket Errors;
    internal AllEntities AllEntities;

    public FormulaParser(MessageBucket errors, AllEntities allEntities) {
      Errors = errors;
      AllEntities = allEntities;
    }

    public ExpBase Parse(IParseElement element, string formula, ExpDataType rootType) {
      ExpBase expression = MicrosoftCsParser.Parse(this, element, formula);
      ExpDataType dataType = expression.DetermineType(rootType);
      expression.DataType = dataType;

      return expression;
    }
  }
}
