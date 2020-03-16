using System;
using System.Collections.Generic;

namespace x10.parsing {
  public class TreeSequence : TreeNode {
    public List<TreeNode> Children { get; private set; }

    public TreeSequence() {
      Children = new List<TreeNode>();
    }

    public void AddChild(TreeNode child) {
      Children.Add(child);
      child.Parent = this;
    }
  }
}