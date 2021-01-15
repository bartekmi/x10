namespace x10.parsing {
  public class XmlScalar : XmlBase {
    public object Value {get; private set;}

    public XmlScalar(XmlElement parent, string value) {
      Parent = parent;
      Value = value;
    }

    // Do not change this definition. Other code depends on it.
    public override string ToString() {
      return Value.ToString();
    }
  }
}