using System;
using System.Collections.Generic;
using System.Text;
using x10.gen.sql.primitives;
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
    internal const string CAPITALIZATION = "datagen_capitalization";
    internal const string PROBABILITY = "datagen_probability";

    private static readonly DataType RANGE_DATA_TYPE = new DataType() {
      Name = "SqlRange",
      Description = "Range of two integers",
      ParseFunction = (x) => new ParseResult(SqlRange.Parse(x)),
      Examples = "7, 0..2, 4..8"
    };

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
        SearchInheritanceTree = true,
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
        AppliesTo = AppliesTo.RegularAttribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = FROM_SOURCE,
        Description = @"Extract data referring to a previously named source",
        AppliesTo = AppliesTo.RegularAttribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = UNIQUE,
        Description = @"Ensure that the data, if generated randomly, is unique",
        AppliesTo = AppliesTo.RegularAttribute,
        DataType = DataTypes.Singleton.Boolean,
        DefaultIfMissing = false,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = MIN,
        Description = @"Minimum value. Either a formula or a value of same type as the attribute.",
        AppliesTo = AppliesTo.RegularAttribute,
        DataTypeMustBeSameAsAttribute = true,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = MAX,
        Description = @"Maximum value. Either a formula or a value of same type as the attribute.",
        AppliesTo = AppliesTo.RegularAttribute,
        DataTypeMustBeSameAsAttribute = true,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = RANDOM_TEXT,
        Description = @"Generate random text based on the pattern: 'x..y <words|sentences|paragraphs>' - e.g. '5..30 words'",
        AppliesTo = AppliesTo.RegularAttribute,
        DataType = DataTypes.Singleton.String,
      },
      new ModelAttributeDefinitionAtomic() {
        Name = CAPITALIZATION,
        Description = @"Controls capitalization of generated text",
        AppliesTo = AppliesTo.RegularAttribute,
        DataType = new DataTypeEnum("DataGenCapitalization", new string[] {"allCaps", "allDown", "wordCaps" }),
      },

      // Association Level
      new ModelAttributeDefinitionAtomic() {
        Name = QUANTITY,
        Description = @"The number of rows of data to generate for this owned association. Either an exact quantity like '5' or a range like 0..2",
        AppliesTo = AppliesTo.Association,
        DataType = RANGE_DATA_TYPE,
      },
      // TODO: In the future, generalize this for all optional values
      new ModelAttributeDefinitionAtomic() {
        Name = PROBABILITY,
        Description = @"For single, non-mandatory associations, if present, determines the probability that a child entity will be generated. Must be between 0.0 and 1.0 inclusive.",
        AppliesTo = AppliesTo.Association,
        DataType = DataTypes.Singleton.Float,
        ValidationFunction = (messages, scalarNode, modelComponent, appliesTo) => {
          double probability = double.Parse(scalarNode.Value.ToString());
          if (probability < 0.0 || probability > 1.0)
            messages.AddError(scalarNode, "Probability most be between 0.0 and 1.0 inclusive.");

          if (modelComponent is Association assoc)
            if (assoc.IsMany || assoc.IsMandatory)
              messages.AddError(scalarNode, "Probability can only be applied to Single, Non-Mandatory Associations.");
        }
      },
    };

    private static ModelLibrary CreateLibrary() {
      ModelLibrary library = new ModelLibrary(_attributes) {
        Name = "Data Generation Library",
      };

      DataTypes.Singleton.AddDataType(RANGE_DATA_TYPE);

      return library;
    }
  }
}
