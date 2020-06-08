using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Text;

using x10.model.definition;
using x10.parsing;

namespace x10.formula {
  public static class FormulaParser {
    public static ExpressionSyntax Parse(MessageBucket errors, IParseElement element, string formula, Entity contextEntity) {
      CSharpParseOptions options = CSharpParseOptions.Default;
      ExpressionSyntax expression = SyntaxFactory.ParseExpression(formula, 0, options, true);

      ProcessSyntaxErrors(errors, element, expression);
      if (contextEntity != null)
        ProcessDataErrors(errors, element, expression, contextEntity);


      return expression;
    }

    private static void ProcessSyntaxErrors(MessageBucket errors, IParseElement element, ExpressionSyntax expression) {
      foreach (Diagnostic diagnostic in expression.GetDiagnostics()) 
        AddError(errors, element, diagnostic.Location.SourceSpan, diagnostic.GetMessage());
    }

    private static void ProcessDataErrors(MessageBucket errors, IParseElement element, ExpressionSyntax expression, Entity contextEntity) {
      // TODO
    }

    private static void AddError(MessageBucket errors, IParseElement element, TextSpan span, string message) {
      BasicParseElement locationInFormula = new BasicParseElement(element);
      locationInFormula.SetPositionRelativeToStart(span.Start, span.End);
      errors.AddError(locationInFormula, message);
    }
  }
}
