using System;
using System.Collections.Generic;
using System.Text;

namespace x10.model.definition {
  public interface IAcceptsModelAttributeValues {
    List<ModelAttributeValue> AttributeValues { get; }
  }
}
