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
  public class FakeDataGenerationTest {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages;

    public FakeDataGenerationTest(ITestOutputHelper output) {
      _output = output;
      _messages = new MessageBucket();
    }

    [Fact]
    public void GenerateLargeData() {
      string INPUT_DIR = "../../../../x10/examples/flexport";
      string OUTPUT_FILE = @"C:\TEMP\x10_flexport_data.sql";

      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.Compile(INPUT_DIR);

      TestUtils.DumpMessages(_messages, _output, CompileMessageSeverity.Error);
      Assert.False(_messages.HasErrors);

      MessageBucket genMessages = new MessageBucket();
      FakeDataGenerator generator = new FakeDataGenerator(genMessages, entities, new Random(0));
      generator.Generate(OUTPUT_FILE);
      TestUtils.DumpMessages(genMessages, _output, CompileMessageSeverity.Error);

      Assert.False(genMessages.HasErrors);
    }

    [Fact]
    public void GenerateSingleEntityData() {
      string yaml = @"
name: Entity
datagen_quantity: 10

attributes:
  - name: integer
    dataType: Integer
    datagen_min: 7
    datagen_max: 8

  - name: float
    dataType: Float
    datagen_min: 10.5
    datagen_max: 10.6

  - name: charReplacement
    dataType: String
    datagen_pattern: ~ZZ.DD.LLL~

  - name: percentagePattern
    dataType: String
    datagen_pattern: (50% = <adjective> <noun> | 50% = <noun>)
    datagen_capitalize: true
";

      string expected =
@"INSERT INTO entity (id, integer, float, char_replacement, percentage_pattern) VALUES
(1, 8, 10.576802268939467, 'ZZ.52.OXL', 'Division'),
(2, 7, 10.563265907281668, 'ZZ.49.AWZ', 'Equity'),
(3, 8, 10.599190217535565, 'ZZ.06.NYR', 'Bid'),
(4, 7, 10.52971718657283, 'ZZ.96.TAJ', 'Well-informed Leather'),
(5, 7, 10.527345148347013, 'ZZ.97.ILD', 'Ill-informed Rain'),
(6, 7, 10.519491343674945, 'ZZ.88.TWR', 'Crash'),
(7, 8, 10.508657988351144, 'ZZ.81.QVF', 'Joey'),
(8, 8, 10.580946258027547, 'ZZ.17.SDX', 'Large Manufacturing'),
(9, 8, 10.546839774142411, 'ZZ.88.UTY', 'Measly Pint'),
(10, 7, 10.582061007331154, 'ZZ.21.BQS', 'Trend');

";

      RunTest(expected, yaml);
    }

    [Fact]
    public void GenerateMultipleEntityData() {
      string independent = @"
name: Independent
datagen_quantity: 3

attributes:
  - name: name
    dataType: String
    datagen_pattern: <first_name>
";

      string parent = @"
name: Parent
datagen_quantity: 1

attributes:
  - name: noun
    dataType: String
    datagen_pattern: <noun>

associations:
  - name: independent
    dataType: Independent
  - name: singleChild
    dataType: SingleChild
    owns: True
  - name: multipleChildren
    dataType: MultipleChild
    owns: True
    many: True
    datagen_quantity: 3
";

      string singleChild = @"
name: SingleChild

attributes:
  - name: name
    dataType: String
    datagen_pattern: Single Child ~DDD~
";

      string multipleChild = @"
name: MultipleChild

attributes:
  - name: name
    dataType: String
    datagen_pattern: Multiple Child ~DDD~
";

      string expected =
@"INSERT INTO independent (id, name) VALUES
(1, 'Liane'),
(2, 'Altha'),
(3, 'Delilah');

INSERT INTO parent (id, noun, independent) VALUES
(1, 'hunter', 3);

INSERT INTO single_child (id, name, parent_id) VALUES
(1, 'Single Child 246', 1);

INSERT INTO multiple_child (id, name, parent_id) VALUES
(1, 'Multiple Child 089', 1),
(2, 'Multiple Child 388', 1),
(3, 'Multiple Child 065', 1);

";

      RunTest(expected, independent, parent, singleChild, multipleChild);
    }

    // Reminiscent of Rails polymorphic associations where a child entity may be "owned"
    // by multiple object types
    [Fact]
    public void GeneratePolymorphic() {
      string independent1 = @"
name: Independent1
datagen_quantity: 1
attributes:
  - name: column
    dataType: Integer
associations:
  - name: child
    dataType: Child
    owns: True
";

      string independent2 = @"
name: Independent2
datagen_quantity: 1
attributes:
  - name: column
    dataType: Integer
associations:
  - name: child
    dataType: Child
    owns: True
";

      string child = @"
name: Child
";


      string expected =
@"INSERT INTO independent1 (id, column) VALUES
(1, 9);

INSERT INTO independent2 (id, column) VALUES
(1, 3);

INSERT INTO child (id, independent1_id, independent2_id) VALUES
(1, 1, NULL),
(2, NULL, 1);

";

      RunTest(expected, independent1, independent2, child);
    }

    [Fact]
    public void GenerateFromExternalFiles() {
      string yaml = @"
name: Entity
datagen_quantity: 10
datagen_sources: (25% = us_cities.csv AS us | 75% = cn_cities.csv AS cn)

attributes:
  - name: city
    dataType: String
    datagen_from_source: (us => city | cn => city)

  - name: stateOrProvince
    dataType: String
    datagen_from_source: (us => state_id | cn => admin)

  - name: contryCode
    dataType: String
    datagen_from_source: (us => 'US' | cn => 'CN')
";

      string expected =
@"INSERT INTO entity (id, city, state_or_province, contry_code) VALUES
(1, 'Tongchuan', 'Sichuan', 'CN'),
(2, 'Liangshi', 'Hunan', 'CN'),
(3, 'Henderson', 'IL', 'US'),
(4, 'Shangdu', 'Inner Mongolia', 'CN'),
(5, 'Hengzhou', 'Guangxi', 'CN'),
(6, 'Shishan', 'Jiangsu', 'CN'),
(7, 'Qiaoshang', 'Yunnan', 'CN'),
(8, 'Xinyang', 'Henan', 'CN'),
(9, 'Piqan', 'Xinjiang', 'CN'),
(10, 'Jijiang', 'Chongqing', 'CN');

";

      RunTest(expected, yaml);
    }

    // A -owns-> B -dpends_on-> C
    [Fact]
    public void GenerateWithIndirectDependency() {
      string a = @"
name: tableA
datagen_quantity: 1
attributes:
  - name: column
    dataType: Integer
associations:
  - name: b
    dataType: tableB
    owns: True
";

      string b = @"
name: tableB
associations:
  - name: c
    dataType: tableC
";


      string c = @"
name: tableC
datagen_quantity: 1
attributes:
  - name: column
    dataType: Integer
";

      string expected =
@"INSERT INTO table_a (id, column) VALUES
(1, 9);

INSERT INTO table_c (id, column) VALUES
(1, 3);

INSERT INTO table_b (id, c, table_a_id) VALUES
(1, 1, 1);

";

      RunTest(expected, a, b, c);
    }

    [Fact]
    public void ErrorInvalidSources() {
      string yaml = @"
name: Entity
datagen_quantity: 10
datagen_sources: (25% = us_cities.csv AS us | 10% = blurg)

attributes:
  - name: city
    dataType: String
    datagen_from_source: (us => city | cn => city)
";

      RunTestExpectingError(yaml, "Expected format: 'file.csv AS alias', but got 'blurg'");
    }

    private void RunTest(string expected, params string[] yamls) {
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.CompileFromYamlStrings(yamls);

      FakeDataGenerator generator = new FakeDataGenerator(_messages, entities, new Random(0));
      string sql = generator.GenerateIntoString();
      _output.WriteLine(sql);

      TestUtils.DumpMessages(_messages, _output);
      _output.WriteLine("");

      Assert.Equal(expected, sql);
    }

    private void RunTestExpectingError(string yaml, string expectedErrorMessage) {
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages));
      List<Entity> entities = compiler.CompileFromYamlStrings(yaml);

      TestUtils.DumpMessages(_messages, _output);
      Assert.False(_messages.HasErrors);

      FakeDataGenerator generator = new FakeDataGenerator(_messages, entities, new Random(0));
      generator.GenerateIntoString();
      TestUtils.DumpMessages(_messages, _output);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);
    }
  }
}
