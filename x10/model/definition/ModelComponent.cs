using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public abstract class ModelComponent : IAcceptsModelAttributeValues {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ModelAttributeValue> AttributeValues { get; private set; }
    public List<Validator> Validators { get; set; }

    protected ModelComponent() {
      AttributeValues = new List<ModelAttributeValue>();
    }
  }
}
