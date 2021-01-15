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

    public ParserXml(MessageBucket messages, string rootDir) : base(messages, rootDir) {
      // Do nothing
    }

    public XmlElement ParseFromString(string xml) {
      using (TextReader reader = new StringReader(xml)) {
        
        XmlElement rootElement = ParsePrivate(reader, null);
        if (rootElement != null)
          rootElement.SetFileInfo(FileInfo.FromFilename(rootElement.Name + ".xml"));

        return rootElement;
      }
    }

    public IParseElement Parse(string filePath) {
      return Parse(FileInfo.FromFilename(filePath));
    }

    public override IParseElement Parse(FileInfo fileInfo) {
      using (StreamReader reader = new StreamReader(fileInfo.FilePath))
        return ParsePrivate(reader, fileInfo);
    }

    private XmlElement ParsePrivate(TextReader reader, FileInfo fileInfo) {
      try {
        using (XmlTextReader xmlReader = new XmlTextReader(reader))
          return ParsePrivateThrowsException(xmlReader);
      } catch (XmlException e) {
        AddError("Can't parse XML file. Error: " + ExceptionUtils.GetMessageRecursively(e),
          new TreeFileError(fileInfo) {
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
          case XmlNodeType.Text:
            XmlScalar scalar = new XmlScalar(current, reader.Value);
            SetLocation(scalar, reader);
            current.SetTextContent(scalar);

            reader.Read();

            if (reader.NodeType != XmlNodeType.EndElement)
              throw new XmlException("Closing tag expected after text content", null, reader.LineNumber, reader.LinePosition);

            current = (XmlElement)current.Parent;

            break;

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
      XmlScalar scalar = new XmlScalar(element, element.Name);
      SetLocation(scalar, reader);

      XmlAttribute nameAttribute = new XmlAttribute(ELEMENT_NAME, scalar);
      element.AddAttribute(nameAttribute);
      SetLocation(nameAttribute, reader);
    }

    private void ReadAttributes(XmlTextReader reader, XmlElement element) {
      for (int ii = 0; ii < reader.AttributeCount; ii++) {
        reader.MoveToAttribute(ii);

        XmlScalar scalar = new XmlScalar(element, reader.Value);
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