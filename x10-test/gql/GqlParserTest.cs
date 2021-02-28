using System;

using Xunit;
using Xunit.Abstractions;

namespace x10.gql {
  public class GqlParserTest {
    [Fact]
    public void Parse() {
      string gqlSchema = @"
type MyNested {
  myString: String
}

""Description of Type""
type MyType {
  myOptional: String
  myMandatory: Boolean!
  myMultiple: [String!]!
  myNestedOptional: MyType
  myNestedMandatory: MyType!
  myNestedMultiple: [MyType!]!
}

type Mutation {
  ""Description of Field""
  myMutation1(myArg1: String!, myArg2: Int, myArg3: [MyNested!]!): [MyType!]!
}
";

      GqlSchema schema = GqlParser.Parse(gqlSchema);
      Console.WriteLine(schema.ToString());

      Assert.Equal(gqlSchema.Trim(), schema.ToString().Trim());
    }
  }
}