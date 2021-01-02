using System.Collections.Generic;
using System.Linq;
using System.IO;

using FileInfo = x10.parsing.FileInfo;
using x10.gen.wpf;
using x10.model.metadata;
using x10.parsing;

namespace x10.formula {

  public abstract class ExpBase : IParseElement {
    public abstract X10DataType DetermineTypeRaw(X10DataType rootType);
    public abstract void Accept(IVisitor visitor);
    public virtual IEnumerable<ExpBase> ChildExpressions() { return new ExpBase[0]; }

    // If this expression can be converted to a simple Path like: 'attr.association.attr',
    // return the first ExpIdentifier in the chain
    public virtual ExpIdentifier FirstMemberOfPath() { return null; }

    internal FormulaParser Parser { get; private set; }
    public X10DataType DataType { get; internal set; }
    public bool IsEnumLiteral { get; internal set; }

    // Derived
    public bool IsContext => this is ExpIdentifier identifier && identifier.Name == FormulaParser.CONTEXT_NAME;
    public bool UsesContext => FormulaUtils.ListAll(this).Any(x => x.IsContext);

    // IParseElement Implementation
    public FileInfo FileInfo { get; private set; }
    public PositionMark Start { get; set; }
    public PositionMark End { get; set; }

    public void SetFileInfo(FileInfo fileInfo) {
      FileInfo = fileInfo;
    }

    protected ExpBase(FormulaParser parser) {
      Parser = parser;
    }

    internal X10DataType DetermineType(X10DataType rootType) {
      DataType = DetermineTypeRaw(rootType);
      return DataType;
    }

    public override string ToString() {
      using StringWriter writer = new StringWriter();
      FormulaWriter visitor = new FormulaWriter(writer);
      this.Accept(visitor);
      return writer.ToString();
    }
  }
}
