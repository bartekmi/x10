using System;
using System.Collections.Generic;
using System.Text;

using x10.formula;
using x10.model.metadata;

namespace x10.model.definition {
  public abstract class X10Attribute : Member {
    public string DataTypeName { get; set; }
    public DataType DataType { get; set; }

    public override ExpDataType GetExpressionDataType() {
      return new ExpDataType(DataType);
    }
  }
}
