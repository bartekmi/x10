using System;
using System.Linq;
using System.Collections.Generic;

namespace x10.parsing {
  public class TreeHash : TreeNode {
    public List<TreeAttribute> Attributes { get; private set; }

    public TreeHash() {
      Attributes = new List<TreeAttribute>();
    }

    public void AddAttribute(TreeAttribute attribute) {
      Attributes.Add(attribute);
      attribute.Parent = this;
    }

    public TreeNode FindNode(string key) {
      TreeAttribute attribute = FindAttribute(key);
      return attribute?.Value;
    }

    public object FindValue(string key) {
      TreeScalar scalar = FindNode(key) as TreeScalar;
      return scalar?.Value;
    }

    public TreeSequence FindSequence(string key) {
      return FindNode(key) as TreeSequence;
    }

    public TreeHash FindHash(string key) {
      return FindNode(key) as TreeHash;
    }

    public TreeAttribute FindAttribute(string key) {
      return Attributes.SingleOrDefault(x => x.Key == key);
    }
  }
}