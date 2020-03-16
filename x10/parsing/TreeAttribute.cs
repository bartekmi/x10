namespace x10.parsing {
  public class TreeAttribute : TreeElement{
    public string Key { get; private set; }
    public TreeNode Value { get; private set; }

    public TreeAttribute(string key, TreeNode value) {
      Key = key;
      Value = value;
    }
  }
}