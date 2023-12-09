using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using x10.model.definition;
using x10.ui.composition;
using x10.model;
using x10.gen.typescript.generate;

namespace x10.gen.typescript.placeholder {
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
        string varName = TypeScriptCodeGenerator.VariableName(entity, true);
        
        writer.WriteLine("const {0}Query = gql`", varName);
        writer.WriteLine("  query {0}_{1}Query {{", _classDef.Name, varName);
        writer.WriteLine("    entities: {0} {{", varName);
        writer.WriteLine("      id");

        // Add attributes needed for the derived "toStringRepresentation" attribute
        X10DerivedAttribute toString = entity.GetToStringRepresentationAttr();
        foreach (X10RegularAttribute regular in toString.ExtractSourceAttributes())
          writer.WriteLine("      {0}", regular.Name);


        writer.WriteLine("    }");
        writer.WriteLine("  }");
        writer.WriteLine("`;");
        writer.WriteLine();
      }
    }
  }
}