using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace x10.parsing {
  public class ParserYamlTest {

    private readonly MessageBucket _messages = new MessageBucket();

    [Fact]
    public void ParseValid() {
      Parser parser  =new ParserYaml(_messages);
      TreeNode file = (TreeNode)parser.Parse("../../../parsing/data/Person.yaml");

      Assert.True(_messages.IsEmpty);

      Assert.IsType<TreeHash>(file);
      TreeHash root = file as TreeHash;
      Assert.Equal(3, root.Attributes.Count);

      VerifyAttribute(root, "name", "Person", 1, 1, 1, 7);
      VerifyAttribute(root, "description", "A human", 2, 1, 2, 14);

      TreeHash attributes = root.FindHash("attributes");
      Assert.Equal(2, attributes.Attributes.Count);

      TreeHash name = attributes.FindHash("name");
      VerifyAttribute(name, "dataType", "String", 6, 5, 6, 15);
      VerifyAttribute(name, "mandatory", "true", 7, 5, 7, 16);

      TreeHash dateOfBirth = attributes.FindHash("dateOfBirth");
      VerifyAttribute(dateOfBirth, "dataType", "Date", 9, 5, 9, 15);
      VerifyAttribute(dateOfBirth, "mandatory", "true", 10, 5, 10, 16);
    }

    private void VerifyAttribute(TreeHash root, string key, string expectedValue, 
      int keyLine, int keyChar, int valueLine, int valueChar) {

        TreeAttribute attribute = root.FindAttribute(key);
        Assert.NotNull(attribute);

        TreeNode value = attribute.Value;
        Assert.Equal(expectedValue, value.ToString());

        Assert.Equal(keyLine, attribute.Start.LineNumber);
        Assert.Equal(keyChar, attribute.End.CharacterPosition);
        
        Assert.Equal(valueLine, value.Start.LineNumber);
        Assert.Equal(valueChar, value.End.CharacterPosition);
    }

    [Fact]
    public void ParseInvalid() {
      Parser parser = new ParserYaml(_messages);
      TreeNode file = (TreeNode)parser.Parse("../../../parsing/data/Broken.yaml");

      Assert.Null(file);
      Assert.Equal(1, _messages.Count);
      
      CompileMessage error = _messages.Messages.Single();
      Assert.Equal(CompileMessageSeverity.Error, error.Severity);
      Assert.Equal("Can't parse YAML file. Error: (Line: 4, Col: 1, Idx: 35) - (Line: 4, Col: 1, Idx: 35): While scanning a simple key, could not find expected ':'.", error.Message);

      Assert.Equal(4, error.TreeElement.Start.LineNumber);
      Assert.Equal(1, error.TreeElement.Start.CharacterPosition);
      Assert.Equal(4, error.TreeElement.End.LineNumber);
      Assert.Equal(1, error.TreeElement.End.CharacterPosition);
    }

    // Just leaving this here as an example of how to use [Theory]
    // [Theory]
    // [InlineData(3)]
    // [InlineData(5)]
    // [InlineData(6)]
    // public void MyFirstTheory(int value) {
    //   Assert.True(IsOdd(value));
    // }
  }
}