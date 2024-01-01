using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Text;

using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpStringInterpolation : ExpBase {
    public string Template { get; set; }
    public List<ExpBase> Expressions { get; set; }

    public ExpStringInterpolation(FormulaParser parser) : base(parser) { 
      Expressions = new List<ExpBase>();
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitStringInterpolation(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return Expressions;
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      return X10DataType.String;
    }

    internal static bool IsStringInterpolation(string formula) {
      formula = formula.Trim();
      return formula.StartsWith('`') && formula.EndsWith('`');
    }

    internal static ExpStringInterpolation Parse(
      FormulaParser parser, 
      IParseElement element, 
      string template,
      X10DataType rootType) {

      if (!IsStringInterpolation(template))
        throw new Exception("Not string interpolation: " + template);

      template = template.Trim('`');

      const string TAG_START = "${";
      const string TAG_END = "}";

      ExpStringInterpolation exp = new ExpStringInterpolation(parser) {
        Template = template,
      };
      exp.DetermineType(rootType);

      int start = 0;
      while((start = template.IndexOf(TAG_START, start)) != -1) {
        start += TAG_START.Length;
        int end = template.IndexOf(TAG_END, start);
        if (end == -1) {
          MicrosoftCsParser.AddError(parser, element, "Mismatches braces in string interpolation",
            new TextSpan(start, template.Length - start));
          break;
        }

        string formula = template.Substring(start, end - start);
        ExpBase expChild = parser.Parse(element, formula, rootType);
        exp.Expressions.Add(expChild);

        start = end + TAG_END.Length;
      }

      return exp;
    }
  }
}
