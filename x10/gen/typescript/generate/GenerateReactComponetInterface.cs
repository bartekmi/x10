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

      ImportsPlaceholder.ImportType(model);

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
      string classDefName = classDef.Name;
      string createDefaultFunc = CreateDefaultFuncName(model);
      string variableName = VariableName(model);

      WriteLine(0, "export default function {0}Interface(): React.JSX.Element {", classDefName);

      WriteLine(1, "const params = useParams()");
      ImportsPlaceholder.Import("useParams", "react-router-dom", ImportLevel.ThirdParty);

      WriteLine(1, "return (");
      WriteLine(2, "<EntityQueryRenderer<{0}>", model.Name);
      WriteLine(3, "id={ params.id }");

      // For Forms, we use the "stateful" version of the component
      string childElement = null;
      if (IsForm(classDef)) {
        childElement = classDefName + "Stateful";
        ImportsPlaceholder.Import(childElement, classDef);
      } else {
        childElement = classDefName;
        ImportsPlaceholder.ImportDefault(classDef);
      }
      WriteLine(3, "createComponentFunc={ ({0}) => <{1} {0}={ {0} }/> }", variableName, childElement);

      WriteLine(3, "createEntityFunc={ {0} }", createDefaultFunc);

      WriteLine(3, "query={ query }");
      WriteLine(2, "/>");
      WriteLine(1, ");");
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefaultFromReactLib("client_apollo/EntityQueryRenderer");
      ImportsPlaceholder.ImportCreateDefaultFunc(model);
  
      WriteLine();
    }

    private void GenerateSingleGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model);
      string fragmentConst = FragmentConst(classDef);

      WriteLine(0, "const query = gql`");
      WriteLine(1, "query {0}InterfaceQuery($id: String!) {", classDefName);
      WriteLine(2, "entity: {0}(id: $id) {", variableName);
      WriteLine(3, "...{0}", FragmentName(classDef));
      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(1, "${ {0} }", fragmentConst);
      WriteLine(0, "`;");
      WriteLine();

      ImportsPlaceholder.Import(fragmentConst, classDef);
    }
    #endregion

    #region Multi-Related Generation
    private void GenerateMultiInterface(ClassDefX10 classDef, Entity model) {
      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);

      WriteLine(0,
@"export default function {0}Interface(props: { }): React.JSX.Element { 
  return (
    <MultiEntityQueryRenderer<{1}>
      createComponentFunc={ ({2}) => <{0} {2}={ {2} }/> }
      query={ query }
    />
  );
}}",
      classDefName,       // Index 0
      model.Name,         // Index 1
      variableName);      // Index 2

      ImportsPlaceholder.ImportDefault(classDef);
      ImportsPlaceholder.ImportDefaultFromReactLib("client_apollo/MultiEntityQueryRenderer");

      WriteLine();
    }

    private void GenerateMultiGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);
      string fragmentConst = FragmentConst(classDef);

      WriteLine(0, "const query = gql`");
      WriteLine(1, "query {0}InterfaceQuery {", classDefName);
      WriteLine(2, "entities: {0} {", variableName);
      WriteLine(3, "...{0}", FragmentName(classDef));
      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(1, "${ {0} }", fragmentConst);
      WriteLine(0, "`;");
      WriteLine();

      ImportsPlaceholder.Import(fragmentConst, classDef);
    }
    #endregion
  }
}