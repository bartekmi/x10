namespace x10.parsing {
  public class XmlAttribute : XmlBase{
    public string Key { get; private set; }
    public XmlScalar Value { get; private set; }

    public XmlAttribute(string key, XmlScalar value) {
      Key = key;
      Value = value;
      value.Parent = this;
    }

    public override string ToString() {
      return string.Format("{0}: {1}", Key, Value);
    }
  }
}