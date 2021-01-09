using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using x10.formula;
using x10.ui.platform;
using x10.ui.composition;
using x10.ui.metadata;
using x10.model.definition;

namespace x10.gen.react {
  public class JavaScriptAttributeDynamic : PlatformAttributeDynamic {

    public JavaScriptAttributeDynamic() { }

    public JavaScriptAttributeDynamic(string logicalName, string platformName) : base(logicalName, platformName) {
      // Do nothing
    }

    public override object CalculateValue(CodeGenerator genericGenerator, Instance instance, out bool isCodeSnippet) {
      ReactCodeGenerator generator = (ReactCodeGenerator)genericGenerator;
      isCodeSnippet = false;

      UiAttributeValueAtomic atomicValue = LogicalAttribute == null ?
        null :
        instance.FindAttributeValueRespectInheritable((UiAttributeDefinitionAtomic)LogicalAttribute);
        
      if (atomicValue == null) {
        // If the corresponding logical attribute does not exist, there is still the possibility that this is 
        // the main binding attribute (i.e. we will "bind" this attribute to the data model)
        if (IsMainDatabindingAttribute) {
          isCodeSnippet = true;
          if (instance.ModelMember == null)
            return generator.SourceVariableName;
          else
            return new CodeSnippetGenerator(WritePrimaryBindingAttribute);
        } else
          return null;
      } else { // logical attribute WAS found...
        if (atomicValue.Expression != null) {
          isCodeSnippet = true;
          return generator.ExpressionToString(atomicValue.Expression);
        } else
          return GenerateAttributeForValue(atomicValue.Value);
      }
    }

    // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
    private void WritePrimaryBindingAttribute(ReactCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDynamic dataBind = platClassDef.DataBindAttribute;
      IEnumerable<Member> path = CodeGenUtils.GetBindingPath(instance);

      if (CodeGenUtils.IsReadOnly(instance)) {
        ExpBase expression = CodeGenUtils.PathToExpression(path);
        string expressionString = generator.ExpressionToString(expression);
        generator.WriteLine(level, "{0}={ {1} }", dataBind.PlatformName, expressionString);
        generator.WriteLine(level, "onChange={ () => { } }"); // Special case for read-only: dummy onChane prop
      } else {
        string pathExpression = string.Join(".", path.Select(x => x.Name));
        generator.WriteLine(level, "{0}={ {1}.{2} }", dataBind.PlatformName, generator.SourceVariableName, pathExpression);
        generator.WriteLine(level, "onChange={ (value) => {");
        Member first = path.First();

        if (path.Count() == 1)
          generator.WriteLine(level + 1, "onChange({ ...{0}, {1}: value })",
            generator.SourceVariableName,
            first.Name);
        else {
          generator.WriteLine(level + 1, "let newObj = JSON.parse(JSON.stringify({0}));", generator.SourceVariableName);
          generator.WriteLine(level + 1, "newObj.{0} = value;", pathExpression);
          generator.WriteLine(level + 1, "onChange(newObj);");
        }

        generator.WriteLine(level, "} }");
      }
    }
  }
}