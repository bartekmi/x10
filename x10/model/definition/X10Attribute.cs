using System;
using System.Collections.Generic;
using System.Text;

using x10.formula;
using x10.model.metadata;

namespace x10.model.definition {
  public abstract class X10Attribute : Member {
    public string DataTypeName { get; set; }
    public DataType DataType { get; set; }
    public bool IsId {get;set;}

    // Derived
    public bool IsEnum => DataType is DataTypeEnum;

    public override X10DataType GetX10DataType() {
      if (DataType == null)   // E.g. was not specified in the yaml
        return X10DataType.ERROR;
      return new X10DataType(DataType);
    }
  }
}
