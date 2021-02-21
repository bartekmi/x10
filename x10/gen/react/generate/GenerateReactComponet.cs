using System;
using System.Collections.Generic;
using System.Linq;

using x10.model.definition;
using x10.model.metadata;
using x10.ui;
using x10.compiler;
using x10.ui.composition;
using x10.ui.platform;
using x10.ui.metadata;
using x10.ui.libraries;
using x10.gen.react.placeholder;
using x10.gen.react.attribute;

namespace x10.gen.react.generate {

  internal enum OutputType {
    JSON,
    React,
  }

  public partial class ReactCodeGenerator {

    #region Top Level
    internal GqlPlaceholder GqlPlaceholder;

    public override void Generate(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      bool isForm = IsForm(classDef);

      GenerateMainUiFile(classDef, model, isForm);
      if (model != null && classDef.Url != null)
        GenerateInterface(classDef, model, isForm);  // See GenerateReactComponentInterface file (partial class)
    }

    private void GenerateMainUiFile(ClassDefX10 classDef, Entity model, bool isForm) {
      PreProcessTree(classDef);
      GqlPlaceholder = new GqlPlaceholder(classDef);

      Begin(classDef.XmlElement.FileInfo, ".jsx");

      GenerateFileHeader();
      GenerateImports(model);
      GenerateComponent(classDef, model, isForm);

      if (isForm) {
        GenerateStatefulWrapper(classDef, model);
        GenerateFormRelayToInternal(model);
        GenerateSave(model);
        GenerateGraphqlMutation(classDef, model);
      }

      if (model != null)
        GenerateFragment(classDef, model, isForm);

      AddPlaceholder(GqlPlaceholder);

      End();
    }

    private void PreProcessTree(ClassDefX10 classDef) {
      foreach (Instance instance in UiUtils.ListSelfAndDescendants(classDef.RootChild))
        // Any instance which has the "visible" attribute is wrapped with the "VisibilityControl" component
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
    #endregion

    #region Generate Component
    private void GenerateComponent(ClassDefX10 classDef, Entity model, bool isForm) {
      PushSourceVariableName(VariableName(model, classDef.IsMany));

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        if (isForm)
          WriteFormSignature(classDef, model);
        else
          WriteNonFormSignature(classDef);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "{0}function {1}(props: Props): React.Node {",
        model == null ? "export default " : "",
        classDef.Name);

      WriteLine(1, "const { {0}{1} } = props;",
        SourceVariableName,
        isForm ? ", onChange" : "");
      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(OutputType.React, 2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();

      PopSourceVariableName();
    }

    private void WriteFormSignature(ClassDefX10 classDef, Entity model) {
      WriteLine(1, "+{0}: {1},", SourceVariableName, model.Name);
      WriteLine(1, "+onChange: ({0}: {1}) => void,", SourceVariableName, model.Name);
      ImportsPlaceholder.ImportType(model);
    }

    private void WriteNonFormSignature(ClassDefX10 classDef) {
      string fragmentName = FragmentName(classDef);

      WriteLine(1, "+{0}: {1},", SourceVariableName, fragmentName);

      ImportsPlaceholder.ImportType(fragmentName,
        string.Format("./__generated__/{0}.graphql", fragmentName),
        ImportLevel.RelayGenerated);
    }
    #endregion

    #region Generate Javascript Object Recursively
    internal void RenderComplexAttrAsJavascript(int level, UiAttributeValueComplex complex) {
      if (complex.Definition.IsMany) {
        WriteLine(level, "[");
        foreach (Instance instance in complex.Instances)
          RenderInstanceAsJavaScript(level + 1, instance, true);
        WriteLine(level, "]");
      } else {
        Instance instance = complex.Instances.FirstOrDefault();
        if (instance != null)
          RenderInstanceAsJavaScript(level, instance, false);
      }
    }

    private void RenderInstanceAsJavaScript(int level, Instance instance, bool appendComma) {
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null) return;

      WriteLine(level, "{");
      CalculateAndWriteAttributes(OutputType.JSON, level + 1, platClassDef, instance);
      WriteLine(level, "}{0}", appendComma ? "," : "");
    }
    #endregion

    #region Generate Component Recursively

    internal void GenerateComponentRecursively(OutputType outputType, int level, Instance instance, PlatformClassDef platClassDef = null) {
      if (instance == null)
        return;

      if (instance.RenderAs is ClassDefX10 classDefX10) {
        GenerateComponentReference(level, instance, classDefX10);
        return;
      }

      if (platClassDef == null)
        platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null)
        return;

      string[] htmlTags = new string[] { "div" };

      if (htmlTags.Contains(platClassDef.PlatformName)) {
        // Since this is an HTML tag, no need for import
      } else {
        if (platClassDef is JavaScriptPlatformClassDef jsPlatClassDef && jsPlatClassDef.IsNonDefaultImport)
          ImportsPlaceholder.Import(platClassDef.PlatformName, platClassDef.ImportDir, ImportLevel.ThirdParty);
        else
          ImportsPlaceholder.ImportDefault(platClassDef.ImportPath, ImportLevel.ThirdParty);
      }

      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName); // Open the React tag
      if (platClassDef.StyleInfo != null)
        Write(" style={ { {0} } }", true, platClassDef.StyleInfo);
      CalculateAndWriteAttributes(outputType, level + 1, platClassDef, instance);
      WriteNestedContentsAndClosingTag(level, platClassDef, instance);
    }

    // At some point, we may consider this NOT a special caes, but, rather, "fabricate" a PlatformClassDef
    // Do this if you're ever tempted to add more code here that is similar to the code in GenerateComponentRecursively
    private void GenerateComponentReference(int level, Instance instance, ClassDefX10 classDefX10) {
      string path = CodeGenUtils.GetBindingPathAsString(instance);
      if (!string.IsNullOrEmpty(path))
        path = "." + path;

      // Making an executive decision that Forms will be included as the Interface version
      // of the component - i.e. they will make a separate GraphQL query for their data.
      // At least one argument for this is that, if we're going to edit data, we want to start
      // with the freshest copy. (There may be others)
      if (IsForm(classDefX10)) {
        string compInterface = classDefX10.Name + "Interface";
        ImportsPlaceholder.ImportDefault(classDefX10, "Interface");

        WriteLine(level, "<{0}Interface id={ {1}{2}.id }/>",
          classDefX10.Name,
          SourceVariableName,
          path);
      } else {
        ImportsPlaceholder.ImportDefault(classDefX10);

        WriteLine(level, "<{0} {1}={ {2}{3} }/>",
          classDefX10.Name,
          VariableName(classDefX10.ComponentDataModel),
          SourceVariableName,
          path);
      }

    }

    private void WriteNestedContentsAndClosingTag(int level, PlatformClassDef platClassDef, Instance instance) {
      UiAttributeValueComplex primaryValue = instance.PrimaryValue as UiAttributeValueComplex;
      bool primaryRenderedAsProp = platClassDef.PlatformAttributes.OfType<JavaScriptAttributePrimaryAsProp>().Any();

      if ((primaryValue == null || primaryRenderedAsProp) &&
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

    #region Generate Stateful Wrapper
    private void GenerateStatefulWrapper(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string modelName = model.Name;
      string variableName = VariableName(model);
      string edited = "edited" + modelName;
      string setEdited = "setEdited" + modelName;

      WriteLine(0, "type StatefulProps = {{|");
      WriteLine(1, "+{0}: {1},", variableName, modelName);
      WriteLine(0, "|}};");

      WriteLine(0, "export function {0}Stateful(props: StatefulProps): React.Node {", classDefName);
      WriteLine(1, "const {0} = relayToInternal(props.{0});", variableName);
      WriteLine(1, "const [{0}, {1}] = React.useState({2});", edited, setEdited, variableName);
      WriteLine(1, "return <{0}", classDefName);
      WriteLine(2, "{0}={ {1} } ", variableName, edited);
      WriteLine(2, "onChange={ {0} }", setEdited);
      WriteLine(1, "/>");
      WriteLine(0, "}");

      WriteLine();

      ImportsPlaceholder.ImportType(model);
    }
    #endregion

    #region Generate Relay To Internal
    private void GenerateFormRelayToInternal(Entity model) {
      WriteLine(0, "function relayToInternal(relay: any): {0} {", model.Name);
      WriteLine(1, "return {");
      WriteLine(2, "...relay,");

      // TODO... Not recursive at this time
      foreach (Association association in model.Associations) {
        bool isNullable = !association.IsMandatory && !association.IsMany;
        if (isNullable) {
          Entity assocModel = association.ReferencedEntity;
          WriteLine(2, "{0}: relay.{0} || {1}(),",
            association.Name,
            ReactCodeGenerator.CreateDefaultFuncName(assocModel));

          ImportsPlaceholder.ImportCreateDefaultFunc(assocModel);
        } else if (!association.Owns)
          WriteLine(2, "{0}: relay.{0}?.id,", association.Name);
      }

      WriteLine(1, "};");
      WriteLine(0, "}");
      WriteLine();
    }
    #endregion

    #region Generate Fragment
    private void GenerateFragment(ClassDefX10 classDef, Entity model, bool isForm) {
      string variableName = VariableName(model, classDef.IsMany);

      WriteLine(0, "// $FlowExpectedError");
      WriteLine(0, "export default createFragmentContainer({0}{1}, {",
        classDef.Name,
        isForm ? "Stateful" : "");
      WriteLine(1, "{0}: graphql`", variableName);
      WriteLine(2, "fragment {0} on {1} {2}{",
        FragmentName(classDef),
        model.Name,
        classDef.IsMany ? "@relay(plural: true) " : "");

      MemberWrapper dataRoot = UiComponentDataCalculator.ExtractData(classDef);
      PrintGraphQL(3, dataRoot);

      WriteLine(2, "}");
      WriteLine(1, "`,");
      WriteLine(0, "});");
      WriteLine();

      ImportsPlaceholder.Import("createFragmentContainer", "react-relay", ImportLevel.ThirdParty);
      ImportsPlaceholder.Import("graphql", "react-relay", ImportLevel.ThirdParty);
    }

    private static string FragmentName(ClassDefX10 classDef) {
      return string.Format("{0}_{1}",
        classDef.Name,
        VariableName(classDef.ComponentDataModel, classDef.IsMany));
    }

    public void PrintGraphQL(int indent, MemberWrapper wrapper) {
      if (wrapper.RootEntity != null)
        PrintGraphQL_Children(indent, wrapper);
      else if (wrapper.Member is Association association) {
        WriteLine(indent, wrapper.Member.Name + " {");
        PrintGraphQL_Children(indent + 1, wrapper);
        WriteLine(indent, "}");
      } else
        WriteLine(indent, wrapper.Member.Name);
    }

    private void PrintGraphQL_Children(int indent, MemberWrapper wrapper) {
      WriteLine(indent, "id");
      if (wrapper.Member != null && wrapper.Member.IsNonOwnedAssociation)
        WriteLine(indent, "toStringRepresentation");

      foreach (MemberWrapper child in wrapper.Children.OrderBy(x => x.Member.Name))
        if (child.Member.Name != "id")
          PrintGraphQL(indent, child);

      foreach (ClassDefX10 classDef in wrapper.ComponentReferences)
        if (!IsForm(classDef))
          WriteLine(indent, "...{0}", FragmentName(classDef));
    }

    #endregion

    #region Generate Save
    private void GenerateSave(Entity model) {
      string variableName = VariableName(model);
      string modelName = model.Name;

      WriteLine(0, "function save({0}: {1}) {", variableName, modelName);
      WriteLine(1, "basicCommitMutation(mutation, { {0} });", variableName);
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefault("react_lib/relay/basicCommitMutation", ImportLevel.ThirdParty);

      WriteLine();
    }

    private bool IncludeInMutation(Member member) {
      if (member is X10RegularAttribute regular && regular.IsId)
        return true;

      return
        !member.IsReadOnly &&
        !(member is X10DerivedAttribute);
    }
    #endregion

    #region Generate Mutation
    private void GenerateGraphqlMutation(ClassDefX10 classDef, Entity model) {
      string variableName = VariableName(model);
      string classDefName = classDef.Name;

      WriteLine(0, "const mutation = graphql`");
      WriteLine(1, "mutation {0}Mutation(", classDefName);
      WriteLine(2, "${0}: {1}Input!", variableName, model.Name);
      WriteLine(1, ") {");
      WriteLine(2, "createOrUpdate{0}(", model.Name);
      WriteLine(3, "{0}: ${0}", variableName);
      WriteLine(2, ")");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      ImportsPlaceholder.Import("graphql", "react-relay", ImportLevel.ThirdParty);

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
          string inputType = association.Owns ? refedEntityName + "Input" : "String";
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
        else if (value is UiAttributeValueComplex complex)
          WriteComplexAttribute(level, complex, attribute);
        else
          WriteAttribute(outputType, level, value, attribute, isCodeSnippet);
      }
    }

    private void WriteAttribute(OutputType outputType, int level, object value, PlatformAttribute attribute, bool? isCodeSnippetOverride = null) {
      value = value ?? attribute.DefaultValue;
      if (value == null ||                                      // No data, no attribute
          value.Equals(attribute.AttributeUnnecessaryWhen))     // Attribute would be unnecessary
        return;

      string name = attribute.PlatformName;
      bool isCodeSnippet = isCodeSnippetOverride ?? attribute.IsCodeSnippet;
      string jsValue = TypedLiteralToString(value, null, isCodeSnippet);

      if (outputType == OutputType.React) {
        bool skipBraces = !isCodeSnippet && value is string;
        if (skipBraces)
          WriteLine(level, "{0}={1}", name, jsValue);
        else
          WriteLine(level, "{0}={ {1} }", name, jsValue);
      } else
        WriteLine(level, "{0}: {1},", name, jsValue);
    }

    private void WriteComplexAttribute(int level, UiAttributeValueComplex value, PlatformAttribute attribute) {
      WriteLine(level, "{0}={", attribute.PlatformName);
      RenderComplexAttrAsJavascript(level + 1, value);
      WriteLine(level, "}");
    }
    #endregion
  }
}