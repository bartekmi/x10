using System;
using System.Collections.Generic;
using System.Text;
using x10.parsing;

namespace x10.model.definition {
  public abstract class ModelComponent : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Validator> Validators { get; set; }

    // IAcceptsModelAttributeValues
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public TreeElement TreeElement { get; set; }

    protected ModelComponent() {
      AttributeValues = new List<ModelAttributeValue>();
    }
  }
}
