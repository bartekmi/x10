using System;
using System.Collections.Generic;
using System.Text;
using x10.parsing;

namespace x10.formula {
  public abstract class ExpBase : IParseElement {
    public FileInfo FileInfo { get; private set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }
    public void SetFileInfo(FileInfo fileInfo) {
      FileInfo = fileInfo;
    }
  }
}
