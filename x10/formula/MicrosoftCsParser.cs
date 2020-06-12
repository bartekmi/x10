using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;
using x10.parsing;

namespace x10.formula {
  public static class MicrosoftCsParser {
    public static ExpBase Parse(MessageBucket errors, IParseElement element, string formula) {
      CSharpParseOptions options = CSharpParseOptions.Default;
      ExpressionSyntax expression = SyntaxFactory.ParseExpression(formula, 0, options, true);

      foreach (Diagnostic diagnostic in expression.GetDiagnostics())
        errors.Messages.Add(CreateMessage(element, diagnostic));

      return ConvertToExpBase(element, expression);
    }

    private static ExpBase ConvertToExpBase(IParseElement element, ExpressionSyntax expression) {
      ExpBase x10Expression;
      TextSpan? span = null;

      if (expression is BinaryExpressionSyntax binary) {
        x10Expression = new ExpBinary() {
          Token = binary.OperatorToken.ToString(),
          Left = ConvertToExpBase(element, binary.Left),
          Right = ConvertToExpBase(element, binary.Right),
        };
        span = binary.OperatorToken.Span;
      } else if (expression is LiteralExpressionSyntax literal) {
        x10Expression = new ExpLiteral() {
          Value = literal.Token.Value,
        };
      } else if (expression is ParenthesizedExpressionSyntax parenth) {
        x10Expression = new ExpParenthesized() {
          Expression = ConvertToExpBase(element, parenth.Expression),
        };
      } else if (expression is IdentifierNameSyntax identifier) {
        x10Expression = new ExpIdentifier() {
          Name = identifier.Identifier.ToString(),
        };
      } else if (expression is MemberAccessExpressionSyntax member) {
        x10Expression = new ExpMemberAccess() {
          Expression = ConvertToExpBase(element, member.Expression),
          MemberName = member.Name.ToString(),
        };
        span = member.OperatorToken.Span;
      } else if (expression is InvocationExpressionSyntax invoke) {
        x10Expression = new ExpInvocation() {
          FunctionName = ((IdentifierNameSyntax)invoke.Expression).ToString(),
          Arguments = new List<ExpBase>(),
        };
        foreach (ArgumentSyntax arg in invoke.ArgumentList.Arguments)
          ((ExpInvocation)x10Expression).Arguments.Add(ConvertToExpBase(element, arg.Expression));
      } else {
        x10Expression = new ExpUnknown() {
          DiagnosticMessage = "Unexpected Node Type: " + expression.GetType().Name,
        };
      }

      SetFilePosition(element, x10Expression, span ?? expression.GetLocation().SourceSpan);

      return x10Expression;
    }

    private static void SetFilePosition(IParseElement source, ExpBase x10Expression, TextSpan textSpan) {
      x10Expression.SetRelativeTo(source, textSpan.Start, textSpan.End);
    }

    private static CompileMessage CreateMessage(IParseElement element, Diagnostic diagnostic) {
      ExpBase dummyExpression = new ExpUnknown();
      SetFilePosition(element, dummyExpression, diagnostic.Location.SourceSpan);

      return new CompileMessage() {
        ParseElement = dummyExpression,
        Message = diagnostic.GetMessage(),
        Severity = CompileMessageSeverity.Error,
      };
    }
  }
}
