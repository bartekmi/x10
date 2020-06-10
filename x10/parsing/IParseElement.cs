using System;
using System.Collections.Generic;
using System.Text;

namespace x10.parsing {
  public interface IParseElement {
    FileInfo FileInfo { get; }
    PositionMark Start { get; set; }
    PositionMark End { get; set; }
    void SetFileInfo(FileInfo fileInfo);
  }

  public static class IParseElementExtensions {
    public static void SetRelativeTo(this IParseElement target, IParseElement relativeTo, int startOffset, int endOffset) {
      target.SetFileInfo(relativeTo.FileInfo);
      target.Start = relativeTo.Start.AdvanceBy(startOffset);
      target.End = relativeTo.Start.AdvanceBy(endOffset);
    }
  }
}
