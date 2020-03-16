namespace x10.parsing {
  public class TreeScalar : TreeNode {
    public object Value {get; private set;}

    public TreeScalar(string value) {
      Value = value;
    }

    public override string ToString() {
      return Value.ToString();
    }
  }
}