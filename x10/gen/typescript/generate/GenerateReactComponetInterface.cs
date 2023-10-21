using x10.model.definition;
using x10.ui.composition;
using x10.gen.typescript.placeholder;

namespace x10.gen.typescript.generate {
  public partial class TypeScriptCodeGenerator {

    #region Top Level
    // Model is guaranteed to be non-null
    private void GenerateInterface(ClassDefX10 classDef, Entity model, bool isForm) {
      Begin(classDef.XmlElement.FileInfo, "Interface.tsx");

      GenerateImports(classDef);

      if (classDef.IsMany) {
        GenerateMultiInterface(classDef, model);
        GenerateMultiGraphqlQuery(classDef, model);
      } else if (model != null) {
        GenerateSingleInterface(classDef, model);
        GenerateSingleGraphqlQuery(classDef, model);
      }

      End();
    }
    #endregion

    #region Generate Imports
    private void GenerateImports(ClassDefX10 classDef) {
      InsertImportsPlaceholder();

      ImportsPlaceholder.ImportReact();
      ImportsPlaceholder.Import("gql", "@apollo/client", ImportLevel.ThirdParty);

      WriteLine();
    }
    #endregion

    #region Non-Multi Generation

    private void GenerateSingleInterface(ClassDefX10 classDef, Entity model) {
      WriteLine(0,
@"type Props = { 
  readonly id?: string,      // When invoked from another Component
  readonly match?: {         // When invoked via Route
    readonly params: { 
      readonly id: string
    }
  }
}};");

      string classDefName = classDef.Name;
      string createDefaultFunc = CreateDefaultFuncName(model);
      string fragmentName = CreateFragmentName(classDef, model);
      string variableName = VariableName(model);
      string classDefStateful = classDefName + "Stateful";

      WriteLine(0, "export default function {0}Interface(props: Props): React.JSX.Element {", classDefName);
      WriteLine(1, "return (");
      WriteLine(2, "<EntityQueryRenderer<{0}>", fragmentName);
      WriteLine(3, "id={ props.id }");
      WriteLine(3, "match={ props.match }");
      WriteLine(3, "createComponentFunc={ ({0}) => <{1} {0}={ {0} }/> }", variableName, classDefStateful);

      WriteLine(3, "createEntityFunc={ {0} }", createDefaultFunc);
      ImportsPlaceholder.Import(classDefStateful, classDef);

      WriteLine(3, "query={ query }");
      WriteLine(2, "/>");
      WriteLine(1, ");");
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefaultFromReactLib("client_apollo/EntityQueryRenderer");
      ImportsPlaceholder.ImportCreateDefaultFunc(model);
      ImportsPlaceholder.ImportGraphqlType(fragmentName);
  
      WriteLine();
    }

    private void GenerateSingleGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model);

      WriteLine(0, "const query = gql`");
      WriteLine(1, "query {0}InterfaceQuery($id: String!) {", classDefName);
      WriteLine(2, "entity: {0}(id: $id) {", variableName);

      WriteLine(3, "...{0}_{1}", classDefName, variableName);

      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      WriteLine();
    }
    #endregion

    #region Multi-Related Generation
    private void GenerateMultiInterface(ClassDefX10 classDef, Entity model) {
      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);

      WriteLine(0,
@"export default function {0}Interface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ ({1}) => <{0} {1}={ {1} }/> }
      query={ query }
    />
  );
}}",
      classDefName,       // Index 0
      variableName);      // Index 1

      ImportsPlaceholder.ImportDefaultFromReactLib("client_apollo/MultiEntityQueryRenderer");

      WriteLine();
    }

    private void GenerateMultiGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);

      WriteLine(0, "const query = gql`");
      WriteLine(1, "query {0}InterfaceQuery {", classDefName);
      WriteLine(2, "entities: {0} {", variableName);

      WriteLine(3, "...{0}_{1}", classDefName, variableName);

      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      WriteLine();
    }
    #endregion
  }
}