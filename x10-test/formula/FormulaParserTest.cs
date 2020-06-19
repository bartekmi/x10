using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model;
using x10.model.metadata;
using x10.compiler;

namespace x10.formula {
  public class FormulaParserTest {

    private readonly ITestOutputHelper _output;
    private readonly AllEnums _enums;
    private readonly AllFunctions _functions;
    private readonly MessageBucket _errors = new MessageBucket();
    private AllEntities _allEntities;
    private Entity _entity;

    public FormulaParserTest(ITestOutputHelper output) {
      _output = output;
      _enums = new AllEnums(_errors);
      _functions = new AllFunctions(_errors);

      InitializeEntities();
      InitializeFunctions();
    }

    private void InitializeEntities() {
      string entityYaml = @"
name: Entity
attributes:
  - name: a
    dataType: Integer
  - name: b
    dataType: Float
  - name: c
    dataType: Integer
  - name: d
    dataType: Integer
  - name: e
    dataType: Integer
  - name: myBoolean
    dataType: Boolean
  - name: myString
    dataType: String
  - name: myEnumValue
    dataType: MyEnum
associations:
  - name: nested
    dataType: Nested
enums:
  - name: MyEnum
    values: one, two, three
";
      string nestedYaml = @"
name: Nested
attributes:
  - name: attr
    dataType: Date
";
      _allEntities = TestUtils.EntityCompile(_errors, _enums, entityYaml, nestedYaml);
      _entity = _allEntities.FindEntityByName("Entity");
    }

    private void InitializeFunctions() {
      string functionYaml = @"
name: MyFunc
description: Take a string and an int
returnDataType: String
arguments:
  - name: aString
    dataType: String
  - name: anInt
    dataType: Integer
";
      FunctionsCompiler funcCompiler = new FunctionsCompiler(_errors, _allEntities, _enums, _functions, new AttributeReader(_errors));
      funcCompiler.CompileFunctionFromString(functionYaml);
      Assert.Single(_functions.All);
    }

    [Fact]
    public void ParseSuccessful() {
      TestExpectedSuccess("1 + 2.7", DataTypes.Singleton.Float);                    // Binary
      TestExpectedSuccess("\"Hello\" + \" World\"", DataTypes.Singleton.String);    // Binary of Strings
      TestExpectedSuccess("(1 + a) / b", DataTypes.Singleton.Float);                // Parenthesis
      TestExpectedSuccess("a + c", DataTypes.Singleton.Integer);                    // Parenthesis
      TestExpectedSuccess("MyFunc(myString, a)", DataTypes.Singleton.String);       // Function
      TestExpectedSuccess("a * b + c * d / e", DataTypes.Singleton.Float);          // Compound expression
      TestExpectedSuccess("a < 7", DataTypes.Singleton.Boolean);                    // Inequalifty
      TestExpectedSuccess("myBoolean && (a < 7)", DataTypes.Singleton.Boolean);     // Logical expression
      TestExpectedSuccess("nested.attr", DataTypes.Singleton.Date);                 // Member access
      TestExpectedSuccess("myString + \" \" + a", DataTypes.Singleton.String);      // String-add
    }

    [Fact]
    public void ParseEnum() {
      TestExpectedSuccess("myEnumValue == MyEnum.three", DataTypes.Singleton.Boolean);     // String-add
    }

    [Fact]
    public void ParseWithSyntaxErrors() {
      TestExpectedError("a + b)", "Unexpected token ')'", 0, 5);
    }

    [Fact]
    public void UnknownEnumValue() {
      TestExpectedError("myEnumValue == MyEnum.none", "Enum 'MyEnum' does not have value 'none'", 22, 26);
    }

    [Fact]
    public void FunctionUnknownName() {
      TestExpectedError("SomeBogusFunction(7)", "Function 'SomeBogusFunction' is not defined", 0, 20);
    }

    [Fact]
    public void FunctionWrongNumberOfArgs() {
      TestExpectedError("MyFunc(7, 8, 9)", "Function 'MyFunc' expects 2 argument(s) but was given 3", 0, 15);
    }

    [Fact]
    public void FunctionArgTypeMismatch() {
      TestExpectedError("MyFunc(\"Hello\", false)", "For argument at position 2, function 'MyFunc' expects data type Integer, but was given Boolean", 16, 21);
    }

    [Fact]
    public void DataTypeMismatch() {
      TestExpectedError("a - myBoolean", "Cannot subtract Integer and Boolean", 2, 3);
      TestExpectedError("a % b", "Cannot take remainder of Integer and Float", 2, 3);
      TestExpectedError("a + myBoolean", "Cannot add Integer and Boolean", 2, 3);
      TestExpectedError("a && myBoolean", "Cannot logical-and Integer and Boolean", 2, 4);
      TestExpectedError("a > myBoolean", "Cannot compare Integer and Boolean", 2, 3);
      TestExpectedError("a == myBoolean", "Cannot test equality of Integer and Boolean", 2, 4);
    }

    [Fact]
    public void MissingAttribute() {
      TestExpectedError("a - missing", "Entity 'Entity' does not contain an Attribute or Association 'missing'", 4, 11);
      TestExpectedError("a - nested.missing", "Entity 'Nested' does not contain an Attribute or Association 'missing'", 11, 18);
    }

    private void TestExpectedSuccess(string formula, DataType expectedType) {
      MessageBucket errors = new MessageBucket();
      IParseElement element = new XmlElement("Dummy") { Start = new PositionMark() };
      FormulaParser parser = new FormulaParser(errors, new AllEntities(errors, new Entity[0]), _enums, _functions);
      ExpBase expression = parser.Parse(element, formula, new ExpDataType(_entity));
      TestUtils.DumpMessages(errors, _output);

      Assert.Equal(0, errors.Count);
      Assert.False(expression is ExpUnknown);
      Assert.Same(expectedType, expression.DataType.DataType);
    }

    private void TestExpectedError(string formula, string expectedError, int startCharPos, int endCharPos) {
      _errors.Clear();
      IParseElement element = new XmlElement("Dummy") {
        Start = new PositionMark() {
          LineNumber = 10,
          CharacterPosition = 100,
          Index = 1000,
        },
      };

      FormulaParser parser = new FormulaParser(_errors, new AllEntities(_errors, new Entity[0]), _enums, _functions);
      parser.Parse(element, formula, new ExpDataType(_entity));
      TestUtils.DumpMessages(_errors, _output);
      Assert.Equal(1, _errors.ErrorCount);

      CompileMessage message = _errors.Errors.Single();
      IParseElement newElement = message.ParseElement;

      Assert.Equal(expectedError, message.Message);
      Assert.Same(newElement.FileInfo, element.FileInfo);
      Assert.Equal(10, newElement.Start.LineNumber);
      Assert.Equal(100 + startCharPos, newElement.Start.CharacterPosition);
      Assert.Equal(100 + endCharPos, newElement.End.CharacterPosition);
    }
  }
}