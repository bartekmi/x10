using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.gql {
  public class GqlTypeDefinition {
    public string Name { get; internal set; }
    public string Description {get; internal set; }

    public IEnumerable<GqlField> Fields {get; private set;}

    internal GqlTypeDefinition(IEnumerable<GqlField> fields) {
      Fields = fields;
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      if (Description != null)
        builder.AppendLine(string.Format("\"{0}\"", Description));

      builder.AppendLine(string.Format("type {0} {{", Name));

      foreach( GqlField field in Fields) 
        builder.Append(field.ToString());

      builder.AppendLine("}");

      return builder.ToString();
    }
  }
}