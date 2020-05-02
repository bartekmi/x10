using System;
using System.Collections.Generic;
using x10.parsing;

namespace x10.model.definition {
  public abstract class ModelComponent : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Validation> Validations { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    protected ModelComponent() {
      AttributeValues = new List<ModelAttributeValue>();
    }

    public override string ToString() {
      return string.Format("{0} {1}", GetType().Name, Name);
    }
  }
}
