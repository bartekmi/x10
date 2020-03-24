namespace x10.parsing {
  public class TreeAttribute : TreeElement{
    public string Key { get; private set; }
    public TreeNode Value { get; private set; }

    public TreeAttribute(string key, TreeNode value) {
      Key = key;
      Value = value;
      value.Parent = this;
    }

    public override string ToString() {
      return string.Format("{0}: {1}", Key, Value);
    }
  }
}