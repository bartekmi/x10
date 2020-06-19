using x10.model.definition;
using x10.parsing;

namespace x10.formula {

  public abstract class ExpBase : IParseElement {
    public abstract ExpDataType DetermineType(ExpDataType rootType);
    public abstract void Accept(IVisitor visitor);
     
    protected FormulaParser Parser { get; private set; }

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

    protected ExpBase(FormulaParser parser) {
      Parser = parser;
    }
  }
}
