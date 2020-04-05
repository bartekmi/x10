using System;
using System.IO;
using System.Linq;
using System.Xml;

using x10.utils;

namespace x10.parsing {
  public class ParserXml : Parser {

    public ParserXml(MessageBucket messages) : base(messages) {
      // Do nothing
    }

    public XmlElement ParseFromString(string xml, string fakeFileName = "FromString.xml") {
      using (TextReader reader = new StringReader(xml))
        return ParsePrivate(reader, null);
    }

    public override IParseElement Parse(string path) {
      using (StreamReader reader = new StreamReader(path))
        return ParsePrivate(reader, path);
    }

    private XmlElement ParsePrivate(TextReader reader, string path) {
      try {
        using (XmlTextReader xmlReader = new XmlTextReader(reader))
          return ParsePrivateThrowsException(xmlReader);
      } catch (XmlException e) {
        AddError("Can't parse XML file. Error: " + ExceptionUtils.GetMessageRecursively(e),
          new TreeFileError(path) {
            Start = new PositionMark() {
              LineNumber = e.LineNumber,
              CharacterPosition = e.LinePosition,
            },
          });
        return null;
      }
    }

    private XmlElement ParsePrivateThrowsException(XmlTextReader reader) {
      XmlElement xmlRoot = null;
      XmlElement current = null;

      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.Element:
            XmlElement newElement = new XmlElement(reader.Name);
            SetLocation(newElement, reader);

            if (current == null)
              xmlRoot = newElement;
            else
              current.AddChild(newElement);

            if (!reader.IsEmptyElement)
              current = newElement;

            ReadAttributes(reader, newElement);

            break;

          case XmlNodeType.EndElement:
            current = (XmlElement)current.Parent;
            break;
          default:
            break;
        }
      }

      return xmlRoot;
    }

    private void ReadAttributes(XmlTextReader reader, XmlElement element) {
      for (int ii = 0; ii < reader.AttributeCount; ii++) {
        reader.MoveToAttribute(ii);
        XmlAttribute newAttribute = new XmlAttribute(reader.Name, new XmlScalar(reader.Value));
        element.AddAttribute(newAttribute);
        SetLocation(newAttribute, reader);
      }
    }

    public override string GetFileExtensionWithDot() {
      return ".xml";
    }

    private void SetLocation(XmlBase treeNode, XmlTextReader reader) {
      treeNode.Start = new PositionMark() {
        LineNumber = reader.LineNumber,
        CharacterPosition = reader.LinePosition,
      };
      treeNode.End = null;    // TODO... Can we at least know the length of the string?
    }
  }
}