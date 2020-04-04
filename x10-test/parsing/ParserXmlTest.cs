using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace x10.parsing {
  public class ParserXmlTest {

    private readonly MessageBucket _messages = new MessageBucket();

    [Fact]
    public void ParseValid() {
      Parser parser  = new ParserXml(_messages);
      XmlElement root = (XmlElement)parser.Parse("../../../parsing/data/Good.xml");

      Assert.True(_messages.IsEmpty);

      // Level 1
      VerifyElement(root, "Level1", 2, 3, 1, 2);
      VerifyAttribute(root, "attr1", "value1", 1, 9);
      VerifyAttribute(root, "attr2", "value2", 1, 24);

      XmlElement sc1 = root.Children[0];
      VerifyElement(sc1, "SelfClosing", 1, 0, 2, 4);
      VerifyAttribute(sc1, "attr", "sc1", 2, 16);

      XmlElement sc2 = root.Children[1];
      VerifyElement(sc2, "SelfClosing", 1, 0, 3, 4);
      VerifyAttribute(sc2, "attr", "sc2", 3, 16);

      // Level 2
      XmlElement level2 = root.Children[2];
      VerifyElement(level2, "Level2", 0, 1, 4, 4);

      // Level 3
      XmlElement level3 = level2.Children[0];
      VerifyElement(level3, "Level3", 1, 0, 5, 6);
      VerifyAttribute(level3, "attr3", "3", 5, 13);
    }

    private void VerifyElement(XmlElement element, string name, int attrCount, int childCount, int line, int character) {
      Assert.Equal(name, element.Name);
      Assert.Equal(attrCount, element.Attributes.Count);
      Assert.Equal(childCount, element.Children.Count);
      Assert.Equal(line, element.Start.LineNumber);
      Assert.Equal(character, element.Start.CharacterPosition);
    }

    private void VerifyAttribute(XmlElement root, string key, string expectedValue, int keyLine, int keyChar) {

        XmlAttribute attribute = root.FindAttribute(key);
        Assert.NotNull(attribute);

        XmlScalar value = attribute.Value;
        Assert.Equal(expectedValue, value.ToString());

        Assert.Equal(keyLine, attribute.Start.LineNumber);
        Assert.Equal(keyChar, attribute.Start.CharacterPosition);
    }
  }
}