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

    // For some cases, notably when the Definition is of "same type as attribute" - indicate the ACTUAL data type here
    public DataType ActualDataType { get; set; }

    public TreeElement TreeElement { get; set; }

    // Derived
    public bool IsEnumValue { get { return EnumType is DataTypeEnum; } }
    public DataTypeEnum EnumType { 
      get {
        if (ActualDataType != null)
          return ActualDataType as DataTypeEnum;
        return Definition is ModelAttributeDefinitionAtomic atomic ? atomic.DataType as DataTypeEnum : null; 
      } 
    }

    public ModelAttributeValue(TreeElement treeElement) {
      TreeElement = treeElement;
    }

    public int CompareTo(ModelAttributeValue other) {
      return Definition.CompareTo(other.Definition);
    }

    public override string ToString() {
      return string.Format("{0}: {1}", Definition.Name, Value);
    }

  }
}