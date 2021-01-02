using System;
using System.Collections.Generic;
using System.Text;

namespace x10.formula {
  public static class FormulaUtils {
    public static bool IsFormula(string valueOrFormula, out string strippedFormula) {
      string trimmed = valueOrFormula.Trim();

      if (trimmed.StartsWith("=")) {
        // TODO: We need to standardize on which types of quotes to use throught 
        // for formulas both within entities YAML and UI XML.
        trimmed = trimmed.Replace('\'', '"');
        strippedFormula = trimmed.Substring(1);
        return true;
      }

      strippedFormula = null;
      return false;
    }

    /// <summary>
    /// Recursively list all ExpBase including this one and all descendents
    /// </summary>
    public static IEnumerable<ExpBase> ListAll(ExpBase expression) {
      List<ExpBase> expressions = new List<ExpBase>();
      ListAllInstances(expressions, expression);
      return expressions;
    }

    private static void ListAllInstances(List<ExpBase> expressions, ExpBase expression) {
      if (expression == null)
        return;

      expressions.Add(expression);

      foreach (ExpBase child in expression.ChildExpressions())
        ListAllInstances(expressions, child);
    }
  }
}
