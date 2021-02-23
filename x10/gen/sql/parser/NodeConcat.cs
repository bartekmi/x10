using System;
using System.Text;
using System.Linq;


namespace x10.gen.sql.parser {
  class NodeConcat : Node {
    internal override void Print(StringBuilder builder) {
      foreach (Node child in Children)
        child.Print(builder);
    }

    // Trim whitespace at both ends
    internal void Trim() {
      if (Children.First() is NodeText first)
        first.TrimStart();
      if (Children.Last() is NodeText last)
        last.TrimEnd();
    }
  }
}