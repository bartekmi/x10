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

    private void GenerateInterface(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, "Interface.jsx");

      Entity model = classDef.ComponentDataModel;

      GenerateFileHeader();
      GenerateImports(classDef, model);
      GenerateFormWrapper(classDef);
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
    }

    private void GenerateFormWrapper(ClassDefX10 classDef) {

    }

    private void GenerateQueryRenderer(ClassDefX10 classDef) {

    }

    private void GenerateGraphqlQuery(ClassDefX10 classDef) {

    }
  }
}