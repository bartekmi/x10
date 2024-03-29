﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.model.definition;
using x10.parsing;
using x10.gen.sql.primitives;
using x10.gen.sql.parser;
using System.ComponentModel;
using System.Data;

namespace x10.gen.sql {
  internal class DataGenerationContext {
    internal List<ExternalDataFile> ExternalDataFiles = new List<ExternalDataFile>();
    private Random _random;
    private Entity _entity;
    private StaticDictionaries _staticDictionaries;

    private const string DEFAULT_DATA_FILES_ROOT = @".";  // Copied by build process

    internal static DataGenerationContext CreateContext(
        MessageBucket messages, 
        DataGenLanguageParser parser, 
        Random random, 
        Entity entity, 
        string dataFilesRoot) {

      dataFilesRoot ??= DEFAULT_DATA_FILES_ROOT;

      DataGenerationContext context = new DataGenerationContext() {
        _random = random,
        _entity = entity,
        _staticDictionaries = new StaticDictionaries(dataFilesRoot),
      };

      ModelAttributeValue sourcesValue = entity.FindAttribute(DataGenLibrary.SOURCES);
      string sources = sourcesValue?.Value as string;

      if (sources != null) {
        try {
          NodeProbabilities sourcesNode = parser.Parse(sources)
            .Children.OfType<NodeProbabilities>().SingleOrDefault();
          if (sourcesNode == null) 
            throw new Exception("Error parsing sources: " + sources);

          foreach (Node source in sourcesNode.Children) {
            string[] fileAndAlias = source.OnlyChildText.Split("AS");
            if (fileAndAlias.Length != 2)
              throw new Exception(string.Format("Expected format: 'file.csv AS alias', but got '{0}'", source.OnlyChildText));

            string path = fileAndAlias[0].Trim();
            string alias = fileAndAlias[1].Trim();

            ExternalDataFile dataFile = new ExternalDataFile() {
              Path = path,
              Probability = source.Probability,
              Alias = alias,
            };
            dataFile.Parse(dataFilesRoot);
            context.ExternalDataFiles.Add(dataFile);
          }
        } catch (Exception e) {
          messages.AddError(sourcesValue.TreeElement, e.Message);
        }
      }

      return context;
    }


    // If there is no QUANTITY specified, and there is exactly one external data file,  the intent is assumed
    // to generate a sample data row for every row of the external file
    internal bool UseFullExternalFile(out int rowCount) {
      if (!_entity.FindValue<int>(DataGenLibrary.QUANTITY, out int _) && 
          ExternalDataFiles.Count == 1) {
            rowCount = ExternalDataFiles.Single().Count;
            return true;
      }

      rowCount = 0;
      return false;
    }

    private int _externalFileIndex = 0;
    // If there is a single file and no quantity specified, we return the contents of the
    // file in consecutive order. Otherwise, we return a random row.
    internal DataFileRow GetExternalFileRow() {
      if (UseFullExternalFile(out int _)) {
        ExternalDataFile singleDataFile = ExternalDataFiles.Single();
        DataFileRow row = singleDataFile.GetRow(_externalFileIndex % singleDataFile.Count);
        _externalFileIndex++;
        return row;
      }

      ExternalDataFile dataFile = GenSqlUtils.GetRandom<ExternalDataFile>(_random, ExternalDataFiles);
      if (dataFile == null)
        return null;
      return dataFile.GetRandomRow(_random);
    }

    internal string GetRandomDictionaryEntry(string dictionaryName) {
      return _staticDictionaries.GetRandomEntry(_random, dictionaryName);
    }

    class StaticDictionaries {
      private string _dataFilesRoot;
      private readonly Dictionary<string, string[]> _dictionaries = new Dictionary<string, string[]>();

      internal StaticDictionaries(string dataFilesRoot) {
        _dataFilesRoot = dataFilesRoot;
      }

      internal string GetRandomEntry(Random random, string dictionaryName) {
        if (!_dictionaries.TryGetValue(dictionaryName, out string[] entries)) {
          string path = Path.Combine(_dataFilesRoot, dictionaryName + ".csv");
          entries = File.ReadAllLines(path);
          _dictionaries[dictionaryName] = entries;
        }

        return entries[random.Next(entries.Length)];
      }
    }
  }
}
