using x10.model.definition;
using x10.parsing;

namespace x10.formula {

  public abstract class ExpBase : IParseElement {
    public abstract ExpDataType DetermineType(MessageBucket errors, Entity context, ExpDataType rootType);

    // Corruently, this is only set for the top level.
    // Verify likely, at some point we'll need this to be set on 
    // all expressions
    public ExpDataType DataType { get; internal set; }

    // IParseElement
    public FileInfo FileInfo { get; private set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }
    public void SetFileInfo(FileInfo fileInfo) {
      FileInfo = fileInfo;
    }
  }
}
