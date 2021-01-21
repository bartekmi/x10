using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace x10.utils {
  public static class PrintUtils {

    public const int INDENT_SIZE = 2;

    public static string Indent(int indent) {
      return new String(' ', indent * INDENT_SIZE);
    }

    public static void Indent(TextWriter writer, int indent) {
      writer.Write(Indent(indent));
    }

    public static void WriteLineIndented(TextWriter writer, int indent, string format, params object[] args) {
      Indent(writer, indent);
      if (args.Length == 0)
        writer.WriteLine(format);
      else
        writer.WriteLine(format, args);
    }
  }
}
