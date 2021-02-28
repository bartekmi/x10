using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.gql {
  public class GqlSchema {
    public IEnumerable<GqlTypeDefinition> Types { get; internal set; }

    private Dictionary<string, GqlTypeDefinition> _types;

    internal GqlSchema(IEnumerable<GqlTypeDefinition> types) {
      _types = types.ToDictionary(x => x.Name, x => x);
    }

    public GqlTypeDefinition Mutations => _types["Mutations"];

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      foreach( GqlTypeDefinition type in _types.Values) 
        builder.AppendLine(type.ToString());

      return builder.ToString();
    }
  }
}