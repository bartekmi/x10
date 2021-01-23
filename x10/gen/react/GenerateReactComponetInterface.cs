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

      if (isForm) {
        GenerateFormWrapper(classDef, model);
        GenerateFormQueryRenderer(classDef, model);
        GenerateFormRelayToInternal(model);
        GenerateFormGraphqlQuery(classDef, model);
      } else if (classDef.IsMany) {
        GenerateMultiQueryRenderer(classDef, model);
        GenerateMultiGraphqlQuery(classDef, model);
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
    private void GenerateFormWrapper(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string modelName = model.Name;
      string variableName = VariableName(model);
      string propsName = modelName + "Props";
      string edited = "edited" + modelName;
      string setEdited = "setEdited" + modelName;

      WriteLine(0, "type {0} = {{|", propsName);
      WriteLine(1, "+{0}: {1},", variableName, modelName);
      WriteLine(0, "|}};");

      WriteLine(0, "function {0}Wrapper(props: {1}): React.Node {", classDefName, propsName);
      WriteLine(1, "const [{0}, {1}] = React.useState(props.{2});", edited, setEdited, variableName);
      WriteLine(1, "return <{0}", classDefName);
      WriteLine(2, "{0}={ {1} } ", variableName, edited);
      WriteLine(2, "onChange={ {0} }", setEdited);
      WriteLine(1, "/>");
      WriteLine(0, "}");

      WriteLine();

      ImportsPlaceholder.ImportType(model);
    }

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

      WriteLine(0, "export default function {0}Interface(props: Props): React.Node {", classDefName);
      WriteLine(1, "return (");
      WriteLine(2, "<EntityQueryRenderer");
      WriteLine(3, "match={ props.match }");
      WriteLine(3, "createDefaultFunc={ {0} }", createDefaultFunc);
      WriteLine(3, "createComponentFunc={ ({0}) => <{1}Wrapper {0}={ relayToInternal({0}) }/> }", variableName, classDefName);
      WriteLine(3, "query={ query }");
      WriteLine(2, "/>");
      WriteLine(1, ");");
      WriteLine(0, "}");

      ImportsPlaceholder.ImportDefault("react_lib/relay/EntityQueryRenderer");
      ImportsPlaceholder.ImportCreateDefaultFunc(model);

      WriteLine();
    }


    private void GenerateFormRelayToInternal(Entity model) {
      IEnumerable<Association> nullableAssociations = model.Associations
        .Where(x => !(x.IsMandatory || x.IsMany));

      WriteLine(0, "function relayToInternal(relay: any): {0} {", model.Name);
      WriteLine(1, "return {");
      WriteLine(2, "...relay,");

      // TODO... Not recursive at this time
      foreach (Association association in nullableAssociations) {
        Entity assocModel = association.ReferencedEntity;
        WriteLine(2, "{0}: relay.{0} || {1}(),",
          association.Name,
          ReactCodeGenerator.CreateDefaultFuncName(assocModel));

        ImportsPlaceholder.ImportCreateDefaultFunc(assocModel);
      }

      WriteLine(1, "};");
      WriteLine(0, "}");
    }


    private void GenerateFormGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model);

      WriteLine(0, "const query = graphql`");
      WriteLine(1, "query {0}InterfaceQuery($id: Int!) {", classDefName);
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