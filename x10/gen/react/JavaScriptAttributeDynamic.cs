using System.IO;
using System;

using x10.formula;
using x10.ui.platform;
using x10.ui.composition;
using x10.gen;

namespace x10.gen.react {
  public class JavaScriptAttributeDynamic : PlatformAttributeDynamic {

    public JavaScriptAttributeDynamic() {}

    public JavaScriptAttributeDynamic(string logicalName, string platformName) : base(logicalName, platformName) {
      // Do nothing
    }

    public override object CalculateValue(CodeGenerator generator, Instance instance, out bool isCodeSnippet) {
      ReactCodeGenerator reactGenerator = (ReactCodeGenerator)generator;
      isCodeSnippet = false;

      UiAttributeValueAtomic atomicValue = instance.FindAttributeValue(LogicalName) as UiAttributeValueAtomic;
      if (atomicValue == null) {
        if (IsMainDatabindingAttribute) {
          isCodeSnippet = true;
          return reactGenerator.MainVariableName;
        } else
          return null;
      }

      if (atomicValue.Expression != null) {
        isCodeSnippet = true;
        return GenerateFormula(instance, atomicValue.Expression);
      } else 
        return GenerateAttributeForValue(atomicValue.Value);
    }

    private string GenerateFormula(Instance context, ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      JavascriptFormulaWriter formulaWriterVisitor = new JavascriptFormulaWriter(writer);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }
  }
}