using x10.model.definition;
using x10.parsing;

namespace x10.formula {

  public abstract class ExpBase : IParseElement {
    public abstract ExpDataType DetermineTypeRaw(ExpDataType rootType);
    public abstract void Accept(IVisitor visitor);
     
    internal FormulaParser Parser { get; private set; }
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

    internal ExpDataType DetermineType(ExpDataType rootType) {
      DataType = DetermineTypeRaw(rootType);
      return DataType;
    }
  }
}
