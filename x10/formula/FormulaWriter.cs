using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using x10.formula;
using x10.utils;
using System.Linq;

namespace x10.gen.wpf {
  internal class FormulaWriter : IVisitor {

    private TextWriter _writer;

    internal FormulaWriter(TextWriter writer) {
      _writer = writer;
    }

    public void VisitBinary(ExpBinary exp) {
      exp.Left.Accept(this);
      _writer.Write(" {0} ", exp.Token);
      exp.Right.Accept(this);
    }

    public void VisitIdentifier(ExpIdentifier exp) {
      _writer.Write(exp.Name);
    }

    public void VisitInvocation(ExpInvocation exp) {
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
      _writer.Write(exp.Value);
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      exp.Expression.Accept(this);
      _writer.Write(".");
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
      _writer.Write("UNKNOWN");
    }
  }
}
