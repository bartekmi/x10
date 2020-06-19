using System;
using System.Reflection;

using x10.parsing;
using x10.model.definition;

namespace x10.model.metadata {

  public class ModelAttributeDefinitionAtomic : ModelAttributeDefinition {
    public DataType DataType { get; set; }
    public Action<MessageBucket, TreeScalar, IAcceptsModelAttributeValues, AppliesTo> ValidationFunction { get; set; }

    private bool _dataTypeMustBeSameAsAttribute;
    public bool DataTypeMustBeSameAsAttribute { 
      get { return _dataTypeMustBeSameAsAttribute; }
      set {
        _dataTypeMustBeSameAsAttribute = value;
        // Any attribute which depends on the data-type attribute
        // must be process after the data type attribute
        // This ensure proper processing order
        AttributeProcessingOrder = value ? 10 : 0;
      }
    }
  }
}