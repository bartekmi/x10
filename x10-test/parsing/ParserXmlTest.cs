using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

namespace x10.parsing {
  public class ParserXmlTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();
    private readonly ParserXml _parser;

    public ParserXmlTest(ITestOutputHelper output) {
      _output = output;
      _parser = new ParserXml(_messages);
    }

    [Fact]
    public void ParseValid() {
      XmlElement root = (XmlElement)_parser.Parse("../../../parsing/data/Good.xml");

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

    [Fact]
    public void ParseMissingClosingTag() {
      RunTest(@"
<Level1>
  <Level2/>
", "Can't parse XML file. Error: Unexpected end of file has occurred. The following elements are not closed: Level1. Line 4, position 1.", 4, 1);
    }

    [Fact]
    public void ParseWrongClosingTag() {
      RunTest(@"
<Level1>
  <Level2/>
</BadTag>
", "Can't parse XML file. Error: The 'Level1' start tag on line 2 position 2 does not match the end tag of 'BadTag'. Line 4, position 3.", 4, 3);
    }

    [Fact]
    public void ParseMissingClosingAngleBracket() {
      RunTest(@"
<Level1>
  <Level2
</Level1>
", "Can't parse XML file. Error: Name cannot begin with the '<' character, hexadecimal value 0x3C. Line 4, position 1.", 4, 1);
    }

    [Fact]
    public void ParseCompleteGibberish() {
      RunTest(@"
This is some gibberish - definitely <not> XML!!!
", "Can't parse XML file. Error: Data at the root level is invalid. Line 2, position 1.", 2, 1);
    }

    [Fact]
    public void ParseAmpersand() {
      RunTest(@"
<Level1>
  <Level2 attr=""this & that""/>
</Level1>
", "Can't parse XML file. Error: An error occurred while parsing EntityName. Line 3, position 23.", 3, 23);
    }

    private void RunTest(string xml, string expectedError, int line, int character) {
      _parser.ParseFromString(xml);

      TestUtils.DumpMessages(_messages, _output);
      CompileMessage error = _messages.Messages.Single();

      Assert.Equal(expectedError, error.Message);
      Assert.Equal(line, error.TreeElement.Start.LineNumber);
      Assert.Equal(character, error.TreeElement.Start.CharacterPosition);
    }

    #region Utilities
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
    #endregion
  }
}