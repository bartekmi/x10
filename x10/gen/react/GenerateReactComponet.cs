using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.compiler;
using x10.formula;
using x10.model.definition;
using x10.ui.composition;
using x10.utils;
using x10.ui.platform;

namespace x10.gen.react {
  public partial class ReactCodeGenerator {

    private readonly string[] IGNORE_ATTRIBUTES = new string[] {
      UiAttributeDefinitions.NAME,
      UiAttributeDefinitions.PATH,
    };

    private OutputPlaceholder _importsPlaceholder;
    private OutputPlaceholder _destructuringPlaceholder;

    public override void Generate(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, ".jsx");

      GenerateFileHeader();

      bool isForm = true; // TODO
      Entity model = classDef.ComponentDataModel;

      GenerateImports(model, isForm);
      GenerateComponent(classDef);

      End();
    }

    private void GenerateImports(Entity model, bool isForm) {
      WriteLine(0, "import * as React from 'react';");
      if (isForm)
        WriteLine(0, "import { graphql, commitMutation } from 'react-relay';");
      WriteLine();

      _importsPlaceholder = CreatePlaceholder(0);
      WriteLine();

      if (model != null) {
        WriteLine(0, "import { type {0} } from '{1}';", model.Name, ImportPath(model));
        WriteLine();
      }
    }

    private void GenerateComponent(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      string typeName = model?.Name;
      string variableName = VariableName(model);

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        WriteLine(1, "+{0}: {1},", variableName, typeName);
        WriteLine(1, "+onChange: ({0}: {1}) => void,", variableName, typeName);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "export default function {0}(props: Props): React.Node {", classDef.Name);
      if (model != null) {
        WriteLine(1, "const { {0}, onChange } = props;", variableName);
        WriteLine(1, "const {");
        _destructuringPlaceholder = CreatePlaceholder(2);
        WriteLine(1, "} = {0};", variableName);
      }
      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();
    }

    #region GenerateComponentRecursively
    private void GenerateComponentRecursively(int level, Instance instance) {
      if (instance == null)
        return;

      // Find the Platform Class Definition
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null)
        return;

      _importsPlaceholder.WriteLine("import {0} from '{1}';", platClassDef.PlatformName, platClassDef.ImportPath);

      // Open the React tag
      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName);

      WriteStaticAttributes(level, platClassDef);
      WriteByFuncAttributes(level, platClassDef, instance);
      bool dataBindAlreadyRendered = WriteLogicalAttributes(level, platClassDef, instance);
      if (!dataBindAlreadyRendered)
        WritePrimaryBindingAttribute(level, platClassDef, instance);
      WritePrimaryAttributeAsProperty(level, platClassDef, instance);

      WriteNestedContentsAndClosingTag(level, platClassDef, instance);
    }

    private void WriteStaticAttributes(int level, PlatformClassDef platClassDef) {
      foreach (PlatformAttributeStatic staticAttr in platClassDef.StaticPlatformAttributes) {
        string value = staticAttr.Value.Trim();
        if (!(value.StartsWith("{") && value.EndsWith("}"))) // Do not add quotes to {bracked}
          value = string.Format("'{0}'", value);
        WriteLine(level + 1, "{0}={1}", staticAttr.PlatformName, value);
      }
    }

    private void WritePrimaryAttributeAsProperty(int level, PlatformClassDef platClassDef, Instance instance) {
      string propName = platClassDef.PrimaryAttributeWrapperProperty;
      if (propName == null)
        return;

      UiAttributeValue primaryValue = instance.PrimaryValue;

      WriteLine(level + 1, "{0}={ [", propName);

      if (primaryValue is UiAttributeValueComplex complexAttr) {
        foreach (Instance childInstance in complexAttr.Instances) 
          RenderInstanceAsJavaScript(level + 2, childInstance);
      } else
        throw new NotImplementedException();

      WriteLine(level + 1, "] }", propName);
    }

    private void RenderInstanceAsJavaScript(int level, Instance instance) {
      WriteLine(level, "{");
      foreach (UiAttributeValue value in instance.AttributeValues) {
        if (IGNORE_ATTRIBUTES.Contains(value.Definition.Name))
          continue;
          
        if (value is UiAttributeValueAtomic atomicValue)
          WriteLine(level + 1, "{0}: {1},", value.Definition.Name, TypedLiteralToString(atomicValue.Value, null));
        else if (value is UiAttributeValueComplex complexValue) {
          WriteLine(level + 1, "{0}: ", value.Definition.Name);
          // TODO: Obviously, an assumption here!
          // TEMP code
          foreach (Instance childInstance in complexValue.Instances) 
            RenderInstanceAsJavaScript(level + 2, childInstance);

          //RenderInstanceAsJavaScript(level + 1, complexValue.Instances.Single());
        }
      }
      WriteLine(level, "},");
    }

    private void WriteByFuncAttributes(int level, PlatformClassDef platClassDef, Instance instance) {
      foreach (PlatformAttributeByFunc byFuncAttr in platClassDef.ByFuncPlatformAttributes) {
        string value = byFuncAttr.Function(instance);
        if (value != null)
          WriteLine(level + 1, "{0}=\"{1}\"", byFuncAttr.PlatformName, value);
      }
    }

    private bool WriteLogicalAttributes(int level, PlatformClassDef platClassDef, Instance instance) {
      UiAttributeValue primaryValue = instance.PrimaryValue;
      PlatformAttributeDataBind dataBind = platClassDef.DataBindAttribute;
      bool dataBindAlreadyRendered = false;

      foreach (UiAttributeValue attrValue in instance.AttributeValues) {
        string attrName = attrValue.Definition.Name;

        if (attrValue == primaryValue || IGNORE_ATTRIBUTES.Contains(attrValue.Definition.Name))
          continue;

        PlatformAttributeDynamic dynamicAttr = platClassDef.FindDyamicAttribute(attrName);
        if (dynamicAttr == dataBind)
          dataBindAlreadyRendered = true;

        // Write the attribute
        if (attrValue is UiAttributeValueAtomic atomicValue) {
          // TODO: I believe this allows for direct translation of logical attributes without them being defined in
          // the platform lib. Consider removing this "feature".
          string name = dynamicAttr == null ? attrName : dynamicAttr.PlatformName;
          if (atomicValue.Expression != null) {
            string formula = GenerateFormula(instance, dynamicAttr, name, atomicValue.Expression);
            WriteLine(level + 1, "{0}={ {1} }", name, formula);
          } else if (atomicValue.Value != null && dynamicAttr != null) {
            object value = dynamicAttr.GenerateAttributeForValue(atomicValue.Value);
            WriteLine(level + 1, "{0}={ {1} }", name, TypedLiteralToString(value, null));
          }
        } else {
          // TODO - e.g. Action from submit button
        }
      }

      return dataBindAlreadyRendered;
    }

    // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
    private void WritePrimaryBindingAttribute(int level, PlatformClassDef platClassDef, Instance instance) {
      PlatformAttributeDataBind dataBind = platClassDef.DataBindAttribute;
      Member member = instance.ModelMember;

      if (dataBind != null && member != null) {
        _destructuringPlaceholder.WriteLine(member.Name + ",");
        WriteLine(level + 1, "{0}={ {1} }", dataBind.PlatformName, member.Name);
        if (member.IsReadOnly)
          WriteLine(level + 1, "onChange={ () => { } }");
        else {
          WriteLine(level + 1, "onChange={ (value) => {");
          WriteLine(level + 2, "onChange({ ...{0}, {1}: value })",
            VariableName(member.Owner),
            member.Name);
          WriteLine(level + 1, "} }");
        }
      }
    }

    private void WriteNestedContentsAndClosingTag(int level, PlatformClassDef platClassDef, Instance instance) {
      UiAttributeValue primaryValue = platClassDef.PrimaryAttributeWrapperProperty == null ?
        instance.PrimaryValue : null;

      // Close the XAML element, possibly recursively writing nested content
      if (primaryValue == null)
        WriteLineClose(level, "/>");
      else {
        WriteLineClose(level, ">");

        if (primaryValue is UiAttributeValueComplex complexAttr) {
          foreach (Instance childInstance in complexAttr.Instances)
            GenerateComponentRecursively(level, childInstance);
        } else
          throw new NotImplementedException();

        WriteLine(level, "</{0}>", platClassDef.EffectivePlatformName);
      }
    }

    #endregion

    #region Utilities
    private string GenerateFormula(Instance context, PlatformAttributeDynamic dynamicAttr, string name, ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      JavascriptFormulaWriter formulaWriterVisitor = new JavascriptFormulaWriter(writer);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

    private string VariableName(Entity model) {
      if (model == null)
        return null;
      return NameUtils.UncapitalizeFirstLetter(model.Name);
    }
    #endregion
  }
}