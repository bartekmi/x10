using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate.Language;

namespace x10.gql {
  public static class GqlParser {
    public static GqlSchema Parse(string gqlSchema) {
      DocumentNode document = Utf8GraphQLParser.Parse(gqlSchema);
      List<GqlTypeDefinition> types = new List<GqlTypeDefinition>();

      foreach (IDefinitionNode definition in document.Definitions)
        if (definition is ObjectTypeDefinitionNode typeDef)
          types.Add(ParseTypeDefinition(typeDef));

      return new GqlSchema(types);
    }

    private static GqlTypeDefinition ParseTypeDefinition(ObjectTypeDefinitionNode typeDef) {
      List<GqlField> fields = new List<GqlField>();
      foreach (FieldDefinitionNode fieldDef in typeDef.Fields) 
        fields.Add(ParseField(fieldDef));


      GqlTypeDefinition type =  new GqlTypeDefinition(fields) {
        Name = typeDef.Name.Value,
        Description = typeDef.Description?.Value,
      };

      return type;
    }

    private static GqlField ParseField(FieldDefinitionNode fieldDef) {
      List<GqlArgument> arguments = new List<GqlArgument>();
      foreach (InputValueDefinitionNode argumentDef in fieldDef.Arguments) 
        arguments.Add(ParseArgument(argumentDef));

      GqlField field = new GqlField(arguments) {
        Name = fieldDef.Name.Value,
        Description = fieldDef.Description?.Value,
        Type = ParseTypeReference(fieldDef.Type),
      };

      return field;
    }

    private static GqlTypeReference ParseTypeReference(ITypeNode fieldTypeDef) {
      GqlTypeReference type = new GqlTypeReference() {
        Name = fieldTypeDef.NamedType().Name.Value,
        IsMandatory = fieldTypeDef is NonNullTypeNode,
      };

      if (fieldTypeDef is NonNullTypeNode nonNullType) {
        type.IsMandatory = true;
        // For now, we assume that all lists are mandatory with mandatory elements
        if (nonNullType.Type.Kind == SyntaxKind.ListType) 
          type.IsMany = true;   
      }


      return type;
    }

    private static GqlArgument ParseArgument(InputValueDefinitionNode argumentDef) {
      GqlArgument argument = new GqlArgument() {
        Name = argumentDef.Name.Value,
        Description = argumentDef.Description?.Value,
        Type = ParseTypeReference(argumentDef.Type),
      };

      return argument;
    }
  }
}