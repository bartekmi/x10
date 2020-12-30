using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using x10.formula;
using x10.ui.platform;
using x10.ui.composition;
using x10.model.definition;

namespace x10.gen.react {
  public class JavaScriptAttributeDynamic : PlatformAttributeDynamic {

    public JavaScriptAttributeDynamic() { }

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
      IEnumerable<Member> path = CodeGenUtils.GetBindingPath(instance);

      generator.DestructuringPlaceholder.WriteLine(path.First().Name + ","); // const {...} = topLevelVar;
      string pathExpression = string.Join(".", path.Select(x => x.Name));
      bool isReadOnly = path.Any(x => x.IsReadOnly);

      generator.WriteLine(level + 1, "{0}={ {1} }", dataBind.PlatformName, pathExpression);

      if (isReadOnly) // Special case for read-only: dummy onChane prop
        generator.WriteLine(level + 1, "onChange={ () => { } }");
      else {
        generator.WriteLine(level + 1, "onChange={ (value) => {");
        Member first = path.First();
        string variableName = generator.VariableName(first.Owner, false /* TODO */);
        
        if (path.Count() == 1) 
          generator.WriteLine(level + 2, "onChange({ ...{0}, {1}: value })",
            variableName,
            first.Name);
        else {
            generator.WriteLine(level + 2, "let newObj = JSON.parse(JSON.stringify({0}));", variableName);
            generator.WriteLine(level + 2, "newObj.{0} = value;", pathExpression);
            generator.WriteLine(level + 2, "onChange(newObj);");
        }

        generator.WriteLine(level + 1, "} }");
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