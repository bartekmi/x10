using x10.model.metadata;

namespace x10.model.definition {
  public class Association : Member {
    public string ReferencedEntityName { get; set; }
    public bool IsMany { get; set; }
    public bool Owns { get; set; }

    // Rehydrated
    public Entity ReferencedEntity { get; set; }

    public override X10DataType GetX10DataType() {
      return new X10DataType(ReferencedEntity, IsMany);
    }
  }
}
