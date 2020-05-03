using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace x10.gen.sql.primitives {
  internal class ExternalDataFile : IWithProbability {
    internal string Path;
    internal string Alias;
    public double Probability { get; internal set; }

    private List<DataFileRow> _rows = new List<DataFileRow>();
    internal Dictionary<string, int> ColumnNameToIndex;

    internal DataFileRow GetRandomRow(Random random) {
      int index = random.Next(_rows.Count);
      return _rows[index];
    }

    internal void Parse() {
      string[] lines = File.ReadAllLines(Path);

      ColumnNameToIndex = ExtractColumns(lines[0]);

      for (int ii = 1; ii < lines.Length; ii++)
        _rows.Add(ExtractRow(lines[ii]));
    }

    private Dictionary<string, int> ExtractColumns(string line) {
      Dictionary<string, int> columnNameToIndex = new Dictionary<string, int>();
      string[] columnNames = line.Split(',');
      for (int ii = 0; ii < columnNames.Length; ii++)
        columnNameToIndex[columnNames[ii]] = ii;
      return columnNameToIndex;
    }

    private DataFileRow ExtractRow(string line) {
      return new DataFileRow() {
        Data = line.Split(','),
        Owner = this,
      };
    }
  }

  internal class DataFileRow {
    internal string[] Data;
    internal ExternalDataFile Owner;

    internal string GetValue(string columnName) {
      int index = Owner.ColumnNameToIndex[columnName];
      return Data[index];
    }
  }
}
