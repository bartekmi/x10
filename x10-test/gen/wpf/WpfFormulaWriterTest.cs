using System;
using System.IO;
using System.Linq;
using x10.formula;
using x10.model;
using x10.model.metadata;
using x10.parsing;
using x10.ui.libraries;
using x10.ui.platform;

using Xunit;
using Xunit.Abstractions;

namespace x10.gen.wpf {
  public class WpfFormulaWriterTest {

    private readonly ITestOutputHelper _output;
    private MessageBucket _messages = new MessageBucket();
    private AllEntities _entities;
    private AllEnums _allEnums;

    public WpfFormulaWriterTest(ITestOutputHelper output) {
      _output = output;
      Initialize();
    }

    private void Initialize() {
      string yaml = @"
name: MyEntity
attributes:
  - name: myField
    dataType: MyEnum
enums:
  - name: MyEnum
    values: one, two, three
";

      _allEnums = new AllEnums(_messages);
      _entities = TestUtils.EntityCompile(_messages, _allEnums, yaml);
      TestUtils.StopIfErrors(_messages, _output);
    }

    [Fact]
    public void WriteExpressionWithShortEnum() {
      TestRun("myField == \"one\"", "MyField == MyEnum.One");
    }

    [Fact]
    public void WriteExpressionWithLongEnum() {
      TestRun("myField == MyEnum.one", "MyField == MyEnum.One");
    }

    private void TestRun(string formula, string expected) {
      FormulaParser parser = new FormulaParser(_messages, _entities, _allEnums, null);
      IParseElement element = new XmlElement("Dummy") { Start = new PositionMark() };
      ExpBase expression = parser.Parse(element, formula, new X10DataType(_entities.All.Single(), false));
      TestUtils.StopIfErrors(_messages, _output);

      // Convert to WPF
      using TextWriter writer = new StringWriter();
      WpfFormulaWriter visitor = new WpfFormulaWriter(writer, false);
      expression.Accept(visitor);
      string renderedExpression = writer.ToString();

      // Verify
      Assert.Equal(expected, renderedExpression);
    }
  }
}