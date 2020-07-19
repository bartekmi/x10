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
    public static IEnumerable<ExpBase> ListAll(ExpBase instance) {
      List<ExpBase> exp = new List<ExpBase>();
      ListAllInstances(exp, instance);
      return exp;
    }

    private static void ListAllInstances(List<ExpBase> instances, ExpBase instance) {
      if (instance == null)
        return;

      instances.Add(instance);

      foreach (ExpBase child in instance.ChildExpressions())
        ListAllInstances(instances, child);
    }
  }
}
