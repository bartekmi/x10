using System;
using System.IO;
using System.Linq;
using System.Xml;

using x10.utils;

namespace x10.parsing {
  public class ParserXml : Parser {

    // Reserved attribute name for the "Name" of the XML node - i.e. the thing
    // that goes in the <> brackets: <MyElement>
    public const string ELEMENT_NAME = "Name";

    public ParserXml(MessageBucket messages) : base(messages) {
      // Do nothing
    }

    public XmlElement ParseFromString(string xml, string fakeFileName = "FromString.xml") {
      using (TextReader reader = new StringReader(xml)) {
        
        XmlElement rootElement = ParsePrivate(reader, null);
        if (rootElement != null)
          rootElement.SetFileInfo(fakeFileName);

        return rootElement;
      }
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

            CreateFakeAttributeForElementName(reader, newElement);
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

    // The compiler has an easier time if we treat the name of the Element itself simply
    // as just another Attribute with the special name "Name"
    private void CreateFakeAttributeForElementName(XmlTextReader reader, XmlElement element) {
      XmlScalar scalar = new XmlScalar(element.Name);
      SetLocation(scalar, reader);

      XmlAttribute nameAttribute = new XmlAttribute(ELEMENT_NAME, scalar);
      element.AddAttribute(nameAttribute);
      SetLocation(nameAttribute, reader);
    }

    private void ReadAttributes(XmlTextReader reader, XmlElement element) {
      for (int ii = 0; ii < reader.AttributeCount; ii++) {
        reader.MoveToAttribute(ii);

        XmlScalar scalar = new XmlScalar(reader.Value);
        SetLocation(scalar, reader);

        XmlAttribute newAttribute = new XmlAttribute(reader.Name, scalar);
        element.AddAttribute(newAttribute);
        SetLocation(newAttribute, reader);
      }
    }

    public override string GetFileExtensionWithDot() {
      return ".xml";
    }

    private void SetLocation(XmlBase xmlBase, XmlTextReader reader) {
      xmlBase.Start = new PositionMark() {
        LineNumber = reader.LineNumber,
        CharacterPosition = reader.LinePosition,
      };

      xmlBase.End = null;    // TODO... Can we at least know the length of the string?
    }
  }
}