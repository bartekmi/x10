using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using x10.model.definition;
using x10.ui.composition;
using x10.model;
using x10.gen.react.generate;

namespace x10.gen.react.placeholder {
  public class GqlPlaceholder : CodeGenerator.Output {

    private ClassDefX10 _classDef;
    private HashSet<Entity> _associationEditors = new HashSet<Entity>();

    internal GqlPlaceholder(ClassDefX10 classDef) {
      _classDef = classDef;
    }

    internal void AddGqlQueryForAssociationEditor(Entity entity) {
      _associationEditors.Add(entity);
    }

    public override void Write(TextWriter writer)  {
      foreach (Entity entity in _associationEditors.OrderBy(x => x.Name)) {
        string varName = ReactCodeGenerator.VariableName(entity, true);
        writer.WriteLine("const {0}Query = graphql`", varName);
        writer.WriteLine("  query {0}_{1}Query {{", _classDef.Name, varName);
        writer.WriteLine("    entities: {0} {{", varName);
        writer.WriteLine("      id");
        writer.WriteLine("      toStringRepresentation");
        writer.WriteLine("    }");
        writer.WriteLine("  }");
        writer.WriteLine("`;");
        writer.WriteLine();
      }
    }
  }
}