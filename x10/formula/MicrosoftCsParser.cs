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
    public static ExpBase Parse(FormulaParser parser, IParseElement element, string formula) {
      CSharpParseOptions options = CSharpParseOptions.Default;
      ExpressionSyntax expression = SyntaxFactory.ParseExpression(formula, 0, options, true);

      foreach (Diagnostic diagnostic in expression.GetDiagnostics())
        parser.Errors.Messages.Add(CreateMessage(parser, element, diagnostic));

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

      SetFilePosition(element, x10Expression, span ?? expression.GetLocation().SourceSpan);

      return x10Expression;
    }

    private static void SetFilePosition(IParseElement source, ExpBase x10Expression, TextSpan textSpan) {
      x10Expression.SetRelativeTo(source, textSpan.Start, textSpan.End);
    }

    private static CompileMessage CreateMessage(FormulaParser parser, IParseElement element, Diagnostic diagnostic) {
      ExpBase dummyExpression = new ExpUnknown(parser);
      SetFilePosition(element, dummyExpression, diagnostic.Location.SourceSpan);

      return new CompileMessage() {
        ParseElement = dummyExpression,
        Message = diagnostic.GetMessage(),
        Severity = CompileMessageSeverity.Error,
      };
    }
  }
}
