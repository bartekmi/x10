using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.gql {

  public class GqlTypeReference {
    public string Name { get; internal set; }
    public bool IsMandatory { get; internal set; }
    public bool IsMany { get; internal set; }

    public override string ToString() {
      if (IsMany)
        return string.Format("[{0}!]!", Name);
      return Name + (IsMandatory ? "!" : "");
    }
  }

  public class GqlField {
    public string Name { get; internal set; }
    public string Description { get; internal set; }
    public GqlTypeReference Type { get; internal set; }
    public IEnumerable<GqlArgument> Arguments { get; private set; }

    internal GqlField(IEnumerable<GqlArgument> arguments) {
      Arguments = arguments;
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      if (Description != null)
        builder.AppendLine(string.Format("  \"{0}\"", Description));

      builder.AppendLine(string.Format("  {0}{1}: {2}",
        Name,
        Arguments.Count() == 0 ?
          null :
          string.Format("({0})", string.Join(", ", Arguments.Select(x => x.ToString()))),
        Type.ToString()));

      return builder.ToString();
    }
  }
}