using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using x10.formula;
using x10.utils;
using System.Linq;

namespace x10.gen.wpf {
  internal class WpfFormulaWriter : IVisitor {

    private TextWriter _writer;

    internal WpfFormulaWriter(TextWriter writer) {
      _writer = writer;
    }

    public void VisitBinary(ExpBinary exp) {
      exp.Left.Accept(this);
      _writer.Write(" {0} ", exp.Token);
      exp.Right.Accept(this);
    }

    public void VisitIdentifier(ExpIdentifier exp) {
      if (exp.Name == FormulaParser.CONTEXT_NAME)
        _writer.Write("AppStatics.Singleton.Context");
      else
        _writer.Write(NameUtils.Capitalize(exp.Name));
    }

    public void VisitInvocation(ExpInvocation exp) {
      // For now, all methods are assumed to be a members of a global static class called 'Functions'
      _writer.Write("Functions.");

      _writer.Write(NameUtils.Capitalize(exp.FunctionName));
      _writer.Write("(");
      foreach (ExpBase argument in exp.Arguments) {
        argument.Accept(this);
        if (argument != exp.Arguments.Last())
          _writer.Write(", ");
      }
      _writer.Write(")");
    }

    public void VisitLiteral(ExpLiteral exp) {
      _writer.Write(WpfGenUtils.TypedLiteralToString(exp.Value));
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      exp.Expression.Accept(this);
      _writer.Write(".");
      _writer.Write(NameUtils.Capitalize(exp.MemberName));
    }

    public void VisitParenthesized(ExpParenthesized exp) {
      _writer.Write("(");
      exp.Expression.Accept(this);
      _writer.Write(")");
    }

    public void VisitUnknown(ExpUnknown exp) {
      // This should never happen if the x10 code was compiled cleanly
    }
  }
}
