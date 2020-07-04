using System.Collections.Generic;

using x10.model.definition;
using x10.model.metadata;

namespace x10.formula {
  public class ExpIdentifier : ExpBase {
    public string Name { get; set; }
    public bool IsOtherVariable { get; private set; }

    // Derived
    public bool IsContext { get { return Name == FormulaParser.CONTEXT_NAME; } }

    public ExpIdentifier(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitIdentifier(this);
    }

    public override ExpIdentifier FirstMemberOfPath() { return this; }

    public override X10DataType DetermineTypeRaw(X10DataType rootType) {
      if (IsContext) {
        Entity context = Parser.AllEntities.FindContextEntityWithError(this);
        if (context == null)
          return X10DataType.ERROR;
        return new X10DataType(context, false);
      }

      // Check if the identifier is a state variable (i.e. OtherAvailableVariables)
      // TODO: Check for ambiguity between state variables and entity properties
      Dictionary<string, X10DataType> otherVars = Parser.OtherAvailableVariables;
      if (otherVars != null && otherVars.TryGetValue(Name, out X10DataType dataType)) {
        IsOtherVariable = true;
        return dataType;
      }

      // Walk down the chain of Members of rootType to try and find the identifier
      // as a property of one of the types in the chain
      List<X10DataType> types = new List<X10DataType>();
      while (rootType != null) {
        types.Add(rootType);

        if (rootType.Member != null)
          rootType = new X10DataType(rootType.Member.Owner, false);
        else
          rootType = null;  // Same as 'break'
      }

      foreach (X10DataType type in types) {
        X10DataType childType = ExpMemberAccess.GetMemberAccessDataTypeStatic(type, Name);
        if (childType != null)
          return childType;
      }

      string message = "Identifier '{0}' is neither a State variable: [{1}] nor a Member of any of " +
        "the following types: [{2}]";
      Parser.Errors.AddError(this, message,
        Name,
        string.Join(", ", otherVars.Keys),
        string.Join(", ", types));

      return X10DataType.ERROR;
    }
  }
}
