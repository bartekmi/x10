using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using x10.model.definition;
using x10.parsing;
using x10.utils;

namespace x10.model.metadata {

  public enum AppliesTo {
    Entity = 1,
    Association = 2,
    Attribute = 4,
    DerivedAttribute = 8,
    EnumType = 16,
    EnumValue = 32,
    Function = 64,
    FunctionArgument = 128,

    Member = Association | Attribute | DerivedAttribute,
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

    // https://stackoverflow.com/questions/600293/how-to-check-if-a-number-is-a-power-of-2
    internal static bool IsSingle(AppliesTo x) {
      return (x & (x - 1)) == 0;
    }

    internal static IEnumerable<AppliesTo> GetAllSingleAppliesTo(AppliesTo bitwiseCombinedAppliesTo) {
      return EnumUtils.List<AppliesTo>()
        .Where(x => IsSingle(x))
        .Where(x => (x & bitwiseCombinedAppliesTo) > 0);
    }

    internal static Type GetTypeForAppliesTo(AppliesTo singleAppliedTo) {
      switch (singleAppliedTo) {
        case AppliesTo.Association:
          return typeof(Association);
        case AppliesTo.Attribute:
          return typeof(X10RegularAttribute);
        case AppliesTo.DerivedAttribute:
          return typeof(X10DerivedAttribute);
        case AppliesTo.Entity:
          return typeof(Entity);
        case AppliesTo.EnumType:
          return typeof(DataTypeEnum);
        case AppliesTo.EnumValue:
          return typeof(EnumValue);
        case AppliesTo.Function:
          return typeof(Function);
        case AppliesTo.FunctionArgument:
          return typeof(Argument);
        default:
          throw new Exception("Unexpected AppliesTo: " + singleAppliedTo);
      }
    }

    internal static IEnumerable<Type> GetTypesForAppliesTo(AppliesTo appliesTo) {
      return GetAllSingleAppliesTo(appliesTo).Select(x => GetTypeForAppliesTo(x));
    }

  }

  public abstract class ModelAttributeDefinition : IComparable<ModelAttributeDefinition> {
    public string Name { get; set; }
    public string Description { get; set; }
    public AppliesTo AppliesTo { get; set; }
    public CompileMessageSeverity? ErrorSeverityIfMissing { get; set; }
    public object DefaultIfMissing { get; set; }
    public string MessageIfMissing { get; set; }
    public string Setter { get; set; }
    public int AttributeProcessingOrder { get; set; }
    public bool MustBeFormula { get; set; }
    
    // If true, when accessing attribute on an Entity, search the entire inheritance
    // tree, starting with the initial object, then going up the parent chain
    public bool SearchInheritanceTree { get; set; }

    public Action<MessageBucket, AllEntities, AllEnums, IAcceptsModelAttributeValues, ModelAttributeValue> Pass2Action { get; set; }

    public bool AppliesToType(AppliesTo type) {
      return (AppliesTo & type) > 0;
    }

    public int CompareTo(ModelAttributeDefinition other) {
      return AttributeProcessingOrder.CompareTo(other.AttributeProcessingOrder);
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