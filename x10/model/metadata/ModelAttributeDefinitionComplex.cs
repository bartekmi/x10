using System;
using System.Reflection;

using x10.parsing;
using x10.model.definition;

namespace x10.model.metadata {

  public class ModelAttributeDefinitionComplex : ModelAttributeDefinition {
    public Func<MessageBucket, TreeElement, object> ParseFunction {get;set;}
    public Action<MessageBucket, TreeElement, IAcceptsModelAttributeValues, AppliesTo> ValidationFunction { get; set; }
  }
}