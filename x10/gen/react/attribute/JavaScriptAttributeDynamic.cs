using System;
using System.Collections.Generic;
using System.Linq;

using x10.ui.platform;
using x10.ui.composition;
using x10.compiler.ui;
using x10.model.definition;
using x10.gen.react.generate;

namespace x10.gen.react.attribute {
  // Use this type of attribute to convert attributes from the logical representation
  // into the platform-specific representation.
  // The following situations are taken care of by this attribute type:
  // 1. Default binding attribute
  // 2. Pass-through with no modification (both complex and atomic). Complex Attribute
  //    will be rendered as a JavaScript list or objects
  // 3. Translation of logical formula into platform-specific formula
  public class JavaScriptAttributeDynamic : PlatformAttributeDynamic {

    public JavaScriptAttributeDynamic() { }

    public JavaScriptAttributeDynamic(string logicalName, string platformName) : base(logicalName, platformName) {
      // Do nothing
    }

    public override object CalculateValue(CodeGenerator genericGenerator, Instance instance, out bool isCodeSnippet) {
      ReactCodeGenerator generator = (ReactCodeGenerator)genericGenerator;
      isCodeSnippet = IsCodeSnippet;

      UiAttributeValue value = LogicalAttribute == null ?
        null :
        instance.FindAttributeValueRespectInheritable(LogicalAttribute);

      if (value == null) {
        // If the corresponding logical attribute does not exist, there is still the possibility that this is 
        // the main binding attribute (i.e. we will "bind" this attribute to the data model)
        if (IsMainDatabindingAttribute) {
          isCodeSnippet = true;
          if (instance.ModelMember == null || generator.AlreadyScopedToMember)
            return generator.SourceVariableName;
          else
            return new CodeSnippetGenerator(WritePrimaryBindingAttribute);
        } else
          return null;
      } else if (value is UiAttributeValueComplex) {
        return value;
      } else if (value is UiAttributeValueAtomic atomicValue) {
        if (atomicValue.Expression != null) {
          isCodeSnippet = true;
          generator.PushSourceVariableName(generator.GetBindingPath(instance));
          string expression = generator.ExpressionToString(atomicValue.Expression);
          generator.PopSourceVariableName();
          return expression;
        } else
          return GenerateAttributeForValue(atomicValue.Value);
      } else
        throw new NotImplementedException("Neither atomic nor complex");
    }

    // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
    private void WritePrimaryBindingAttribute(ReactCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDynamic dataBind = platClassDef.DataBindAttribute;

      if (UiCompilerUtils.IsReadOnly(instance)) {
        string expressionString = generator.GetReadOnlyBindingPath(instance);
        generator.WriteLine(level, "{0}={ {1} }", dataBind.PlatformName, expressionString);
      } else {
        string pathExpression = generator.GetBindingPath(instance);

        // For non-owned association, we represent the data as as an object containing the single "id" property
        IEnumerable<Member> path = UiCompilerUtils.GetBindingPath(instance);
        bool isNonOwnedAssociation = path.Last().IsNonOwnedAssociation;
        string bindingValueExpression = isNonOwnedAssociation ? "value == null ? null : { id: value }" : "value";

        generator.WriteLine(level, "{0}={ {1}{2} }",
          dataBind.PlatformName, 
          pathExpression,
          isNonOwnedAssociation ? "?.id" : "");
        generator.WriteLine(level, "onChange={ (value) => {");

        if (path.Count() == 1) {
          generator.WriteLine(level + 1, "// $FlowExpectedError");
          generator.WriteLine(level + 1, "onChange({ ...{0}, {1}: {2} })",
            generator.SourceVariableName,
            path.Single().Name,
            bindingValueExpression);
        } else {
          generator.WriteLine(level + 1, "let newObj = JSON.parse(JSON.stringify({0}));", generator.SourceVariableName);
          pathExpression = pathExpression.Replace("?", "");   // Cannot assign to maybe-null
          generator.WriteLine(level + 1, "newObj.{0} = {1};", pathExpression, bindingValueExpression);
          generator.WriteLine(level + 1, "onChange(newObj);");
        }

        generator.WriteLine(level, "} }");
      }
    }
  }
}