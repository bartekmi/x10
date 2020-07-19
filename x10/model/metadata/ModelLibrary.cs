using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.model.metadata {
  public class ModelLibrary {

    public List<ModelAttributeDefinition> Attributes { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ModelLibrary(List<ModelAttributeDefinition> attributes) {
      Attributes = attributes;
    }

    internal ModelAttributeDefinition Find(string attrName) {
      return Attributes.Single(x => x.Name == attrName);
    }
  }
}
