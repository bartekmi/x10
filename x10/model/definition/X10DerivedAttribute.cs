using x10.formula;
using x10.model.libraries;

namespace x10.model.definition {
  public class X10DerivedAttribute : X10Attribute {
    public ExpBase Expression {
      get {
        ModelAttributeValue formulaAttrValue = this.FindAttribute(BaseLibrary.FORMULA);
        return formulaAttrValue.Expression;
      }
    }

    public X10DerivedAttribute() {
      // Derived attributes are always read-only, and this cannot be changed
      // in the Entity definition files.
      AttributeValues.Add(new ModelAttributeValue(null) {
        Value = true,
        Definition = BaseLibrary.Singleton().Find(BaseLibrary.READ_ONLY),
      });

      IsReadOnly = true;
    }
  }
}
