using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using x10.utils;

namespace x10.gen.sql.primitives {
  internal class ExternalDataFile : IWithProbability {
    internal string Path;
    internal string Alias;
    public double Probability { get; internal set; }

    private readonly List<DataFileRow> _rows = new List<DataFileRow>();
    internal Dictionary<string, int> ColumnNameToIndex;

    internal int Count { get { return _rows.Count; } }
    internal DataFileRow GetRow(int index) {
      return _rows[index];
    }

    internal DataFileRow GetRandomRow(Random random) {
      int index = random.Next(_rows.Count);
      return _rows[index];
    }

    internal void Parse(string rootDir) {
      string fullPath = System.IO.Path.Combine(rootDir, Path);
      string[] lines = File.ReadAllLines(fullPath);

      ColumnNameToIndex = ExtractColumns(lines[0]);

      for (int ii = 1; ii < lines.Length; ii++)
        _rows.Add(ExtractRow(lines[ii]));
    }

    private Dictionary<string, int> ExtractColumns(string line) {
      Dictionary<string, int> columnNameToIndex = new Dictionary<string, int>();
      string[] columnNames = ParseCsvLine(line);
      for (int ii = 0; ii < columnNames.Length; ii++)
        columnNameToIndex[columnNames[ii]] = ii;
      return columnNameToIndex;
    }

    private DataFileRow ExtractRow(string line) {
      return new DataFileRow() {
        Data = ParseCsvLine(line),
        Owner = this,
      };
    }

    private string[] ParseCsvLine(string line) {
      return line.Split(',')
        .Select(x => NameUtils.StripQuotes(x))
        .ToArray();
    }
  }

  internal class DataFileRow {
    internal string[] Data;
    internal ExternalDataFile Owner;

    internal string GetValue(string columnName) {
      if (!Owner.ColumnNameToIndex.TryGetValue(columnName, out int index))
        throw new Exception(string.Format("Column '{0}' not found among available columns: {1} - of Data File '{2}'",
          columnName, string.Join(", ", Owner.ColumnNameToIndex.Keys), Owner.Alias));
      return Data[index];
    }
  }
}
