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

    enum OutputType {
      JSON,
      React,
    }

    internal string MainVariableName;
    internal OutputPlaceholder ImportsPlaceholder;
    internal OutputPlaceholder DestructuringPlaceholder;

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

      ImportsPlaceholder = CreatePlaceholder(0);
      WriteLine();

      if (model != null) {
        WriteLine(0, "import { type {0} } from '{1}';", model.Name, ImportPath(model));
        WriteLine();
      }
    }

    private void GenerateComponent(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      MainVariableName = VariableName(model, classDef.IsMany);

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        string typeName = model.Name;
        if (classDef.IsMany)
          typeName = string.Format("$ReadOnlyArray<{0}>", typeName);

        WriteLine(1, "+{0}: {1},", MainVariableName, typeName);
        WriteLine(1, "+onChange: ({0}: {1}) => void,", MainVariableName, typeName);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "export default function {0}(props: Props): React.Node {", classDef.Name);
      if (model != null) {
        WriteLine(1, "const { {0}, onChange } = props;", MainVariableName);
        WriteLine(1, "const {");
        DestructuringPlaceholder = CreatePlaceholder(2);
        WriteLine(1, "} = {0};", MainVariableName);
      }
      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(OutputType.React, 2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();
    }

    #region GenerateJavascriptObjectRecursively
    private void RenderComplexAttrAsJavascript(int level, UiAttributeValueComplex complex) {
      if (complex.Definition.IsMany) {
        WriteLine(level, "[");
        foreach (Instance instance in complex.Instances)
          RenderInstanceAsJavaScript(level + 1, instance);
        WriteLine(level, "]");
      } else {
        Instance instance = complex.Instances.FirstOrDefault();
        if (instance != null)
          RenderInstanceAsJavaScript(level, instance);
      }
    }

    private void RenderInstanceAsJavaScript(int level, Instance instance) {
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null) return;

      WriteLine(level, "{");

      foreach (PlatformAttribute attribute in platClassDef.PlatformAttributes) {
        object value = attribute.CalculateValue(this, instance, out bool isCodeSnippet);
        WriteAttribute(OutputType.JSON, level + 1, value, attribute, isCodeSnippet);
      }

      WriteLine(level, "},");
    }
    #endregion

    #region GenerateComponentRecursively

    private void GenerateComponentRecursively(OutputType outputType, int level, Instance instance) {
      if (instance == null)
        return;

      // Find the Platform Class Definition
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null)
        return;

      ImportsPlaceholder.WriteLine("import {0} from '{1}';", platClassDef.PlatformName, platClassDef.ImportPath);

      // Open the React tag
      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName);

      foreach (PlatformAttribute attribute in platClassDef.PlatformAttributes) {
        object value = attribute.CalculateValue(this, instance, out bool isCodeSnippet);
        if (value is CodeSnippetGenerator snippetGenerator)
          snippetGenerator.Generate(this, level, platClassDef, instance);
        else
          WriteAttribute(outputType, level + 1, value, attribute, isCodeSnippet);
      }

      WritePrimaryAttributeAsProperty(level + 1, platClassDef, instance);
      WriteNestedContentsAndClosingTag(level, platClassDef, instance);
    }

    private void WritePrimaryAttributeAsProperty(int level, PlatformClassDef platClassDef, Instance instance) {
      // If this is present, write the Primary Attribute as a property rather than as child
      // components (e.g. columns of a Table)
      string propName = platClassDef.PrimaryAttributeWrapperProperty;
      if (propName == null)
        return;

      UiAttributeValue primaryValue = instance.PrimaryValue;

      WriteLine(level + 1, "{0}={", propName);

      if (primaryValue is UiAttributeValueComplex complexAttr) {
        RenderComplexAttrAsJavascript(level + 2, complexAttr);
      } else
        throw new NotImplementedException();

      WriteLine(level + 1, "}", propName);
    }

    private void WriteNestedContentsAndClosingTag(int level, PlatformClassDef platClassDef, Instance instance) {
      UiAttributeValueComplex primaryValue = instance.PrimaryValue as UiAttributeValueComplex;
      if (platClassDef.PrimaryAttributeWrapperProperty != null)
        primaryValue = null;

      // Close the XAML element, possibly recursively writing nested content
      if (primaryValue == null)
        WriteLineClose(level, "/>");
      else {
        WriteLineClose(level, ">");

        foreach (Instance childInstance in primaryValue.Instances)
          GenerateComponentRecursively(OutputType.React, level + 1, childInstance);

        WriteLine(level, "</{0}>", platClassDef.EffectivePlatformName);
      }
    }

    #endregion

    #region Utilities
    private void WriteAttribute(OutputType outputType, int level, object value, PlatformAttribute attribute, bool? isCodeSnippetOverride = null) {
      bool isCodeSnippet = isCodeSnippetOverride ?? attribute.IsCodeSnippet;
      string name = attribute.PlatformName;

      value = value ?? attribute.DefaultValue;
      if (value == null) return;
      string jsValue = isCodeSnippet ? value.ToString() : TypedLiteralToString(value, null);

      if (outputType == OutputType.React) {
        bool needsBrackets = isCodeSnippet || !(value is string);
        if (needsBrackets)
          WriteLine(level + 1, "{0}={ {1} }", name, jsValue);
        else
          WriteLine(level + 1, "{0}={1}", name, jsValue);
      } else
        WriteLine(level + 1, "{0}: {1},", name, jsValue);
    }
    #endregion
  }
}