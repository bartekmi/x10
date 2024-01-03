using System;
using System.Linq;
using System.IO;

using x10.formula;
using x10.model;
using x10.model.metadata;
using x10.model.definition;
using x10.gen.typescript.placeholder;
using x10.gen.typescript.generate;
using x10.utils;

namespace x10.gen.typescript {
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
      Write(" {0} ", exp.Token);
      PossiblyWrap(exp, exp.Right);
    }

    private void PossiblyWrap(ExpBinary binary, ExpBase expression) {
      // This is hokey. Correct way would be to convert sequences of string concatenation additions
      // to proper JavaScript string interpolation... `Foo ${myVar} Bar`, but this is harder to do.
      bool dontWrapWithToString = expression is ExpLiteral || expression is ExpBinary && expression.DataType.IsString;
      if (binary.DataType.IsString && !dontWrapWithToString)
        Wrap(expression, HelperFunctions.X10_ToString);
      else if (binary.IsComparison && !binary.DataType.IsString)
        Wrap(expression, HelperFunctions.ToNum);
      else if (binary.IsEqualityOrInequality && 
               expression.DataType.IsEnum && !(expression is ExpLiteral))
        Wrap(expression, HelperFunctions.ToEnum);
      else
        expression.Accept(this);
    }

    private void Wrap(ExpBase expression, Function function) {
      Write("{0}(", function);
      expression.Accept(this);
      Write(")");

      _imports.ImportFunction(function);
    }

    public void VisitIdentifier(ExpIdentifier exp) {
      // This is the case for Enum Names
      if (exp.DataType == null)
        return;

      if (exp.IsContext)
        Write(CONTEXT_VARIABLE);
      else {
        if (exp.DataType.Member is X10DerivedAttribute derivedAttr) { // Derived Attrs become function calls
          string functionName = TypeScriptCodeGenerator.DerivedAttrFuncName(derivedAttr);
          Write("{0}({1}, {2})", functionName, CONTEXT_VARIABLE, _variableName);
          _imports.ImportDerivedAttributeFunction(derivedAttr);
        } else {
          string content = _variableName == null ?
            exp.Name :
            string.Format("{0}?.{1}", _variableName, exp.Name);
          Write(content);
        }
      }
    }

    public void VisitInvocation(ExpInvocation exp) {
      Write(TypeScriptCodeGenerator.FunctionName(exp.FunctionName));
      Write("(");
      foreach (ExpBase argument in exp.Arguments) {
        argument.Accept(this);
        if (argument != exp.Arguments.Last())
          Write(", ");
      }
      Write(")");

      _imports.ImportFunction(exp.Function);
    }

    public void VisitLiteral(ExpLiteral exp) {
      if (WriteEnum(exp, exp.Value))
        return;
      Write(TypeScriptCodeGenerator.TypedLiteralToString(exp.Value, exp.DataType.DataTypeAsEnum, false));
    }

    public void VisitMemberAccess(ExpMemberAccess exp) {
      if (WriteEnum(exp, exp.MemberName))
        return;

      // If Expression corresponds to a primitive type (e.g. Date), we are accessing a "member" of
      // the type - e.g. Date.year. This requires special handling
      if (WriteDataTypeMemberAccess(exp))
        return;

      if (WriteDerivedAttribute(exp))
        return;

      if (WriteArrayIntrinsicMember(exp))
        return;

      exp.Expression.Accept(this);
      Write("?.");
      Write(exp.MemberName);
    }

    private bool WriteArrayIntrinsicMember(ExpMemberAccess exp) {
      if (exp.Expression.DataType.IsMany) {
        switch (exp.MemberName) {
          case "count":
            exp.Expression.Accept(this);
            Write(".length");
            break;
          default:
            throw new NotImplementedException("Unimplemented array member access: " + exp.MemberName);
        }

        return true;
      }

      return false;
    }

    private bool WriteDerivedAttribute(ExpMemberAccess exp) {
      if (exp.DataType.Member is X10DerivedAttribute derivedAttr) {
        string functionName = TypeScriptCodeGenerator.DerivedAttrFuncName(derivedAttr);
        WriteFunctionAroundExpression(exp.Expression, functionName, true);
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
      if (dataType == DataTypes.Singleton.Date || dataType == DataTypes.Singleton.Timestamp) {
        importPath = "type_helpers/dateFunctions";
        if (exp.MemberName == "year") {
           functionName = "getYear";
        } else if (exp.MemberName == "date") {
          functionName = "getDate";
        }
      }

      if (functionName != null) {
        WriteFunctionAroundExpression(exp.Expression, functionName, false); // These helper func's don't need context
        _imports.ImportFromReactLib(functionName, importPath);
        return true;
      }

      return false;
    }

    private void WriteFunctionAroundExpression(ExpBase expression, string functionName, bool includeContext) {
      Write(functionName);

      if (includeContext)
        Write("({0}, ", CONTEXT_VARIABLE);
      else
        Write("(");

      expression.Accept(this);
      Write(")");
    }

    public void VisitParenthesized(ExpParenthesized exp) {
      Write("(");
      exp.Expression.Accept(this);
      Write(")");
    }

    public void VisitUnary(ExpUnary exp) {
      Write(exp.Token);
      exp.Expression.Accept(this);
    }

    public void VisitConditional(ExpConditional exp) {
      exp.Conditional.Accept(this);
      Write(" ? ");
      exp.WhenTrue.Accept(this);
      Write(" : ");
      exp.WhenFalse.Accept(this);
    }

    public void VisitStringInterpolation(ExpStringInterpolation exp) {
      _writer.Write('`');

      foreach (ExpStringInterpolation.StringOrExpression chunk in exp.Chunks)
        if (chunk.Expression == null)
          _writer.Write(chunk.String);
        else {
          _writer.Write("${");
          chunk.Expression.Accept(this);
          _writer.Write("}");
        }

      _writer.Write('`');
    }

    public void VisitUnknown(ExpUnknown exp) {
      // This should never happen if the x10 code was compiled cleanly
    }

    private bool WriteEnum(ExpBase expression, object nameOrNull) {
      if (expression.IsEnumLiteral) {
        string text = nameOrNull == null ? 
          "null" : 
          String.Format("\"{0}\"", TypeScriptCodeGenerator.ToEnumValueString(nameOrNull));

        Write(text);
        return true;
      }

      return false;
    }

    // If turned on, this will annotate every single piece of text
    // with the call site. This is a debugging help so we can trace the 
    // generated code to where we generated it.
    private void Write(string pattern, params object[] args) {
      string text = string.Format(pattern, args);
      
      if (ProgramStatics.AnnotateFormulas)
        text = DebugUtils.StampWithCallerSource(text, 1, true);
        
      _writer.Write(text, 1, true);   // Just skip this method
    }

  }
}
