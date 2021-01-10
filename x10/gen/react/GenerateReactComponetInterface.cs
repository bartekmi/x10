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
      GenerateQueryRenderer(classDef);
      GenerateGraphqlQuery(classDef);

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

    private void GenerateQueryRenderer(ClassDefX10 classDef) {

      WriteLine();
    }

    private void GenerateGraphqlQuery(ClassDefX10 classDef) {

      WriteLine();
    }
  }
}