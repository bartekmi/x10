using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using x10.gen.sql.primitives;

namespace x10.gen.sql.parser {
  abstract class Node : IWithProbability {
    internal abstract void Print(StringBuilder builder);
    public double Probability { get; internal set; }
    internal List<Node> Children = new List<Node>();

    // Derived
    internal string OnlyChildText {
      get {
        return Children.OfType<NodeText>().Single().Text;
      }
    }
  }
}