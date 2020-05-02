using System;
using System.Reflection;

using x10.parsing;
using x10.model.definition;

namespace x10.model.metadata {

  public class ModelAttributeDefinitionAtomic : ModelAttributeDefinition {
    public DataType DataType { get; set; }
    public bool DataTypeMustBeSameAsAttribute { get; set; }
    public Action<MessageBucket, TreeScalar, IAcceptsModelAttributeValues, AppliesTo> ValidationFunction { get; set; }
  }
}