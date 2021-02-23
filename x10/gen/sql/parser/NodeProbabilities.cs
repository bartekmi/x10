using System;
using System.Text;
using System.Linq;

namespace x10.gen.sql.parser {
  class NodeProbabilities : Node {
    internal override void Print(StringBuilder builder) {
      builder.Append("( ");
      foreach (Node child in Children) {
        builder.Append(child.Probability * 100);
        builder.Append("% => ");
        child.Print(builder);
        if (child != Children.Last())
          builder.Append(" | ");
      }
      builder.Append(" )");
    }
  }
}