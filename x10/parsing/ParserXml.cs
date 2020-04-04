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

    public override IParseRoot Parse(string path) {
      using (StreamReader reader = new StreamReader(path))
      return ParsePrivate(reader, path);
    }

    private XmlElement ParsePrivate(TextReader reader, string path) { 
      try {
        using (XmlTextReader xmlReader = new XmlTextReader(reader))
          return ParsePrivateThrowsException(xmlReader);
      } catch (XmlException e) {
        AddError("Can't parse YAML file. Error: " + ExceptionUtils.GetMessageRecursively(e),
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
      XmlElement treeRoot = null;
      XmlElement current = null;

      while (reader.Read()) {
        switch (reader.NodeType) {
          case XmlNodeType.Element:
            XmlElement newNode = new XmlElement();
            if (current == null)
              treeRoot = newNode;
            else
              current.AddChild(newNode);
            current = newNode;
            break;
          case XmlNodeType.Attribute:
            break;
          default:
            break;
        }
      }


      return treeRoot;
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