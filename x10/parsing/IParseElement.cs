using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public interface IParseElement {
    FileInfo FileInfo { get;  }
    PositionMark Start { get; }
    PositionMark End { get; }
    void SetFileInfo(FileInfo fileInfo);
  }

  public class BasicParseElement : IParseElement {

    public FileInfo FileInfo { get; set;  }
    public PositionMark Start { get; set;  }
    public PositionMark End { get; set;  }

    public void SetFileInfo(FileInfo fileInfo) {
      FileInfo = fileInfo;
    }

    public BasicParseElement() {
      // Do nothing
    }

    public BasicParseElement(IParseElement element) {
      FileInfo = element.FileInfo;
      Start = element.Start;
    }

    internal void SetPositionRelativeToStart(int start, int end) {
      Start = Start.AdvanceBy(start);
      End = Start.AdvanceBy(end);
    }
  }
}
