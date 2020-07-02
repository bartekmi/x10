using x10.model.metadata;

namespace x10.formula {
  public class ExpUnknown : ExpBase {
    public string DiagnosticMessage { get; set; }

    public ExpUnknown(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitUnknown(this);
    }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      // If a message is present, report it
      if (DiagnosticMessage != null)
        Parser.Errors.AddError(this, DiagnosticMessage);
      return X10DataType.ERROR;
    }
  }
}
