using System;
using System.Reflection;

using x10.parsing;
using x10.model.definition;

namespace x10.model.metadata {

  public enum AppliesTo {
    Entity = 1,
    Association = 2,
    Attribute = 4,
    DerivedAttribute = 8,
    EnumType = 16,
    EnumValue = 32,
  }

  internal static class AppliesToHelper {
    internal static AppliesTo GetForObject(IAcceptsModelAttributeValues element) {
      if (element is Entity) return AppliesTo.Entity;
      if (element is Association) return AppliesTo.Association;
      if (element is X10RegularAttribute) return AppliesTo.Attribute;
      if (element is X10DerivedAttribute) return AppliesTo.DerivedAttribute;
      if (element is DataType) return AppliesTo.EnumType;
      if (element is EnumValue) return AppliesTo.EnumValue;

      throw new Exception("Unexpected type: " + element.GetType());
    }
  }

  public class ModelAttributeDefinition {
    public string Name { get; set; }
    public string Description { get; set; }
    public AppliesTo AppliesTo { get; set; }
    public CompileMessageSeverity? ErrorSeverityIfMissing { get; set; }
    public object DefaultIfMissing { get; set; }
    public string MessageIfMissing { get; set; }
    public DataType DataType { get; set; }
    public string Setter { get; set; }
    public Action<MessageBucket, TreeScalar, IAcceptsModelAttributeValues, AppliesTo> ValidationFunction { get; set; }
    public Action<MessageBucket, AllEntities, AllEnums, IAcceptsModelAttributeValues, ModelAttributeValue> Pass2Action { get; set; }

    public bool AppliesToType(AppliesTo type) {
      return (AppliesTo & type) > 0;
    }

    public PropertyInfo GetPropertyInfo(Type type) {
      if (Setter == null) return null;
      return type.GetProperty(Setter, BindingFlags.Public | BindingFlags.Instance);
    }

    public override string ToString() {
      return "ModelAttributeDefinition: " + Name;
    }
  }
}