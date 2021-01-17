using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.ui;
using x10.ui.composition;
using x10.ui.platform;
using x10.ui.metadata;
using x10.ui.libraries;

namespace x10.gen.react {
  public partial class ReactCodeGenerator {

    internal enum OutputType {
      JSON,
      React,
    }

    #region Top Level
    public override void Generate(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      bool isForm = classDef.RootChild.RenderAs.Name == BaseLibrary.CLASS_DEF_FORM;

      GenerateMainUiFile(classDef, model, isForm);
      if (model != null)
        GenerateInterface(classDef, model, isForm);  // Done in GenerateReactComponentInterface file (partial class)
    }

    private void GenerateMainUiFile(ClassDefX10 classDef, Entity model, bool isForm) {
      PreProcessTree(classDef);

      Begin(classDef.XmlElement.FileInfo, ".jsx");

      GenerateFileHeader();
      GenerateImports(model);
      GenerateComponent(classDef, isForm);

      if (isForm) {
        GenerateSave(model);
        GenerateGraphqlMutation(classDef, model);
      }

      End();
    }

    private void PreProcessTree(ClassDefX10 classDef) {
      foreach (Instance instance in UiUtils.ListSelfAndDescendants(classDef.RootChild))
        if (instance.HasAttributeValue(ClassDefNative.ATTR_VISIBLE)) {
          Instance intermediate = UiTreeUtils.InsertIntermediateParent(instance, ClassDefNative.VisibilityControl);
          UiAttributeValue visibleAttribute = instance.RemoveAttributeValue(ClassDefNative.ATTR_VISIBLE);
          intermediate.AttributeValues.Add(visibleAttribute);
        }
    }

    private void GenerateImports(Entity model) {
      InsertImportsPlaceholder();

      ImportsPlaceholder.ImportReact();
      if (model != null) 
        ImportsPlaceholder.ImportType(model.Name, model);

      WriteLine();
    }

    private void GenerateComponent(ClassDefX10 classDef, bool isForm) {
      Entity model = classDef.ComponentDataModel;
      PushSourceVariableName(VariableName(model, classDef.IsMany));

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        string typeName = model.Name;
        if (classDef.IsMany)
          typeName = string.Format("$ReadOnlyArray<{0}>", typeName);

        WriteLine(1, "+{0}: {1},", SourceVariableName, typeName);
        if (isForm)
          WriteLine(1, "+onChange: ({0}: {1}) => void,", SourceVariableName, typeName);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "export default function {0}(props: Props): React.Node {", classDef.Name);
      if (isForm) {
        WriteLine(1, "const { onChange } = props;");
        GenerateCreateNullableEntities(model);
      } else
        WriteLine(1, "const { {0} } = props;", SourceVariableName);
      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(OutputType.React, 2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();

      PopSourceVariableName();
    }

    private void GenerateCreateNullableEntities(Entity model) {
      IEnumerable<Association> nullableAssociations = model.Associations
        .Where(x => !(x.IsMandatory || x.IsMany));

      if (nullableAssociations.Count() == 0)
        WriteLine(1, "const { {0} } = props;", SourceVariableName);
      else {
        WriteLine(1, "const { {0}: raw } = props;", SourceVariableName);
        WriteLine();
        WriteLine(1, "const {0} = {", SourceVariableName);
        WriteLine(2, "...raw,");

        // TODO... Not recursive at this time
        foreach (Association association in nullableAssociations) {
          Entity assocModel = association.ReferencedEntity;
          WriteLine(2, "{0}: raw.{0} || {1}(),",
            association.Name,
            ReactCodeGenerator.CreateDefaultFuncName(assocModel));

          ImportsPlaceholder.ImportCreateDefaultFunc(assocModel);
        }

        WriteLine(1, "};");
      }
    }
    #endregion

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

      GenerateComponentRecursively(outputType, level, instance, platClassDef);
    }

    internal void GenerateComponentRecursively(OutputType outputType, int level, Instance instance, PlatformClassDef platClassDef) {
      string[] htmlTags = new string[] { "div" };

      if (htmlTags.Contains(platClassDef.PlatformName)) {
        // Since this is an HTML tag, no need for import
      } else {
        if (platClassDef is JavaScriptPlatformClassDef jsPlatClassDef && jsPlatClassDef.IsNonDefaultImport)
          ImportsPlaceholder.Import(platClassDef.PlatformName, platClassDef.ImportDir);
        else
          ImportsPlaceholder.ImportDefault(platClassDef.ImportPath);
      }

      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName); // Open the React tag
      if (platClassDef.StyleInfo != null)
        Write(" style={ { {0} } }", true, platClassDef.StyleInfo);
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

      if (primaryValue == null &&
          platClassDef.ProgrammaticallyGenerateChildren == null &&
          platClassDef.NestedClassDef == null)
        WriteLineClose(level, "/>");
      else {
        WriteLineClose(level, ">");

        if (platClassDef.NestedClassDef != null)
          GenerateComponentRecursively(OutputType.React, level + 1, instance, platClassDef.NestedClassDef);
        else if (primaryValue != null)
          foreach (Instance childInstance in primaryValue.Instances)
            GenerateComponentRecursively(OutputType.React, level + 1, childInstance);
        else
          platClassDef.ProgrammaticallyGenerateChildren(this, level + 1, platClassDef, instance);

        WriteLine(level, "</{0}>", platClassDef.EffectivePlatformName);
      }
    }

    #endregion

    #region Generate Save & Mutation
    private void GenerateSave(Entity model) {
      string variableName = VariableName(model);
      string modelName = model.Name;

      WriteLine(0, "function save({0}: {1}) {", variableName, modelName);
      WriteLine(1, "const variables = {");

      foreach (Member member in model.Members)
        if (IncludeInMutation(member))
          WriteLine(2, "{0}: {1}.{0},", member.Name, variableName);

      WriteLine(1, "};");

      WriteLine();
      WriteLine(1, "basicCommitMutation(mutation, variables);");
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefault("react_lib/relay/basicCommitMutation");

      WriteLine();
    }

    private bool IncludeInMutation(Member member) {
      if (member is X10RegularAttribute regular && regular.IsId)
        return true;

      return
        !member.IsReadOnly &&
        !(member is X10DerivedAttribute);
    }

    private void GenerateGraphqlMutation(ClassDefX10 classDef, Entity model) {
      string variableName = VariableName(model);
      string classDefName = classDef.Name;

      WriteLine(0, "const mutation = graphql`");
      WriteLine(1, "mutation {0}Mutation(", classDefName);

      foreach (Member member in model.Members)
        if (IncludeInMutation(member))
          WriteLine(2, "${0}: {1}", member.Name, GraphqlType(member));

      WriteLine(1, ") {");
      WriteLine(2, "createOrUpdate{0}(", model.Name);

      foreach (Member member in model.Members)
        if (IncludeInMutation(member))
          WriteLine(3, "{0}: ${0}", member.Name);

      WriteLine(2, ")");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      ImportsPlaceholder.Import("graphql", "react-relay");

      WriteLine();
    }

    #region GQL
    // Eventually, this will probably move to GraphQL utilities
    private string GraphqlType(Member member) {
      if (member is X10Attribute attribute) {
        string typeString = GetAtomicGraphqlType(attribute.DataType);

        if (member.IsMandatory ||
            attribute.DataType == DataTypes.Singleton.String) // Strings are always mandatory to prevent null/empty ambiguity
          typeString += "!";

        return typeString;
      } else if (member is Association association) {
        string refedEntityName = association.ReferencedEntityName;

        // Distinction between null and empty list is ill-defined, so only allow empty list
        if (association.IsMany)
          return string.Format("[{0}Input!]!", refedEntityName);
        else {
          string inputType = refedEntityName + "Input";
          if (member.IsMandatory)
            inputType += "!";
          return inputType;
        }
      } else
        throw new NotImplementedException("Unknown member type: " + member.GetType());
    }

    private string GetAtomicGraphqlType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "Boolean";
      if (dataType == DataTypes.Singleton.Date) return "DateTime";
      if (dataType == DataTypes.Singleton.Float) return "Float";
      if (dataType == DataTypes.Singleton.Integer) return "Int";
      if (dataType == DataTypes.Singleton.String) return "String";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime";
      if (dataType == DataTypes.Singleton.Money) return "Float";
      if (dataType is DataTypeEnum enumType) return EnumToName(enumType);

      throw new NotImplementedException("Unknown data type: " + dataType.Name);
    }
    #endregion

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