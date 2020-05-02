using System;
using System.Collections.Generic;
using System.Text;
using x10.model.definition;
using x10.model.metadata;

namespace x10.gen.sql {
  public class DataGenLibrary {

    private static ModelLibrary _singleton;
    public static ModelLibrary Singleton() {
      if (_singleton == null)
        _singleton = CreateLibrary();
      return _singleton;
    }

    private readonly static List<ModelAttributeDefinition> _attributes = new List<ModelAttributeDefinition>() {
      // Entity Level
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_quantity",
        Description = @"The number of rows of data to generate",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.Integer,

        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          // TODO: Do not allow negative values
        },
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_sources",
        Description = @"External source(s) for data - .e.g CSV file(s)",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = SQL_DO_NOT_GENERATE,
        Description = @"By default, an SQL table and data is generated for Entites. Prevent this behavior by setting this property to True.",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.Boolean,
        DefaultIfMissing = false,
      },

      // Attribute Level
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_pattern",
        Description = @"Pattern-based randomly generated text.",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_by_percentage",
        Description = @"Percentage-based generation strategy",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_from_source",
        Description = @"Extract data referring to a previously named source",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_unique",
        Description = @"Ensure that the data, if generated randomly, is unique",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.Boolean,
        DefaultIfMissing = false,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_min",
        Description = @"Minimum value",
        AppliesTo = AppliesTo.Attribute,
        DataTypeMustBeSameAsAttribute = true,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_max",
        Description = @"Maximum value",
        AppliesTo = AppliesTo.Attribute,
        DataTypeMustBeSameAsAttribute = true,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = "datagen_random_text",
        Description = @"Generate random text based on the pattern: 'X to Y words' - e.g. '5 to 30 words'",
        AppliesTo = AppliesTo.Attribute,
        DataTypeMustBeSameAsAttribute = true,
      },
    };

    public const string SQL_DO_NOT_GENERATE = "datagen_no_sql_schema";

    private static ModelLibrary CreateLibrary() {
      ModelLibrary library = new ModelLibrary(_attributes) {
        Name = "Data Generation Library",
      };

      return library;
    }
  }
}
