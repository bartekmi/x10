using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using x10.formula;
using x10.utils;
using System.Linq;
using x10.model.metadata;

namespace x10.gen.wpf {
  internal class WpfFormulaWriter : IVisitor {

    private TextWriter _writer;
    private bool _prefixWithModel; // Prefix with model when generating for VM

    internal WpfFormulaWriter(TextWriter writer, bool prefixWithModel) {
      _writer = writer;
      _prefixWithModel = prefixWithModel;
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
        _writer.Write("AppStatics.Singleton.Context");
      else {
        string name = ToIdentifier(exp.Name, exp);
        if (_prefixWithModel)
          name = WpfGenUtils.MODEL_PROPERTY_PREFIX + name;
        _writer.Write(name);
      }
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
      if (WriteEnum(exp, exp.Value.ToString()))
        return;
      _writer.Write(WpfGenUtils.TypedLiteralToString(exp.Value, exp.DataType.DataTypeAsEnum));
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      if (WriteEnum(exp, exp.MemberName))
        return;

      exp.Expression.Accept(this);
      _writer.Write("?.");

      string memberName = ToIdentifier(exp.MemberName, exp);
      _writer.Write(memberName);
    }

    private string ToIdentifier(string name, ExpBase exp) {
      return exp.DataType.Member == null ?
        NameUtils.Capitalize(name) :
        WpfGenUtils.MemberToName(exp.DataType.Member);
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

    private bool WriteEnum(ExpBase expression, string name) {
      X10DataType dataType = expression.DataType;

      if (expression.IsEnumLiteral) {
        _writer.Write("{0}.{1}", dataType.DataType?.Name, NameUtils.Capitalize(name));
        return true;
      }

      return false;
    }
  }
}
