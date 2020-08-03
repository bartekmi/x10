using System.Linq;
using x10.formula;
using x10.model;
using x10.model.definition;
using x10.model.libraries;
using x10.model.metadata;
using x10.ui;
using x10.ui.composition;
using x10.ui.metadata;
using x10.utils;

namespace x10.compiler {
  // This is a final opportunity to modify the compilation results based on certain rules.
  // One example is that for the Member applicableWhen Attribute, we do two additional things in the model:
  // 1. Create a Derived Attribute with the name <Member-Name>ApplicableWhen
  // 2. For any instance that maps to such an attribute, we hook up a corresponding "visible" property
  public class PostCompileTransformations {
    internal static void PostCompile(AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions, AllFunctions allFunctions) {
      foreach (Entity entity in allEntities.All)
        foreach (Member member in entity.Members.ToList())
          PostCompile(member);

      foreach (ClassDefX10 classDef in allUiDefinitions.All)
        foreach (Instance instance in UiUtils.ListSelfAndDescendants(classDef.RootChild))
          PostCompile(instance);
    }

    private static void PostCompile(Member member) {
      // APPLICABLE_WHEN
      // Add Derived Attribute to Entity 
      ModelAttributeValue applicableWhen = member.FindAttribute(BaseLibrary.APPLICABLE_WHEN);
      if (applicableWhen != null)
        member.Owner.LocalMembers.Add(new X10DerivedAttribute() {
          Name = ApplicableWhenPropertyName(member),
          Expression = applicableWhen.Expression,
          DataType = DataTypes.Singleton.Boolean,
          Owner = member.Owner,
        });
    }

    private static void PostCompile(Instance instance) {
      // APPLICABLE_WHEN
      // Add visibility attribute referring to Entity Derived Attribute
      ModelAttributeValue applicableWhen = instance.ModelMember?.FindAttribute(BaseLibrary.APPLICABLE_WHEN);
      if (applicableWhen != null) 
        // Do not override "visible" attribute if defined
        if (!instance.HasAttributeValue(ClassDefNative.ATTR_VISIBLE)) {
          UiAttributeDefinitionAtomic attrDef = ClassDefNative.Visual.FindAtomicAttribute(ClassDefNative.ATTR_VISIBLE);
          instance.AttributeValues.Add(new UiAttributeValueAtomic(attrDef, instance, instance.XmlElement) {
            Expression = new ExpIdentifier(null) {
              Name = ApplicableWhenPropertyName(instance.ModelMember),
              DataType = new X10DataType(DataTypes.Singleton.Boolean),
            },
          });
        }
    }

    private static string ApplicableWhenPropertyName(Member member) {
      return "ApplicableWhenFor" + NameUtils.Capitalize(member.Name);
    }

  }
}
