using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using x10.parsing;

namespace x10.formula {
  public static class MicrosoftCsParser {
    public static ExpBase Parse(FormulaParser parser, IParseElement element, string formula) {
      CSharpParseOptions options = CSharpParseOptions.Default;
      ExpressionSyntax expression = SyntaxFactory.ParseExpression(formula, 0, options, true);

      foreach (Diagnostic diagnostic in expression.GetDiagnostics())
        AddError(parser, element, diagnostic.GetMessage(), diagnostic.Location.SourceSpan);

      return ConvertToExpBase(parser, element, expression);
    }

    private static ExpBase ConvertToExpBase(FormulaParser parser, IParseElement element, ExpressionSyntax expression) {
      ExpBase x10Expression;
      TextSpan? span = null;

      if (expression is BinaryExpressionSyntax binary) {
        x10Expression = new ExpBinary(parser) {
          Token = binary.OperatorToken.ToString(),
          Left = ConvertToExpBase(parser, element, binary.Left),
          Right = ConvertToExpBase(parser, element, binary.Right),
        };
        span = binary.OperatorToken.Span;
      } else if (expression is LiteralExpressionSyntax literal) {
        x10Expression = new ExpLiteral(parser) {
          Value = literal.Token.Value,
        };
      } else if (expression is PrefixUnaryExpressionSyntax unary) {
        x10Expression = new ExpUnary(parser) {
          Token = unary.OperatorToken.ToString(),
          Expression = ConvertToExpBase(parser, element, unary.Operand),
        };
      } else if (expression is ConditionalExpressionSyntax conditional) {
        x10Expression = new ExpConditional(parser) {
          Conditional = ConvertToExpBase(parser, element, conditional.Condition),
          WhenTrue = ConvertToExpBase(parser, element, conditional.WhenTrue),
          WhenFalse = ConvertToExpBase(parser, element, conditional.WhenFalse),
        };
      } else if (expression is ParenthesizedExpressionSyntax parenth) {
        x10Expression = new ExpParenthesized(parser) {
          Expression = ConvertToExpBase(parser, element, parenth.Expression),
        };
      } else if (expression is IdentifierNameSyntax identifier) {
        x10Expression = new ExpIdentifier(parser) {
          Name = identifier.Identifier.ToString(),
        };
      } else if (expression is MemberAccessExpressionSyntax member) {
        x10Expression = new ExpMemberAccess(parser) {
          Expression = ConvertToExpBase(parser, element, member.Expression),
          MemberName = member.Name.ToString(),
        };
        span = member.Name.Identifier.Span;
      } else if (expression is InvocationExpressionSyntax invoke) {
        x10Expression = new ExpInvocation(parser) {
          FunctionName = ((IdentifierNameSyntax)invoke.Expression).ToString(),
          Arguments = new List<ExpBase>(),
        };
        foreach (ArgumentSyntax arg in invoke.ArgumentList.Arguments)
          ((ExpInvocation)x10Expression).Arguments.Add(ConvertToExpBase(parser, element, arg.Expression));
      } else {
        x10Expression = new ExpUnknown(parser) {
          DiagnosticMessage = "Unexpected Node Type: " + expression.GetType().Name,
        };
      }

      if (element != null)
        SetFilePosition(element, x10Expression, span ?? expression.GetLocation().SourceSpan);

      return x10Expression;
    }

    #region Utils

    private static void SetFilePosition(IParseElement source, ExpBase x10Expression, TextSpan textSpan) {
      x10Expression.SetRelativeTo(source, textSpan.Start, textSpan.End);
    }

    internal static void AddError(FormulaParser parser, IParseElement element, string message, TextSpan span) {
      ExpBase dummyExpression = new ExpUnknown(parser);
      SetFilePosition(element, dummyExpression, span);

      CompileMessage compMessage = new CompileMessage() {
        ParseElement = dummyExpression,
        Message = message,
        Severity = CompileMessageSeverity.Error,
      };

      parser.Errors.Add(compMessage);
    }
    #endregion
  }
}
