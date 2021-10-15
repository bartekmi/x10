using System.Collections.Generic;

using x10.model.definition;
using x10.model.metadata;

namespace x10.formula {
  public class ExpIdentifier : ExpBase {
    public string Name { get; set; }
    public bool IsOtherVariable { get; private set; }

    public ExpIdentifier(FormulaParser parser) : base(parser) {
      // Do nothing
    }

    public override void Accept(IVisitor visitor) {
      visitor.VisitIdentifier(this);
    }

    public override ExpIdentifier FirstMemberOfPath() { return this; }

    public override X10DataType DetermineTypeRaw(X10DataType type) {
      if (type.IsError)
        return X10DataType.ERROR;

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

      // As of Aug 15, 2020, I am removing the code which walks down the path of Member.Owner
      // to allow formulas that reference properties of parent attributes
      // While this was useful in some cases (specifically, when a form sub-component was
      // targeted/narrowed by 'path' - and formulas in it could still access parent attributes)
      // it was fraught with problems, e.g.
      // 1. What if the parent data was not even loaded?
      // 2. What if the component was in a Table or other Many-to-one component and does not
      //    have access to parent data
      // If this functionality is ever needed, you can retrieve it from the Code Repo based on
      // the above date, but you should also fixed the aforementioned issues.
      X10DataType childType = ExpMemberAccess.GetMemberAccessDataTypeStatic(type, Name);
      if (childType != null)
        return childType;

      string otherVarsMessage = otherVars == null ? null : string.Format("not a State variable: [{0}] and ",
        string.Join(", ", otherVars.Keys));

      // TODO: This is an opportunity to use AddErrorDidYouMean(), but would require a bit of work to 
      // come up with full list of allowed values
      Parser.Errors.AddError(this, "Identifier '{0}' is {1}not a Member of type: {2}", 
        Name,                 // Index 0
        otherVarsMessage,     // Index 1
        type);                // Index 2

      return X10DataType.ERROR;
    }
  }
}
