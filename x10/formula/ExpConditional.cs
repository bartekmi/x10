using System.Collections.Generic;
using x10.model.metadata;

namespace x10.formula {
  public class ExpConditional : ExpBase {
    public ExpBase Conditional { get; set; }
    public ExpBase WhenTrue { get; set; }
    public ExpBase WhenFalse { get; set; }

    public ExpConditional(FormulaParser parser) : base(parser) { 
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitConditional(this);
    }

    public override IEnumerable<ExpBase> ChildExpressions() {
      return new ExpBase[] { Conditional, WhenTrue, WhenFalse };
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      X10DataType conditional = Conditional.DetermineType(rootType);
      if (!conditional.IsBoolean) {
          Parser.Errors.AddError(this, "The portion before the ? must evaluate to a boolean");
          return X10DataType.ERROR;
      }

      X10DataType whenTrue = WhenTrue.DetermineType(rootType);
      X10DataType whenFalse = WhenFalse.DetermineType(rootType);

      if (whenTrue.IsNull && whenFalse.IsNull) {
        Parser.Errors.AddError(this, "Both sides of the conditional (ternary) expression can't be null");
        return X10DataType.ERROR;
      }

      if (whenTrue.IsNull)
        return whenFalse;

      if (whenFalse.IsNull)
        return whenTrue;

      if (!whenTrue.Equals(whenFalse)) {
        Parser.Errors.AddError(this, "Type on the left is {0}, but type on the right is {1}",
          whenTrue, whenFalse);
        return X10DataType.ERROR;
      }

      return whenTrue;
    }
  }
}
