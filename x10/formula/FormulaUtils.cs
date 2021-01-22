using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;

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

    public static HashSet<X10Attribute> ExtractSourceAttributes(ExpBase root) {
      HashSet<X10Attribute> attributes = new HashSet<X10Attribute>();

      IEnumerable<ExpBase> subExpressions = FormulaUtils.ListAll(root);
      foreach (ExpBase expression in subExpressions) 
        if (expression.DataType.Member is X10Attribute attr)
          attributes.Add(attr);

      return attributes;
    }

    public static IEnumerable<IEnumerable<Member>> ExtractMemberPaths(ExpBase root) {
      IEnumerable<ExpBase> subExpressions = FormulaUtils.ListAll(root);
      List<IEnumerable<Member>> paths = new List<IEnumerable<Member>>();

      foreach (ExpBase expression in subExpressions) 
        if (expression.DataType.Member is X10Attribute attr)
          paths.Add(ExtractMemberPath(expression));

      return paths;
    }

    private static IEnumerable<Member> ExtractMemberPath(ExpBase expression) {
      List<Member> members = new List<Member>();
      members.Add(expression.DataType.Member);

      while (expression is ExpMemberAccess memberAccess) {
        expression = memberAccess.Expression;
        if (expression.DataType.Member != null)
          members.Insert(0, expression.DataType.Member);
      }

      return members;
    }

    public static HashSet<X10RegularAttribute> ExtractSourceRegularAttributes(ExpBase root) {
      HashSet<X10RegularAttribute> regAttrs = new HashSet<X10RegularAttribute>();

      HashSet<X10Attribute> sourceAttrs = ExtractSourceAttributes(root);

      foreach (X10Attribute attribute in sourceAttrs)
        if (attribute is X10RegularAttribute regular)
          regAttrs.Add(regular);
        else if (attribute is X10DerivedAttribute derived) 
          regAttrs.UnionWith(derived.ExtractSourceAttributes());
        else
          throw new NotImplementedException();

      return regAttrs;
    }
  }
}
