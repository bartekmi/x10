﻿using System;
using System.Linq;
using System.IO;

using x10.formula;
using x10.utils;
using x10.model;
using x10.model.metadata;
using x10.model.definition;
using x10.gen.react.placeholder;
using x10.gen.react.generate;

namespace x10.gen.react {
  internal class JavaScriptFormulaWriter : IVisitor {

    internal const string CONTEXT_VARIABLE = "appContext";

    private TextWriter _writer;
    private string _variableName;
    private ImportsPlaceholder _imports;

    internal JavaScriptFormulaWriter(TextWriter writer, string variableName, ImportsPlaceholder imports) {
      _writer = writer;
      _variableName = variableName;
      _imports = imports;
    }

    public void VisitBinary(ExpBinary exp) {
      PossiblyWrap(exp, exp.Left);
      _writer.Write(" {0} ", exp.Token);
      PossiblyWrap(exp, exp.Right);
    }

    private void PossiblyWrap(ExpBinary binary, ExpBase expression) {
      if (binary.DataType.IsString && !expression.DataType.IsString)
        // Without converting to string, Flow complains and UI shows "null"
        Wrap(expression, HelperFunctions.X10_ToString);
      else if (binary.IsComparison && !binary.DataType.IsString)
        Wrap(expression, HelperFunctions.ToNum);
      else
        expression.Accept(this);
    }

    private void Wrap(ExpBase expression, Function function) {
      _writer.Write("{0}(", function);
      expression.Accept(this);
      _writer.Write(")");

      _imports.ImportFunction(function);
    }

    public void VisitIdentifier(ExpIdentifier exp) {
      // This is the case for Enum Names
      if (exp.DataType == null)
        return;

      if (exp.IsContext)
        _writer.Write(CONTEXT_VARIABLE);
      else {
        if (exp.DataType.Member is X10DerivedAttribute derivedAttr) { // Derived Attrs become function calls
          string functionName = ReactCodeGenerator.DerivedAttrFuncName(derivedAttr);
          _writer.Write("{0}({1})", functionName, _variableName);
          _imports.ImportDerivedAttributeFunction(derivedAttr);
        } else
          _writer.Write("{0}?.{1}", _variableName, exp.Name);
      }
    }

    public void VisitInvocation(ExpInvocation exp) {
      _writer.Write(ReactCodeGenerator.FunctionName(exp.FunctionName));
      _writer.Write("(");
      foreach (ExpBase argument in exp.Arguments) {
        argument.Accept(this);
        if (argument != exp.Arguments.Last())
          _writer.Write(", ");
      }
      _writer.Write(")");

      _imports.ImportFunction(exp.Function);
    }

    public void VisitLiteral(ExpLiteral exp) {
      if (WriteEnum(exp, exp.Value))
        return;
      _writer.Write(ReactCodeGenerator.TypedLiteralToString(exp.Value, exp.DataType.DataTypeAsEnum, false));
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      // If Expression corresponds to a primitive type (e.g. Date), we are accessing a "member" of
      // the type - e.g. Date.year. This requires special handling
      if (WriteDataTypeMemberAccess(exp))
        return;

      if (WriteEnum(exp, exp.MemberName))
        return;

      if (WriteDerivedAttribute(exp))
        return;

      exp.Expression.Accept(this);
      _writer.Write("?.");
      _writer.Write(exp.MemberName);
    }

    private bool WriteDerivedAttribute(ExpMemberAccess exp) {
      if (exp.DataType.Member is X10DerivedAttribute derivedAttr) {
        string functionName = ReactCodeGenerator.DerivedAttrFuncName(derivedAttr);
        WriteFunctionAroundExpression(exp.Expression, functionName);
        _imports.ImportDerivedAttributeFunction(derivedAttr);
        return true;
      }

      return false;
    }

    private bool WriteDataTypeMemberAccess(ExpMemberAccess exp) {
      if (!exp.Expression.DataType.IsPrimitive)
        return false;

      string functionName = null;
      string importPath = null;

      DataType dataType = exp.Expression.DataType.DataType;
      if (dataType == DataTypes.Singleton.Date) {
        importPath = "react_lib/type_helpers/dateFunctions";
        if (exp.MemberName == "year") {
          functionName = "getYear";
        }
      }

      if (functionName != null) {
        WriteFunctionAroundExpression(exp.Expression, functionName);
        _imports.Import(functionName, importPath, ImportLevel.ThirdParty);
        return true;
      }

      return false;
    }

    private void WriteFunctionAroundExpression(ExpBase expression, string functionName) {
      _writer.Write(functionName);
      _writer.Write("(");
      expression.Accept(this);
      _writer.Write(")");
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

    public void VisitConditional(ExpConditional exp) {
      exp.Conditional.Accept(this);
      _writer.Write(" ? ");
      exp.WhenTrue.Accept(this);
      _writer.Write(" : ");
      exp.WhenFalse.Accept(this);
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
