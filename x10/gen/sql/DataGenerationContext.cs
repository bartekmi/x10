using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;
using x10.parsing;

namespace x10.gen.sql {
  internal class DataGenerationContext {
    internal List<ExternalDataFile> ExternalDataFiles = new List<ExternalDataFile>();
    private readonly DataGenLanguageParser _parser;
    private Random _random;
    private readonly StaticDictionaries _staticDictionaries = new StaticDictionaries();

    internal const string DATA_FILES_ROOT = @"C:\x10\x10\data";  // TODO: Move to config

    internal static DataGenerationContext CreateContext(DataGenLanguageParser parser, Random random, Entity entity) {
      DataGenerationContext context = new DataGenerationContext() {
        _random = random,
      };

      string sources = entity.FindValue<string>(DataGenLibrary.SOURCES);
      if (sources != null) {
        NodeProbabilities sourcesNode = parser.Parse(sources)
          .Children.OfType<NodeProbabilities>().SingleOrDefault();
        if (sourcesNode == null)
          throw new Exception(string.Format("Could not parse 'Probabilities Node' from '{0}'", sources));

        foreach (Node source in sourcesNode.Children) {
          string[] fileAndAlias = source.OnlyChildText.Split("AS");
          if (fileAndAlias.Length != 2)
            throw new Exception("Expected format: 'file.csv AS alias': " + source.OnlyChildText);

          string path = fileAndAlias[0].Trim();
          string alias = fileAndAlias[1].Trim();

          ExternalDataFile dataFile = new ExternalDataFile() {
            Path = path,
            Probability = source.Probability,
            Alias = alias,
          };
          dataFile.Parse(DATA_FILES_ROOT);
          context.ExternalDataFiles.Add(dataFile);
        }
      }

      return context;
    }

    internal DataFileRow GetRandomExternalFileRow() {
      ExternalDataFile dataFile = GenSqlUtils.GetRandom<ExternalDataFile>(_random, ExternalDataFiles);
      if (dataFile == null)
        return null;
      return dataFile.GetRandomRow(_random);
    }

    internal string GetRandomDictionaryEntry(string dictionaryName) {
      return _staticDictionaries.GetRandomEntry(_random, dictionaryName);
    }

    class StaticDictionaries {
      private readonly Dictionary<string, string[]> _dictionaries = new Dictionary<string, string[]>();

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
