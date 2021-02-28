using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.gql {
  public class GqlArgument {
    public string Name { get; internal set; }
    public string Description {get; internal set; }
    public GqlTypeReference Type {get; internal set;}

    public override string ToString() {
      return string.Format("{0}{1}: {2}",
        Description == null ? null : string.Format("\"{0}\" ", Description),
        Name,
        Type);
    }
  }
}