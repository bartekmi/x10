using x10.model.metadata;
using x10.parsing;

namespace x10.model.definition {
  public class ModelAttributeValue {
    public ModelAttributeDefinition Definition { get; set; }
    public object Value { get; set; }

    public TreeElement TreeElement { get; set; }

    public ModelAttributeValue(TreeElement treeElement) {
      TreeElement = treeElement;
    }
  }
}