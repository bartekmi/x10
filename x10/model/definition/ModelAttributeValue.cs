using System;
using System.Diagnostics.CodeAnalysis;
using x10.formula;
using x10.model.metadata;
using x10.parsing;

namespace x10.model.definition {
  public class ModelAttributeValue : IComparable<ModelAttributeValue> {
    public ModelAttributeDefinition Definition { get; set; }
    public object Value { get; set; }
    public string Formula { get; set; }
    public ExpBase Expression { get; set; }

    public TreeElement TreeElement { get; set; }

    public ModelAttributeValue(TreeElement treeElement) {
      TreeElement = treeElement;
    }

    public int CompareTo(ModelAttributeValue other) {
      return Definition.CompareTo(other.Definition);
    }

    public override string ToString() {
      return string.Format("Attribute Value: {0}", Value);
    }

  }
}