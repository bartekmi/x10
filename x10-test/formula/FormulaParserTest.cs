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
    private readonly Dictionary<string, X10DataType> _otherAvailableVariables;
    private AllEntities _allEntities;
    private Entity _entity;

    #region Test Initialization
    public FormulaParserTest(ITestOutputHelper output) {
      _output = output;
      _enums = new AllEnums(_errors);
      _functions = new AllFunctions(_errors);
      _otherAvailableVariables = new Dictionary<string, X10DataType>() {
        { "stateInt", new X10DataType(DataTypes.Singleton.Integer) },
        { "stateString", new X10DataType(DataTypes.Singleton.String) },
      };

      InitializeEntities();
      InitializeFunctions();

      if (_errors.HasErrors) {
        TestUtils.DumpMessages(_errors, _output);
        Assert.False(_errors.HasErrors);
      }
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
  - name: many
    dataType: Nested
    many: true
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
      string myFuncYaml = @"
name: MyFunc
description: Take a string and an int
returnDataType: String
arguments:
  - name: aString
    dataType: String
  - name: anInt
    dataType: Integer
";
      string funcWithEnumYaml = @"
name: FuncWithEnum
description: Take a string and an enum
returnDataType: Integer
arguments:
  - name: aString
    dataType: String
  - name: anEnum
    dataType: MyEnum
";
      FunctionsCompiler funcCompiler = new FunctionsCompiler(_errors, _allEntities, _enums, _functions, new AttributeReader(_errors));
      funcCompiler.CompileFunctionFromString(myFuncYaml);
      funcCompiler.CompileFunctionFromString(funcWithEnumYaml);
      Assert.Equal(2, _functions.All.Count());
    }
    #endregion

    #region Success 
    [Fact]
    public void ParseSuccessful() {
      Entity nested = _allEntities.FindEntityByName("Nested");
      Assert.NotNull(nested);

      TestExpectedSuccess("1 + 2.7", DataTypes.Singleton.Float);                    // Binary - Literals
      TestExpectedSuccess("a + c", DataTypes.Singleton.Integer);                    // Binary - Identifiers
      TestExpectedSuccess("\"Hello\" + \" World\"", DataTypes.Singleton.String);    // Binary of Strings
      TestExpectedSuccess("(1 + a) / b", DataTypes.Singleton.Float);                // Parenthesis
      TestExpectedSuccess("MyFunc(myString, a)", DataTypes.Singleton.String);       // Function
      TestExpectedSuccess("a * b + c * d / e", DataTypes.Singleton.Float);          // Compound expression
      TestExpectedSuccess("a < 7", DataTypes.Singleton.Boolean);                    // Inequalifty
      TestExpectedSuccess("myBoolean && (a < 7)", DataTypes.Singleton.Boolean);     // Logical expression
      TestExpectedSuccess("nested.attr", DataTypes.Singleton.Date);                 // Member access
      TestExpectedSuccess("myString + \" \" + a", DataTypes.Singleton.String);      // String-add
      TestExpectedSuccess("many.count", DataTypes.Singleton.Integer);               // Multiple: Count
      TestExpectedSuccess("many.first", new X10DataType(nested, false));            // Multiple: First
      TestExpectedSuccess("many.last", new X10DataType(nested, false));             // Multiple: Last
      TestExpectedSuccess("stateInt > 10", DataTypes.Singleton.Boolean);            // State
      TestExpectedSuccess("-a", DataTypes.Singleton.Integer);                       // Unary minus
      TestExpectedSuccess("!myBoolean", DataTypes.Singleton.Boolean);               // Unary negation
    }

    // Assume an expression A.B.C
    // Note that the expression tree is built in such a way that the top-level expression is
    // (A.B) . C
    [Fact]
    public void ParsePropertyOfDataType() {
      ExpBase expression = TestExpectedSuccess("nested.attr.year", DataTypes.Singleton.Integer);         

      Assert.True(expression is ExpMemberAccess);
      ExpMemberAccess expMemberAccess = (ExpMemberAccess)expression;

      Assert.Equal("year", expMemberAccess.MemberName);
      Assert.Same(DataTypes.Singleton.Date, expMemberAccess.Expression.DataType.DataType);
    }

    #endregion

    #region Enumerated Types
    [Fact]
    public void ParseEnum() {
      TestExpectedSuccess("myEnumValue == MyEnum.three", DataTypes.Singleton.Boolean);    
    }

    [Fact]
    // TODO: This doesn't actually test the fact that string was upgraded to enum
    public void UpgradeEnumInCompareOnRight() {
      TestExpectedSuccess("myEnumValue == \"three\"", DataTypes.Singleton.Boolean);
    }

    [Fact]
    // TODO: This doesn't actually test the fact that string was upgraded to enum
    public void UpgradeEnumInCompareOnLeft() {
      TestExpectedSuccess("\"three\" == myEnumValue", DataTypes.Singleton.Boolean);
    }

    [Fact]
    public void UpgradeEnumInFunctionArg() {
      TestExpectedSuccess("1 + FuncWithEnum(\"blah\", \"three\")", DataTypes.Singleton.Integer);
    }

    [Fact]
    public void AttemptEnumUpgradeWrongType() {
      TestExpectedError("myEnumValue == 1", "Attempted to upgrade '1' to Enumerated Type MyEnum. Expected a String, but got data type 'Int32' instead.", 15, 16);
    }

    [Fact]
    public void AttemptEnumUpgradeWrongValue() {
      TestExpectedError("myEnumValue == bogus", "Identifier 'bogus' is not a State variable: [stateInt, stateString] and not a Member of type: Entity", 15, 20);
    }
    #endregion

    #region Parsing Errors
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
    public void DataTypeMismatchBinary() {
      TestExpectedError("a - myBoolean", "Cannot subtract Integer and Boolean", 2, 3);
      TestExpectedError("a % b", "Cannot take remainder of Integer and Float", 2, 3);
      TestExpectedError("a + myBoolean", "Cannot add Integer and Boolean", 2, 3);
      TestExpectedError("a && myBoolean", "Cannot logical-and Integer and Boolean", 2, 4);
      TestExpectedError("a > myBoolean", "Cannot compare Integer and Boolean", 2, 3);
      TestExpectedError("a == myBoolean", "Cannot test equality of Integer and Boolean", 2, 4);
    }

    [Fact]
    public void DataTypeMismatchUnary() {
      TestExpectedError("-myBoolean", "Cannot apply '-' to type Boolean", 0, 10);
      TestExpectedError("!a", "Cannot apply '!' to type Integer", 0, 2);
    }

    [Fact]
    public void MissingAttribute() {
      TestExpectedError("a - missing", "Identifier 'missing' is not a State variable: [stateInt, stateString] and not a Member of type: Entity", 4, 11);
      TestExpectedError("a - nested.missing", "Entity 'Nested' does not contain an Attribute or Association 'missing'", 11, 18);
    }

    [Fact]
    public void BadManyAttribute() {
      TestExpectedError("many.notCount", "notCount is not a valid property of a collection. The only valid properties are: count, first, last", 5, 13);
    }
    #endregion

    #region Utils
    private ExpBase TestExpectedSuccess(string formula, DataType expectedType) {
      return TestExpectedSuccess(formula, new X10DataType(expectedType));
    }

    private ExpBase TestExpectedSuccess(string formula, X10DataType expectedType) {
      MessageBucket errors = new MessageBucket();
      IParseElement element = new XmlElement("Dummy") { Start = new PositionMark() };
      FormulaParser parser = new FormulaParser(errors, new AllEntities(errors, new Entity[0]), _enums, _functions) {
        OtherAvailableVariables = _otherAvailableVariables,
      };
      ExpBase expression = parser.Parse(element, formula, new X10DataType(_entity, false));
      TestUtils.DumpMessages(errors, _output);

      Assert.Equal(0, errors.Count);
      Assert.False(expression is ExpUnknown);
      Assert.Equal(expectedType, expression.DataType);

      return expression;
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

      FormulaParser parser = new FormulaParser(_errors, new AllEntities(_errors, new Entity[0]), _enums, _functions) {
        OtherAvailableVariables = _otherAvailableVariables,
      };
      parser.Parse(element, formula, new X10DataType(_entity, false));
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
  #endregion
}