using System.Collections.Generic;
using x10.model;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class FormulaParser {

    public const string CONTEXT_NAME = "__Context__";

    internal readonly MessageBucket Errors;
    internal readonly AllEntities AllEntities;
    internal readonly AllEnums AllEnums;
    internal readonly AllFunctions AllFunctions;
    public Dictionary<string, X10DataType> OtherAvailableVariables;

    public FormulaParser(MessageBucket errors,
      AllEntities allEntities,
      AllEnums allEnums,
      AllFunctions allFunctions
      ) {

      Errors = errors;
      AllEntities = allEntities;
      AllEnums = allEnums;
      AllFunctions = allFunctions;
    }

    public ExpBase Parse(IParseElement element, string formula, X10DataType rootType) {
      if (ExpStringInterpolation.IsStringInterpolation(formula))
        return ExpStringInterpolation.Parse(this, element, formula, rootType);

      ExpBase expression = MicrosoftCsParser.Parse(this, element, formula);
      expression.DetermineType(rootType);

      return expression;
    }

    private void SetOwnerRecursive(ExpBase expression) {
      foreach (ExpBase child in expression.ChildExpressions()) {
        child.Owner = expression;
        SetOwnerRecursive(child);
      }
    }
  }
}
