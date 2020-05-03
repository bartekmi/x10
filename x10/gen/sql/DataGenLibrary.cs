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

    internal const string QUANTITY = "datagen_quantity";
    internal const string SOURCES = "datagen_sources";
    internal const string PATTERN = "datagen_pattern";
    internal const string FROM_SOURCE = "datagen_from_source";
    internal const string UNIQUE = "datagen_unique";
    internal const string MIN = "datagen_min";
    internal const string MAX = "datagen_max";
    internal const string RANDOM_TEXT = "datagen_random_text";
    internal const string NO_SQL_SCHEMA = "datagen_no_sql_schema";

    private readonly static List<ModelAttributeDefinition> _attributes = new List<ModelAttributeDefinition>() {
      // Entity Level
      new ModelAttributeDefinitionAtomic() {
        Name = QUANTITY,
        Description = @"The number of rows of data to generate",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.Integer,

        Pass2Action = (messages, allEntities, allEnums, modelComponent, attributeValue) => {
          // TODO: Do not allow negative values
        },
      },
      new ModelAttributeDefinitionAtomic() {
        Name = SOURCES,
        Description = @"External source(s) for data. E.g. use: '25% => us_cities.csv AS us; 75% => cn_cities.csv as cn'",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = NO_SQL_SCHEMA,
        Description = @"By default, an SQL table and data is generated for Entites. Prevent this behavior by setting this property to True.",
        AppliesTo = AppliesTo.Entity,
        DataType = DataTypes.Singleton.Boolean,
        DefaultIfMissing = false,
      },

      // Attribute Level
      new ModelAttributeDefinitionAtomic() {
        Name = PATTERN,
        Description = @"Pattern-based randomly generated text.",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = FROM_SOURCE,
        Description = @"Extract data referring to a previously named source",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = UNIQUE,
        Description = @"Ensure that the data, if generated randomly, is unique",
        AppliesTo = AppliesTo.Attribute,
        DataType = DataTypes.Singleton.Boolean,
        DefaultIfMissing = false,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = MIN,
        Description = @"Minimum value. Either a formula or a value of same type as the attribute.",
        AppliesTo = AppliesTo.Attribute,
        DataTypeMustBeSameAsAttribute = true,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = MAX,
        Description = @"Maximum value. Either a formula or a value of same type as the attribute.",
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

    private static ModelLibrary CreateLibrary() {
      ModelLibrary library = new ModelLibrary(_attributes) {
        Name = "Data Generation Library",
      };

      return library;
    }
  }
}
