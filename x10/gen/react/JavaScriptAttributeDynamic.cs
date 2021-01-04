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

    public override object CalculateValue(CodeGenerator genericGenerator, Instance instance, out bool isCodeSnippet) {
      ReactCodeGenerator generator = (ReactCodeGenerator)genericGenerator;
      string variableName = generator.MainVariableName;
      isCodeSnippet = false;

      UiAttributeValueAtomic atomicValue = instance.FindAttributeValue(LogicalName) as UiAttributeValueAtomic;
      if (atomicValue == null) {
        // If the corresponding logical attribute does not exist, there is still the possibility that this is 
        // the main binding attribute (i.e. we will "bind" this attribute to the data model)
        if (IsMainDatabindingAttribute) {
          isCodeSnippet = true;
          if (instance.ModelMember == null)
            return variableName;
          else
            return new CodeSnippetGenerator(WritePrimaryBindingAttribute);
        } else
          return null;
      } else { // logical attribute WAS found...
        if (atomicValue.Expression != null) {
          isCodeSnippet = true;
          return generator.ExpressionToString(atomicValue.Expression, variableName);
        } else
          return GenerateAttributeForValue(atomicValue.Value);
      }
    }

    // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
    private void WritePrimaryBindingAttribute(ReactCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDynamic dataBind = platClassDef.DataBindAttribute;

      if (instance.ModelMember is X10DerivedAttribute derivedAttribute)
        WritePrimaryBindingAttributeForDerived(generator, level, dataBind, derivedAttribute);
      else
        WritePrimaryBindingAttributeForRegular(generator, level, dataBind, instance);
    }

    private void WritePrimaryBindingAttributeForDerived(ReactCodeGenerator generator, int level, PlatformAttributeDynamic dataBind, X10DerivedAttribute derivedAttribute) {
      generator.ImportsPlaceholder.ImportDerivedAttributeFunction(derivedAttribute);
      string functionName = ReactCodeGenerator.DerivedAttrFuncName(derivedAttribute);
      generator.WriteLine(level, "{0}={ {1}({2}) }", dataBind.PlatformName, functionName, generator.MainVariableName);
      generator.WriteLine(level, "onChange={ () => { } }");
    }

    private void WritePrimaryBindingAttributeForRegular(ReactCodeGenerator generator, int level, PlatformAttributeDynamic dataBind, Instance instance) {
      IEnumerable<Member> path = CodeGenUtils.GetBindingPath(instance);

      generator.DestructuringPlaceholder.WriteLine(path.First().Name + ","); // const {...} = topLevelVar;
      string pathExpression = string.Join(".", path.Select(x => x.Name));
      bool isReadOnly = path.Any(x => x.IsReadOnly);

      generator.WriteLine(level, "{0}={ {1} }", dataBind.PlatformName, pathExpression);

      if (isReadOnly) // Special case for read-only: dummy onChane prop
        generator.WriteLine(level, "onChange={ () => { } }");
      else {
        generator.WriteLine(level, "onChange={ (value) => {");
        Member first = path.First();
        string variableName = ReactCodeGenerator.VariableName(first.Owner, false /* TODO */);

        if (path.Count() == 1)
          generator.WriteLine(level + 1, "onChange({ ...{0}, {1}: value })",
            variableName,
            first.Name);
        else {
          generator.WriteLine(level + 1, "let newObj = JSON.parse(JSON.stringify({0}));", variableName);
          generator.WriteLine(level + 1, "newObj.{0} = value;", pathExpression);
          generator.WriteLine(level + 1, "onChange(newObj);");
        }

        generator.WriteLine(level, "} }");
      }
    }
  }
}