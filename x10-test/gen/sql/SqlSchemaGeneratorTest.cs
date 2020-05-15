using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model;
using x10.ui.libraries;
using x10.ui.metadata;
using x10.compiler;
using x10.model.definition;

namespace x10.gen.sql {
  public class SqlSchemaGeneratorTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public SqlSchemaGeneratorTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
    }

    [Fact]
    public void GenerateSqlSchemaLargetExample() {
      string INPUT_DIR = "../../../../x10/examples/flexport";
      string OUTPUT_FILE = @"C:\TEMP\x10_schema.sql";

      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.Compile(INPUT_DIR);

      Assert.False(_messages.HasErrors);

      SqlSchemaGenerator.Generate(entities, OUTPUT_FILE);
    }

    [Fact]
    public void GenerateSqlSchemaSingleEntity() {
      string yaml1 = @"
name: Entity

attributes:
  - name: myString
    dataType: String
    mandatory: true
  - name: myInt
    dataType: Integer
  - name: myFloat
    dataType: Float
  - name: myDate
    dataType: Date
  - name: myTimestamp
    dataType: Timestamp
  - name: myBoolean
    dataType: Boolean
  - name: myMoney
    dataType: Money
";

      RunTest(
@"------------------------ Tables ------------------------------
CREATE TABLE ""entity"" (
  id serial PRIMARY KEY,
  my_string VARCHAR NOT NULL,
  my_int INTEGER NULL,
  my_float DOUBLE PRECISION NULL,
  my_date DATE NULL,
  my_timestamp TIMESTAMP NULL,
  my_boolean BOOLEAN NULL,
  my_money MONEY NULL
);



------------------------ Foreign Key Constraints ------------------------------
", yaml1);
    }

    [Fact]
    public void GenerateSqlSchemaReverseReference() {
      string yaml1 = @"
name: Parent

attributes:
  - name: parentString
    dataType: String
associations:
  - name: children
    dataType: Child
    many: true
    owns: true
";
      string yaml2 = @"
name: Child

attributes:
  - name: childString
    dataType: String
";

      RunTest(
@"------------------------ Tables ------------------------------
CREATE TABLE ""parent"" (
  id serial PRIMARY KEY,
  parent_string VARCHAR NULL
);

CREATE TABLE ""child"" (
  id serial PRIMARY KEY,
  child_string VARCHAR NULL,

  parent_id INTEGER NOT NULL
);



------------------------ Foreign Key Constraints ------------------------------
-- Related to Table Child
ALTER TABLE ""child"" ADD CONSTRAINT child_parent_fkey FOREIGN KEY(parent_id) REFERENCES ""parent""(id);

", yaml1, yaml2);
    }

    [Fact]
    public void GenerateSqlSchemaForwardReference() {
      string yaml1 = @"
name: Entity

associations:
  - name: other
    mandatory: true
    dataType: Other
";
      string yaml2 = @"
name: Other

attributes:
  - name: childString
    dataType: String
";

      RunTest(
@"------------------------ Tables ------------------------------
CREATE TABLE ""entity"" (
  id serial PRIMARY KEY,
  other_id INTEGER NOT NULL
);

CREATE TABLE ""other"" (
  id serial PRIMARY KEY,
  child_string VARCHAR NULL
);



------------------------ Foreign Key Constraints ------------------------------
-- Related to Table Entity
ALTER TABLE ""entity"" ADD CONSTRAINT entity_other_fkey FOREIGN KEY(other_id) REFERENCES ""other""(id);

", yaml1, yaml2);
    }

    [Fact]
    public void GenerateSqlSchemaMutualReference() {
      string yaml1 = @"
name: Entity

associations:
  - name: other
    dataType: Other
    mandatory: true
    owns: true
";
      string yaml2 = @"
name: Other

attributes:
  - name: childString
    dataType: String
associations:
  - name: entity
    dataType: Entity
    mandatory: true
";

      RunTest(
@"------------------------ Tables ------------------------------
CREATE TABLE ""entity"" (
  id serial PRIMARY KEY,
);

CREATE TABLE ""other"" (
  id serial PRIMARY KEY,
  child_string VARCHAR NULL,

  entity_id INTEGER NOT NULL
);



------------------------ Foreign Key Constraints ------------------------------
-- Related to Table Other
ALTER TABLE ""other"" ADD CONSTRAINT other_entity_fkey FOREIGN KEY(entity_id) REFERENCES ""entity""(id);

", yaml1, yaml2);
    }

    #region Utils
    private void RunTest(string expected, params string[] yamls) {
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.CompileFromYamlStrings(yamls);

      TestUtils.DumpMessages(_messages, _output);
      _output.WriteLine("");

      string result = SqlSchemaGenerator.GenerateIntoString(entities);
      _output.WriteLine(result);

      Assert.Equal(expected, result);
    }
    #endregion
  }
}
