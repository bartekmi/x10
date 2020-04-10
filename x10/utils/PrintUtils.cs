using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace x10.utils {
  public static class PrintUtils {

    public const int INDENT_SIZE = 2;

    public static void Indent(TextWriter writer, int indent) {
      writer.Write(new String(' ', indent * INDENT_SIZE));
    }
  }
}
