﻿using System;
using System.Collections.Generic;
using System.Text;

using x10.model.metadata;

namespace x10.model.definition {
  public abstract class X10Attribute : Member {
    public DataType DataType { get; set; }
  }
}
