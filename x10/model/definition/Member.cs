using x10.model.metadata;
using x10.ui.metadata;

namespace x10.model.definition {
  public abstract class Member : ModelComponent {
    public abstract X10DataType GetX10DataType();

    public bool IsMandatory { get; set; }
    public bool IsReadOnly { get; set; }
    public Entity Owner { get; internal set; }
    public string UiName { get; internal set; }
    public ClassDef Ui { get; internal set; }

    // If this member has an applicable-when property (See APPLICABLE_WHEN in BaseLibrary),
    // this property points to the auto-generated derived attribute which defines the condition
    public X10DerivedAttribute ApplicableWhen {get; internal set; }

    // Derived
    public bool IsNonOwnedAssociation => this is Association assoc && !assoc.Owns;

    public override string ToString() {
      return string.Format("{0}.{1}", Owner.Name, Name);
    }
  }
}
