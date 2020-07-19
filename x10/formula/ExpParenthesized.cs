using System.Collections.Generic;
using x10.model.metadata;

namespace x10.formula {
  public class ExpParenthesized : ExpBase {
    public ExpBase Expression { get; set; }

    public ExpParenthesized(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitParenthesized(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return new ExpBase[] { Expression };
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      return Expression.DetermineType(rootType);
    }
  }
}
