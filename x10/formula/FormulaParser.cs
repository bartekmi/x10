using System;
using System.Collections.Generic;
using System.Text;
using x10.model;
using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public class FormulaParser {

    public const string CONTEXT_NAME = "__Context__";

    internal readonly MessageBucket Errors;
    internal readonly AllEntities AllEntities;
    internal readonly AllEnums AllEnums;

    public FormulaParser(MessageBucket errors, AllEntities allEntities, AllEnums allEnums) {
      Errors = errors;
      AllEntities = allEntities;
      AllEnums = allEnums;
    }

    public ExpBase Parse(IParseElement element, string formula, ExpDataType rootType) {
      ExpBase expression = MicrosoftCsParser.Parse(this, element, formula);
      ExpDataType dataType = expression.DetermineType(rootType);
      expression.DataType = dataType;

      return expression;
    }
  }
}
