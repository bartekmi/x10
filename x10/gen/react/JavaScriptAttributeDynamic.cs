using System.IO;
using System;

using x10.formula;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.definition;

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
          if (instance.ModelMember == null)
            return reactGenerator.MainVariableName;
          else 
            return new CodeSnippetGenerator(WritePrimaryBindingAttribute);
        } else
          return null;
      }

      if (atomicValue.Expression != null) {
        isCodeSnippet = true;
        return GenerateFormula(instance, atomicValue.Expression);
      } else 
        return GenerateAttributeForValue(atomicValue.Value);
    }

    // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
    private void WritePrimaryBindingAttribute(ReactCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDynamic dataBind = platClassDef.DataBindAttribute;
      Member member = instance.ModelMember;

      if (dataBind != null && member != null) {
        generator.DestructuringPlaceholder.WriteLine(member.Name + ",");
        generator.WriteLine(level + 1, "{0}={ {1} }", dataBind.PlatformName, member.Name);
        if (member.IsReadOnly)
          generator.WriteLine(level + 1, "onChange={ () => { } }");
        else {
          generator.WriteLine(level + 1, "onChange={ (value) => {");
          generator.WriteLine(level + 2, "onChange({ ...{0}, {1}: value })",
            generator.VariableName(member.Owner, false /* TODO */),
            member.Name);
          generator.WriteLine(level + 1, "} }");
        }
      }
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