using System;
using System.Collections.Generic;

namespace x10.parsing {
  public abstract class TreeNode : TreeElement {
    public TreeNode Parent { get; internal set; }
  }
}