namespace x10.parsing {
  public class XmlScalar : XmlBase {
    public object Value {get; private set;}

    public XmlScalar(string value) {
      Value = value;
    }

    public override string ToString() {
      return Value.ToString();
    }
  }
}