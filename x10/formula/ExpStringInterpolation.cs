using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Text;

using x10.model.metadata;
using x10.parsing;

namespace x10.formula {
  public class ExpStringInterpolation : ExpBase {
    public class StringOrExpression {
      internal string String;
      internal ExpBase Expression;
    }

    public string Template { get; set; }
    public List<StringOrExpression> Chunks { get; set; }

    public ExpStringInterpolation(FormulaParser parser) : base(parser) { 
      Chunks = new List<StringOrExpression>();
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitStringInterpolation(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return Chunks.Where(x => x.Expression != null).Select(x => x.Expression);
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
      int end = 0;
      while((start = template.IndexOf(TAG_START, start)) != -1) {
        MaybeAddString(exp, start, end);

        start += TAG_START.Length;
        end = template.IndexOf(TAG_END, start);
        if (end == -1) {
          MicrosoftCsParser.AddError(parser, element, "Mismatches braces in string interpolation",
            new TextSpan(start, template.Length - start));
          return exp;
        }

        string formula = template.Substring(start, end - start);
        ExpBase expChild = parser.Parse(element, formula, rootType);
        exp.Chunks.Add(new StringOrExpression() {
          Expression = expChild,
        });

        start = end + TAG_END.Length;
        end = start;
      }

      // Add any remaining string
      MaybeAddString(exp, template.Length, end);

      return exp;
    }

    private static void MaybeAddString(ExpStringInterpolation exp, int nextStart, int prevEnd) {
      if (nextStart > prevEnd) {
        exp.Chunks.Add(new StringOrExpression() {
          String = exp.Template.Substring(prevEnd, nextStart - prevEnd),
        });
      }

    }
  }
}
