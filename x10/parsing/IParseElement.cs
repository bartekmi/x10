using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public interface IParseElement {
    FileInfo FileInfo { get;  }
    PositionMark Start { get; }
    PositionMark End { get; }
    void SetFileInfo(string path);
  }
}
