using x10.model.definition;
using x10.model.metadata;

namespace x10.formula {
  public class ExpIdentifier : ExpBase {
    public string Name { get; set; }

    public ExpIdentifier(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitIdentifier(this);
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      if (Name == FormulaParser.CONTEXT_NAME) {
        Entity context = Parser.AllEntities.FindContextEntityWithError(this);
        if (context == null)
          return X10DataType.ERROR;
        return new X10DataType(context, false);
      }

      if (rootType == null) {
        Parser.Errors.AddError(this, "In this context, there is no Entity to access");
        return X10DataType.ERROR;
      }

      return ExpMemberAccess.GetMemberAccessDataType(this, Parser.Errors, rootType, Name);
    }
  }
}
