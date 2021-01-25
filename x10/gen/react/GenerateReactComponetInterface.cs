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

    #region Top Level
    // Model is guaranteed to be non-null
    private void GenerateInterface(ClassDefX10 classDef, Entity model, bool isForm) {
      Begin(classDef.XmlElement.FileInfo, "Interface.jsx");

      GenerateFileHeader();
      GenerateImports(classDef);

      if (classDef.IsMany) {
        GenerateMultiQueryRenderer(classDef, model);
        GenerateMultiGraphqlQuery(classDef, model);
      } else if (model != null) {
        GenerateFormQueryRenderer(classDef, model);
        GenerateFormGraphqlQuery(classDef, model);
      }

      End();
    }

    private void GenerateImports(ClassDefX10 classDef) {
      InsertImportsPlaceholder();

      ImportsPlaceholder.ImportReact();
      ImportsPlaceholder.Import("graphql", "react-relay");
      ImportsPlaceholder.Import("QueryRenderer", "react-relay");
      ImportsPlaceholder.ImportDefault("environment");

      ImportsPlaceholder.ImportDefault(classDef);

      WriteLine();
    }
    #endregion

    #region Form-Related Generation

    private void GenerateFormQueryRenderer(ClassDefX10 classDef, Entity model) {
      WriteLine(0,
@"type Props = { 
  +match: { 
    +params: { 
      +id: string
    }
  }
}};");

      string classDefName = classDef.Name;
      string createDefaultFunc = CreateDefaultFuncName(model);
      string variableName = VariableName(model);
      string classDefStateful = classDefName + "Stateful";

      WriteLine(0, "export default function {0}Interface(props: Props): React.Node {", classDefName);
      WriteLine(1, "return (");
      WriteLine(2, "<EntityQueryRenderer");
      WriteLine(3, "match={ props.match }");
      WriteLine(3, "createComponentFunc={ ({0}) => <{1} {0}={ {0} }/> }", variableName, classDefName);
      WriteLine(3, "createComponentFuncNew={ () => <{0} {1}={ {2}() }/> }", classDefStateful, variableName, createDefaultFunc);
      WriteLine(3, "query={ query }");
      WriteLine(2, "/>");
      WriteLine(1, ");");
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefault("react_lib/relay/EntityQueryRenderer");
      ImportsPlaceholder.ImportCreateDefaultFunc(model);
      ImportsPlaceholder.Import(classDefStateful, classDef);

      WriteLine();
    }

    private void GenerateFormGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model);

      WriteLine(0, "const query = graphql`");
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
    private void GenerateMultiQueryRenderer(ClassDefX10 classDef, Entity model) {
      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);

      WriteLine(0,
@"export default function {0}Interface(props: { }): React.Node { 
  return (
    <MultiEntityQueryRenderer
      createComponentFunc={ ({1}) => <{0} {1}={ {1} }/> }
      query={ query }
    />
  );
}}",
      classDefName,       // Index 0
      variableName);      // Index 1

      ImportsPlaceholder.ImportDefault("react_lib/relay/MultiEntityQueryRenderer");

      WriteLine();
    }

    private void GenerateMultiGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model, true);

      WriteLine(0, "const query = graphql`");
      WriteLine(1, "query {0}InterfaceQuery {", classDefName);
      WriteLine(2, "entities: {0} {", variableName);
      WriteLine(3, "nodes {");

      WriteLine(4, "...{0}_{1}", classDefName, variableName);

      WriteLine(3, "}");
      WriteLine(2, "}");
      WriteLine(1, "}");
      WriteLine(0, "`;");

      WriteLine();
    }
    #endregion
  }
}