using System;
using System.Collections.Generic;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;

namespace x10.gen.sql {
  internal class DataGenerationContext {
    internal Dictionary<string, ExternalDataFile> ExternalDataFiles;
    internal Random Random;

    internal const string DATA_FILES_ROOT = @"C:\x10\x10\data";  // TODO: Move to config

    internal static DataGenerationContext CreateContext(Random random, Entity entity) {
      DataGenerationContext context = new DataGenerationContext() {
        Random = random,
      };

      object sourcesString = entity.FindValue(DataGenLibrary.SOURCES);
      if (sourcesString != null) {
        Dictionary<string, string> sources = GenSqlUtils.ToDictionary(sourcesString.ToString());

        context.ExternalDataFiles = new Dictionary<string, ExternalDataFile>();
        foreach (var source in sources) {
          string[] fileAndAlias = source.Value.Split("AS");
          if (fileAndAlias.Length != 2)
            throw new Exception("Expected format: 'file.csv AS alias': " + source.Value);

          string path = fileAndAlias[0].Trim();
          string alias = fileAndAlias[1].Trim();

          ExternalDataFile dataFile = new ExternalDataFile() { 
            Path = path,
            Probability = GenSqlUtils.ParsePercentage(source.Key),
            Alias = alias,
          };
          dataFile.Parse(DATA_FILES_ROOT);
          context.ExternalDataFiles.Add(alias, dataFile);
        }
      }

      return context;
    }

    internal DataFileRow GetRandomRow() {
      ExternalDataFile dataFile = GenSqlUtils.GetRandom<ExternalDataFile>(Random, ExternalDataFiles.Values);
      if (dataFile == null)
        return null;
      return dataFile.GetRandomRow(Random);
    }
  }
}
