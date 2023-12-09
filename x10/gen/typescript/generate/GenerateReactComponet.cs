using System;
using System.Collections.Generic;
using System.Linq;

using x10.utils;
using x10.model.definition;
using x10.model.metadata;
using x10.compiler;
using x10.ui;
using x10.ui.composition;
using x10.ui.platform;
using x10.ui.metadata;
using x10.gen.typescript.placeholder;
using x10.gen.typescript.attribute;

namespace x10.gen.typescript.generate {

  internal enum OutputType {
    JSON,
    React,
  }

  public partial class TypeScriptCodeGenerator {

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
      MemberWrapper dataInventory = model == null ? null : UiComponentDataCalculator.ExtractData(classDef);

      Begin(classDef.XmlElement.FileInfo, ".tsx");

      GenerateImports(model);
      GenerateComponent(classDef, model, isForm);

      if (isForm) {
        GenerateStatefulWrapper(classDef, model);
        GenerateGraphqlMutation(classDef, model, dataInventory);
      }

      if (model != null)
        GenerateFragment(classDef, model, dataInventory, isForm);

      AddPlaceholder(GqlPlaceholder);

      End();
    }

    private void PreProcessTree(ClassDefX10 classDef) {
      // Any instance which has any of the "ClassDefVisual" attributes set is wrapped with the "StyleControl" component
      var visualAttrs = new HashSet<UiAttributeDefinition>(ClassDefNative.Visual.AttributeDefinitions);
      foreach (Instance instance in UiUtils.ListSelfAndDescendants(classDef.RootChild)) {
        bool needsStyleControl = instance.AttributeValues.Any(x => visualAttrs.Contains(x.Definition));

        if (needsStyleControl) {
          Instance intermediate = UiTreeUtils.InsertIntermediateParent(instance, ClassDefNative.StyleControl);
          foreach (UiAttributeValue attrValue in instance.AttributeValues.ToList())
            if (visualAttrs.Contains(attrValue.Definition)) {
              instance.AttributeValues.Remove(attrValue);
              intermediate.AttributeValues.Add(attrValue);
            }
        }
      }

      // Other pre-processing operations can go here:
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
      WriteProps(classDef, model, isForm);

      // Component Definition
      if (isForm) {
        WriteLine(0, "function {0}(props: Props): React.JSX.Element {", classDef.Name);
        WriteLine(1, "const { {0}, onChange } = props;", SourceVariableName);
      } else {
        WriteLine(0, "export default function {0}(props: Props): React.JSX.Element {", classDef.Name);
        WriteLine(1, "const { {0} } = props;", SourceVariableName);
      }

      WriteLine(1, "const appContext = React.useContext(AppContext);");
      ImportsPlaceholder.ImportAppContext();

      WriteLine();

      WriteLine(1, "return (");
      GenerateComponentRecursively(OutputType.React, 2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
      WriteLine();

      PopSourceVariableName();
    }

    private void WriteProps(ClassDefX10 classDef, Entity model, bool isForm) {
      WriteLine(0, "type Props = {");
      if (model != null) {
        WriteLine(1, "readonly {0}{1}: {2}{3},", 
          SourceVariableName, 
          isForm ? "" : "?",
          model.Name,
          classDef.IsMany ? "[]" : "");

        if (isForm)
          WriteLine(1, "readonly onChange: ({0}: {1}) => void,", SourceVariableName, model.Name);
      }
      WriteLine(0, "};");
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
        ImportsPlaceholder.Import(platClassDef);
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
        path = "?." + path;

      // At one point, for Forms, I wanted to embed the Interface (go back through history Oct 7 2021)
      // but changed my mind - data is just passed to form
      ImportsPlaceholder.ImportDefault(classDefX10);

      WriteLine(level, "<{0} {1}={ {2}{3} }/>",
        classDefX10.Name,
        VariableName(classDefX10.ComponentDataModel, classDefX10.IsMany),
        SourceVariableName,
        path);
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


      WriteLine(0, "type StatefulProps = {");
      WriteLine(1, "readonly {0}: {1},", variableName, model.Name);
      WriteLine(0, "};");

      WriteLine(0, "export function {0}Stateful(props: StatefulProps): React.JSX.Element {", classDefName);
      WriteLine(1, "const [{0}, {1}] = React.useState(props.{2});", edited, setEdited, variableName);
      WriteLine(1, "return <{0}", classDefName);
      WriteLine(2, "{0}={ {1} } ", variableName, edited);
      WriteLine(2, "onChange={ {0} }", setEdited);
      WriteLine(1, "/>");
      WriteLine(0, "}");

      WriteLine();
    }
    #endregion

    #region Generate Fragment
    private void GenerateFragment(ClassDefX10 classDef, Entity model, MemberWrapper dataInventory, bool isForm) {
      string fragmentName = FragmentName(classDef);
      string fragmentConst = FragmentConst(classDef);

      WriteLine(0, "export const {0} = gql`", fragmentConst);
      WriteLine(1, "fragment {0} on {1} {", fragmentName, model.Name);

      PrintGraphQL(2, dataInventory);

      WriteLine(1, "}");
      WriteLine(0, "`");
      WriteLine();

      ImportsPlaceholder.Import("gql", "@apollo/client", ImportLevel.ThirdParty);
    }

    #region Printing of GraphQL
    public void PrintGraphQL(int indent, MemberWrapper wrapper) {
      if (wrapper.RootEntity != null)
        PrintGraphQL_Children(indent, wrapper);
      else if (wrapper.Member is Association) {
        WriteLine(indent, wrapper.Member.Name + " {");
        PrintGraphQL_Children(indent + 1, wrapper);
        WriteLine(indent, "}");
      } else
        WriteLine(indent, wrapper.Member.Name);
    }

    private void PrintGraphQL_Children(int indent, MemberWrapper wrapper) {
      WriteLine(indent, "id");
      // More comments why this is removed in hot_chocolate generation
      // if (wrapper.Member != null && wrapper.Member.IsNonOwnedAssociation)
      //   WriteLine(indent, "toStringRepresentation");

      foreach (MemberWrapper child in wrapper.Children.OrderBy(x => x.Member.Name))
        if (child.Member.Name != "id")
          PrintGraphQL(indent, child);

      foreach (ClassDefX10 classDef in wrapper.ComponentReferences)
        WriteLine(indent, "...{0}", FragmentName(classDef));
    }

    #endregion

    #endregion

    #region Generate Mutation
    private void GenerateGraphqlMutation(ClassDefX10 classDef, Entity model, MemberWrapper dataInventory) {
      string variableName = VariableName(model);
      string classDefName = classDef.Name;

      WriteLine(0, "const mutation = gql`");
      WriteLine(1, "mutation {0}Mutation(", classDefName);
      WriteLine(2, "${0}: {1}{2}Input!", variableName, classDef.Name, model.Name);
      WriteLine(1, ") {");
      WriteLine(2, "{0}Update{1}(", 
        NameUtils.UncapitalizeFirstLetter(classDef.Name), 
        model.Name);
      WriteLine(3, "data: ${0}", variableName);
      WriteLine(2, ") {");

      PrintGraphQL(3, dataInventory);

      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      ImportsPlaceholder.Import("gql", "@apollo/client", ImportLevel.ThirdParty);

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
      if (dataType == DataTypes.Singleton.Time) return "TimeSpan";
      if (dataType == DataTypes.Singleton.Float) return "Float";
      if (dataType == DataTypes.Singleton.Integer) return "Int";
      if (dataType == DataTypes.Singleton.String) return "String";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime";
      if (dataType == DataTypes.Singleton.Money) return "Float";
      if (dataType is DataTypeEnum enumType) return EnumToTypeName(enumType);

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
      value ??= attribute.DefaultValue;
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