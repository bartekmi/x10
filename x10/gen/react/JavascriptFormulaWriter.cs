using System;
using System.Linq;
using System.IO;

using x10.formula;
using x10.utils;
using x10.model.metadata;

namespace x10.gen.react {
  internal class JavaScriptFormulaWriter : IVisitor {

    private TextWriter _writer;
    private string _variableName;

    internal JavaScriptFormulaWriter(TextWriter writer, string variableName) {
      _writer = writer;
      _variableName = variableName;
    }

    public void VisitBinary(ExpBinary exp) {
      exp.Left.Accept(this);
      _writer.Write(" {0} ", exp.Token);
      exp.Right.Accept(this);
    }

    public void VisitIdentifier(ExpIdentifier exp) {
      // This is the case for Enum Names
      if (exp.DataType == null)
        return;

      if (exp.Name == FormulaParser.CONTEXT_NAME)
        _writer.Write("Context");
      else 
        _writer.Write("{0}.{1}", _variableName, exp.Name);
    }

    public void VisitInvocation(ExpInvocation exp) {
      _writer.Write(exp.FunctionName);
      _writer.Write("(");
      foreach (ExpBase argument in exp.Arguments) {
        argument.Accept(this);
        if (argument != exp.Arguments.Last())
          _writer.Write(", ");
      }
      _writer.Write(")");
    }

    public void VisitLiteral(ExpLiteral exp) {
      if (WriteEnum(exp, exp.Value))
        return;
      _writer.Write(ReactCodeGenerator.TypedLiteralToString(exp.Value, exp.DataType.DataTypeAsEnum));
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      if (WriteEnum(exp, exp.MemberName))
        return;

      exp.Expression.Accept(this);
      _writer.Write("?.");
      _writer.Write(exp.MemberName);
    }

    public void VisitParenthesized(ExpParenthesized exp) {
      _writer.Write("(");
      exp.Expression.Accept(this);
      _writer.Write(")");
    }

    public void VisitUnary(ExpUnary exp) {
      _writer.Write(exp.Token);
      exp.Expression.Accept(this);
    }

    public void VisitUnknown(ExpUnknown exp) {
      // This should never happen if the x10 code was compiled cleanly
    }

    private bool WriteEnum(ExpBase expression, object nameOrNull) {
      X10DataType dataType = expression.DataType;

      if (expression.IsEnumLiteral) {
        if (nameOrNull == null)
          _writer.Write("null");
        else
          _writer.Write("{0}.{1}", dataType.DataType?.Name, NameUtils.Capitalize(nameOrNull.ToString()));
        return true;
      }

      return false;
    }
  }
}
