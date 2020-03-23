using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public static class TreeUtils {
    public static T GetOptional<T>(TreeHash parent, string key, MessageBucket messageBucket) where T : TreeNode {
      TreeNode node = GetOptionalNode(parent, key);
      if (node == null)
        return null;

      if (node is T)
        return node as T;

      messageBucket.AddError(node, string.Format("Expected a {0} at this location, but found {1}", 
        typeof(T).Name, node.GetType().Name));

      return null;
    }

    public static TreeNode GetOptionalNode(TreeHash parent, string key) {
      TreeAttribute attribute = parent.FindAttribute(key);
      return attribute == null ? null : attribute.Value;
    }
  }
}
