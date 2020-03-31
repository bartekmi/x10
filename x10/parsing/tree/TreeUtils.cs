using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public static class TreeUtils {
    public static T GetOptional<T>(TreeHash parent, string key, MessageBucket messages) where T : TreeNode {
      TreeNode node = GetOptionalAttribute(parent, key);
      if (node == null)
        return null;
      return GetOfCorrectType<T>(node, messages);
    }

    public static T GetMandatory<T>(TreeHash parent, string key, MessageBucket messages) where T : TreeNode {
      TreeNode node = GetMandatoryAttribute(parent, key, messages);
      if (node == null)
        return null;
      return GetOfCorrectType<T>(node, messages);
    }

    private static T GetOfCorrectType<T>(TreeNode node, MessageBucket messageBucket) where T : TreeNode {
      if (node is T)
        return node as T;

      // There is a special case where if a label is given such as 'attributes:', and the contents
      // are empty (Scalar with no data), but a Sequence is desired, we can safely 
      // return an empty sequence, since that seems to be the intent of the user
      // - just a placeholder where one day attributes will be added
      if (typeof(T) == typeof(TreeSequence) && 
        node is TreeScalar scalar &&
        string.IsNullOrWhiteSpace(scalar.Value?.ToString())) {

        TreeSequence sequence = new TreeSequence();
        sequence.CopyPropertiesFrom(node);
        return sequence as T;
      }


      messageBucket.AddError(node, string.Format("Expected a {0} at this location, but found {1}",
        typeof(T).Name, node.GetType().Name));

      return null;
    }

    public static TreeNode GetOptionalAttribute(TreeHash parent, string key) {
      TreeAttribute attribute = parent.FindAttribute(key);
      return attribute == null ? null : attribute.Value;
    }

    public static TreeNode GetMandatoryAttribute(TreeHash parent, string key, MessageBucket messages) {
      TreeAttribute attribute = parent.FindAttribute(key);
      if (attribute == null) {
        messages.AddError(parent, string.Format("Mandatory attribute '{0}' is missing", key));
        return null;
      }
      return attribute == null ? null : attribute.Value;
    }
  }
}
