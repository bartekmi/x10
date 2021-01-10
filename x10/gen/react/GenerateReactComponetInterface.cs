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

    // Model is guaranteed to be non-null
    private void GenerateInterface(ClassDefX10 classDef, Entity model) {
      Begin(classDef.XmlElement.FileInfo, "Interface.jsx");

      GenerateFileHeader();
      GenerateImports(classDef, model);
      GenerateWrapper(classDef, model);
      GenerateQueryRenderer(classDef, model);
      GenerateGraphqlQuery(classDef, model);

      End();
    }

    private void GenerateImports(ClassDefX10 classDef, Entity model) {
      ImportsPlaceholder = new ImportsPlaceholder();
      AddPlaceholder(ImportsPlaceholder);

      ImportsPlaceholder.ImportReact();
      ImportsPlaceholder.Import("graphql", "react-relay");
      ImportsPlaceholder.Import("QueryRenderer", "react-relay");
      ImportsPlaceholder.ImportDefault("environment");

      ImportsPlaceholder.ImportDefault(classDef);
      if (model != null) {
        ImportsPlaceholder.ImportType(model);
        ImportsPlaceholder.ImportCreateDefaultFunc(model);
      }

      WriteLine();
    }

    private void GenerateWrapper(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string modelName = model.Name;
      string variableName = VariableName(model, false);
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
    }

    private void GenerateQueryRenderer(ClassDefX10 classDef, Entity model) {
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
      string variableName = VariableName(model, false);

      WriteLine(0,
@"export default function {0}Interface(props: Props): React.Node { 
  return (
    <EntityQueryRenderer
      match={ props.match }
      createDefaultFunc={ {1} }
      createComponentFunc={ ({2}) => <{0}Wrapper {2}={ {2} }/> }
      query={ query }
    />
  );
}}",
      classDefName,       // Index 0
      createDefaultFunc,  // Index 1
      variableName);      // Index 2

      ImportsPlaceholder.ImportDefault("react_lib/relay/EntityQueryRenderer");
      ImportsPlaceholder.ImportCreateDefaultFunc(model);

      WriteLine();
    }

    private void GenerateGraphqlQuery(ClassDefX10 classDef, Entity model) {

      string classDefName = classDef.Name;
      string variableName = VariableName(model, false);

      WriteLine(0, "const query = graphql`");
      WriteLine(1, "query {0}InterfaceQuery($id: Int!) {", classDefName);
      WriteLine(2, "entity: {0}(id: $id) {", variableName);

      GenerateGraphqlQueryRecursively(2, model);

      // Trailing brace of level 2 was written by recurse function
      WriteLine(1, "}");
      WriteLine(0, "`;");

      WriteLine();
    }

    private void GenerateGraphqlQueryRecursively(int level, Entity model) {
      WriteLine(level + 1, "id");
      WriteLine(level + 1, "dbid");

      foreach (X10RegularAttribute attribute in model.RegularAttributes) 
        WriteLine(level + 1, attribute.Name);
      
      foreach (Association association in model.Associations) {
        WriteLine(level + 1, "{0} {", association.Name);
        GenerateGraphqlQueryRecursively(level + 1, association.ReferencedEntity);
      }

      WriteLine(level, "}");  // Only the trailing brace is written by this method
    }
  }
}