using System;
using System.Linq;
using System.Collections.Generic;

namespace x10.parsing {
  public class XmlElement : XmlBase {
    public string Name { get; private set; }
    public List<XmlAttribute> Attributes { get; private set; }
    public List<XmlElement> Children { get; private set; }
    public XmlScalar TextContent { get; private set; }

    public XmlElement(string name) {
      Name = name;
      Attributes = new List<XmlAttribute>();
      Children = new List<XmlElement>();
    }

    public void SetTextContent(XmlScalar scalar) {
      TextContent = scalar;
      scalar.Parent = this;
    }

    public void AddAttribute(XmlAttribute attribute) {
      Attributes.Add(attribute);
      attribute.Parent = this;
    }

    public void AddChild(XmlElement child) {
      Children.Add(child);
      child.Parent = this;
    }

    public XmlScalar FindAttributeScalar(string key) {
      XmlAttribute attribute = FindAttribute(key);
      return attribute?.Value;
    }

    public object FindAttributeValue(string key) {
      XmlScalar scalar = FindAttributeScalar(key);
      return scalar?.Value;
    }

    public XmlAttribute FindAttribute(string key) {
      return Attributes.SingleOrDefault(x => x.Key == key);
    }

    public override string ToString() {
      return string.Format("<{0}>", Name);
    }

    internal XmlElement CloneFileLocation() {
      XmlElement clone = new XmlElement(Name);

      clone.Parent = Parent;
      clone.Start = Start;
      clone.End = End;

      return clone;
    }
  }
}