using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.gen.sql.primitives;

namespace x10.gen.sql {
  public class FakeDataPrinter {
    private string _outputDir;

    public FakeDataPrinter(string outputDir) {
      _outputDir = outputDir;
    }

    public void Print(FakeDataGenerator generator) {
      foreach (EntityInfo entityInfo in generator.EntityInfos.Values)
        PrintEntityInfo(entityInfo);
    }

    public void PrintEntityInfo(EntityInfo entityInfo) {
      IEnumerable<Row> rows = entityInfo.NonOwnedRows;
      if (rows.Count() == 0)
        return;

      if (!Directory.Exists(_outputDir))
        Directory.CreateDirectory(_outputDir);

      string path = Path.Combine(_outputDir, entityInfo.Entity.Name + ".json");
      using TextWriter writer = new StreamWriter(path);

      PrintRows(0, writer, rows, false);
    }

    private void PrintRows(int level, TextWriter writer, IEnumerable<Row> rows, bool appendComma) {
      if (rows.Count() == 0) {
        writer.WriteLine("[],");
        return;
      }

      writer.WriteLine("[");

      foreach (Row row in rows)
        PrintRow(level + 1, writer, row, row != rows.Last());

      writer.WriteLine("{0}]{1}", Spacer(level), appendComma ? "," : "");
    }

    private void PrintRow(int level, TextWriter writer, Row row, bool appendComma) {
      writer.WriteLine("{0}{{", Spacer(level));
      bool hasChildAssociations = row.ChildAssociations != null && row.ChildAssociations.Count > 0;

      // Attributes and Non-Owned Associations
      foreach (MemberAndValue mav in row.Values)
        writer.WriteLine("{0}\"{1}\": {2}{3}", 
          Spacer(level + 1), 
          mav.Member.Name, 
          ObjectToJson(mav.Value),
          mav == row.Values.Last() && !hasChildAssociations ? "" : ",");  // Only append comman if not last item

      // Owned Associations
      if (row.ChildAssociations != null) {
        var associations = row.ChildAssociations.ToList();

        for (int ii = 0; ii < associations.Count; ii++) {
          bool isLast = ii == associations.Count - 1;
          var associationData = associations[ii];
          Association assoc = associationData.Key;
          List<Row> assocRows = associationData.Value;

          writer.Write("{0}\"{1}\": ", Spacer(level + 1), assoc.Name);

          if (assoc.IsMany)
            PrintRows(level + 1, writer, assocRows, !isLast);
          else {
            Row assocRow = assocRows.SingleOrDefault();
            if (assocRow == null)
              writer.Write("null,");
            else
              PrintRow(level + 1, writer, assocRow, !isLast);
          }
        }
      }

      writer.WriteLine("{0}}}{1}", Spacer(level), appendComma ? "," : "");
    }

    private static string Spacer(int level) {
      return new string(' ', level * 2);
    }

    private static string ObjectToJson(object literal) {
      if (literal == null)
        return "null";

      if (literal is string str) {
        return string.Format("\"{0}\"", literal);
      } else if (literal is bool)
        return literal.ToString().ToLower();
      else
        return literal.ToString();
    }
  }
}