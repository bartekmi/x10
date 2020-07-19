using System;
using System.Linq;
using System.Collections.Generic;

using x10.formula;
using x10.model.libraries;

namespace x10.model.definition {

  // For a Model Entity Definition, return a mapping for all non-derived members:
  // Member => List of derived members which depend on it
  public class DerivedAttributeDependencyMap {
    private Dictionary<Member, HashSet<X10DerivedAttribute>> _dictionary = new Dictionary<Member, HashSet<X10DerivedAttribute>>();

    public HashSet<X10DerivedAttribute> ChildDependencies(Member member) {
      return _dictionary[member];
    }

    public static DerivedAttributeDependencyMap BuildMap(Entity entity) {
      DerivedAttributeDependencyMap map = new DerivedAttributeDependencyMap();
      foreach (Member member in entity.Members.Where(x => x is X10Attribute || x is Association))
          map._dictionary.Add(member, new HashSet<X10DerivedAttribute>());
      
      foreach (X10DerivedAttribute derived in entity.DerivedAttributes) 
        BuildMapResursively(map, derived, derived);

      return map;
    }

    private static void BuildMapResursively(DerivedAttributeDependencyMap map, X10DerivedAttribute target, X10DerivedAttribute current) {
      foreach (ExpBase expression in FormulaUtils.ListAll(current.Expression)) {
        Member member = expression.DataType.Member;
        if (member is X10RegularAttribute || member is Association) {
          // Below is false for formulas which involve __Context__
          if (map._dictionary.TryGetValue(member, out HashSet<X10DerivedAttribute> deriveds))
            deriveds.Add(target);
        } else if (member is X10DerivedAttribute parentDerived)
          BuildMapResursively(map, target, parentDerived);
      }
    }
  }

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
