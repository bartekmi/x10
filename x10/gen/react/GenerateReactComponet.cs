using System;
using System.Collections.Generic;
using System.Linq;

using x10.utils;
using x10.formula;
using x10.model.definition;
using x10.ui;
using x10.ui.composition;
using x10.ui.platform;
using x10.ui.metadata;

namespace x10.gen.react {
  public partial class ReactCodeGenerator {

    internal enum OutputType {
      JSON,
      React,
    }

    private void PreProcessTree(ClassDefX10 classDef) {
      foreach (Instance instance in UiUtils.ListSelfAndDescendants(classDef.RootChild)) 
        if (instance.HasAttributeValue(ClassDefNative.ATTR_VISIBLE)) {
          Instance intermediate = UiTreeUtils.InsertIntermediateParent(instance, ClassDefNative.VisibilityControl);
          UiAttributeValue visibleAttribute = instance.RemoveAttributeValue(ClassDefNative.ATTR_VISIBLE);
          intermediate.AttributeValues.Add(visibleAttribute);
        }
    }

    public override void Generate(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;

      GenerateMainUiFile(classDef, model);
      if (model != null)
        GenerateInterface(classDef, model);  // Done in GenerateReactComponentInterface file (partial class)
    }

    private void GenerateMainUiFile(ClassDefX10 classDef, Entity model) {
      PreProcessTree(classDef);

      bool isForm = true; // TODO

      Begin(classDef.XmlElement.FileInfo, ".jsx");

      GenerateFileHeader();
      GenerateImports(model, isForm);
      GenerateComponent(classDef);

      End();
    }

    private void GenerateImports(Entity model, bool isForm) {
      WriteLine(0, "import * as React from 'react';");
      if (isForm)
        WriteLine(0, "import { graphql, commitMutation } from 'react-relay';");
      WriteLine();

      ImportsPlaceholder = new ImportsPlaceholder();
      AddPlaceholder(ImportsPlaceholder);
      WriteLine();

      if (model != null) {
        WriteLine(0, "import { type {0} } from '{1}';", model.Name, ImportPath(model));
        WriteLine();
      }
    }

    private void GenerateComponent(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      PushSourceVariableName(VariableName(model, classDef.IsMany));

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        string typeName = model.Name;
        if (classDef.IsMany)
          typeName = string.Format("$ReadOnlyArray<{0}>", typeName);

        WriteLine(1, "+{0}: {1},", SourceVariableName, typeName);
        WriteLine(1, "+onChange: ({0}: {1}) => void,", SourceVariableName, typeName);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "export default function {0}(props: Props): React.Node {", classDef.Name);
      if (model != null) 
        WriteLine(1, "const { {0}, onChange } = props;", SourceVariableName);
      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(OutputType.React, 2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();

      PopSourceVariableName();
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
      CalculateAndWriteAttributes(OutputType.JSON, level + 1, platClassDef, instance);
      WriteLine(level, "},");
    }
    #endregion

    #region GenerateComponentRecursively

    internal void GenerateComponentRecursively(OutputType outputType, int level, Instance instance) {
      if (instance == null)
        return;

      // Find the Platform Class Definition
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null)
        return;

      ImportsPlaceholder.ImportDefault(platClassDef.ImportPath);

      // Open the React tag
      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName);
      CalculateAndWriteAttributes(outputType, level + 1, platClassDef, instance);
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

      WriteLine(level, "{0}={", propName);

      if (primaryValue is UiAttributeValueComplex complexAttr) {
        RenderComplexAttrAsJavascript(level + 1, complexAttr);
      } else
        throw new NotImplementedException();

      WriteLine(level, "}", propName);
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
    private void CalculateAndWriteAttributes(OutputType outputType, int level, PlatformClassDef platClassDef, Instance instance) {
      foreach (PlatformAttribute attribute in platClassDef.PlatformAttributes) {
        object value = attribute.CalculateValue(this, instance, out bool isCodeSnippet);
        if (value is CodeSnippetGenerator snippetGenerator)
          snippetGenerator.Generate(this, level, platClassDef, instance);
        else
          WriteAttribute(outputType, level, value, attribute, isCodeSnippet);
      }
    }

   private void WriteAttribute(OutputType outputType, int level, object value, PlatformAttribute attribute, bool? isCodeSnippetOverride = null) {
      bool isCodeSnippet = isCodeSnippetOverride ?? attribute.IsCodeSnippet;
      string name = attribute.PlatformName;

      value = value ?? attribute.DefaultValue;
      if (value == null) return;
      string jsValue = isCodeSnippet ? value.ToString() : TypedLiteralToString(value, null);

      if (outputType == OutputType.React) {
        bool needsBrackets = isCodeSnippet || !(value is string);
        if (needsBrackets)
          WriteLine(level, "{0}={ {1} }", name, jsValue);
        else
          WriteLine(level, "{0}={1}", name, jsValue);
      } else
        WriteLine(level, "{0}: {1},", name, jsValue);
    }
    #endregion
  }
}