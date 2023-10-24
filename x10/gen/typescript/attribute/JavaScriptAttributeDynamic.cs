using System;
using System.Collections.Generic;
using System.Linq;

using x10.ui.platform;
using x10.ui.composition;
using x10.compiler.ui;
using x10.model.definition;
using x10.gen.typescript.generate;

namespace x10.gen.typescript.attribute {
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
      TypeScriptCodeGenerator generator = (TypeScriptCodeGenerator)genericGenerator;
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
    private void WritePrimaryBindingAttribute(TypeScriptCodeGenerator generator, int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDynamic dataBind = platClassDef.DataBindAttribute;

      if (UiCompilerUtils.IsReadOnly(instance)) {
        string expressionString = generator.GetReadOnlyBindingPath(instance);
        generator.WriteLine(level, "{0}={ {1} } ", dataBind.PlatformName, expressionString);
      } else {
        string pathExpression = generator.GetBindingPath(instance);

        // For non-owned association, we represent the data as as an object containing the single "id" property
        IEnumerable<Member> bindingPath = UiCompilerUtils.GetBindingPath(instance);
        Member lastMember = bindingPath.Last();
        bool isNonOwnedAssociation = lastMember.IsNonOwnedAssociation;
        bool isMany = lastMember.IsManyAssociation;
        string bindingValueExpression = isNonOwnedAssociation ? "value == null ? undefined : { id: value }" : "value";

        generator.WriteLine(level, "{0}={ {1}{2}{3} }",
          dataBind.PlatformName, 
          pathExpression,
          isNonOwnedAssociation ? "?.id" : "",
          isMany ? " || []" : "");
        generator.WriteLine(level, "onChange={ (value) => {");

        if (bindingPath.Count() == 1) {
          generator.WriteLine(level + 1, "onChange({ ...{0}, {1}: {2} })",
            generator.SourceVariableName,
            bindingPath.Single().Name,
            bindingValueExpression);
        } else {
          generator.WriteLine(level + 1, "let newObj = JSON.parse(JSON.stringify({0}));", generator.SourceVariableName);

          string pathAfterRoot = string.Join('.', pathExpression.Split('.').Skip(1));
          pathAfterRoot = pathAfterRoot.Replace("?", "");   // Cannot assign to maybe-null
          generator.WriteLine(level + 1, "newObj.{0} = {1};", pathAfterRoot, bindingValueExpression);

          generator.WriteLine(level + 1, "onChange(newObj);");
        }

        generator.WriteLine(level, "} }");
      }
    }
  }
}