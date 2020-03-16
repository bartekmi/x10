namespace x10.model.metadata {

  public enum AppliesTo {
    Entity,
    Association,
    Attribute,
    EnumValue,
  }

  public class ModelAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }
    public AppliesTo AppliesTo { get; set; }
  }
}