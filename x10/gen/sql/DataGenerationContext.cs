using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;

namespace x10.gen.sql {
  internal class DataGenerationContext {
    internal Dictionary<string, ExternalDataFile> ExternalDataFiles;
    internal Random Random;
    private StaticDictionaries _staticDictionaries = new StaticDictionaries();

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

    internal string GetRandomDictionaryEntry(string dictionaryName) {
      return _staticDictionaries.GetRandomEntry(Random, dictionaryName);
    }

    class StaticDictionaries {
      private Dictionary<string, string[]> _dictionaries = new Dictionary<string, string[]>();
      
      internal string GetRandomEntry(Random random, string dictionaryName) {
        if (!_dictionaries.TryGetValue(dictionaryName, out string[] entries)) {
          string path = Path.Combine(DataGenerationContext.DATA_FILES_ROOT, dictionaryName + ".csv");
          entries = File.ReadAllLines(path);
          _dictionaries[dictionaryName] = entries;
        }

        return entries[random.Next(entries.Length)];
      }
    }
  }
}
