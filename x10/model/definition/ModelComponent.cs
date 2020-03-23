using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public class ModelComponent {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ModelAttributeValue> AttributeValues { get; set; }
    public List<Validator> Validators { get; set; }
  }
}
