using System;
using System.Collections.Generic;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;
using x10.model.metadata;
using x10.utils;

namespace x10.gen.sql {
  internal static class AtomicDataGenerator {
    private const int DEFAULT_INT_MIN = 1;
    private const int DEFAULT_INT_MAX = 10;
    private const double DEFAULT_FLOAT_MIN = 1.0;
    private const double DEFAULT_FLOAT_MAX = 10.0;

    private const int DEFAULT_DATE_OFFSET_DAYS_MIN = -20;
    private const int DEFAULT_DATE_OFFSET_DAYS_MAX = +5;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN = -20.0;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX = +5.0;

    internal static MemberAndValue Generate(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {
      object objMin = x10Attr.FindValue(DataGenLibrary.MIN);
      object objMax = x10Attr.FindValue(DataGenLibrary.MAX);

      object value = null;
      if (x10Attr.DataType == DataTypes.Singleton.Integer) {
        int min = objMin is int _min ? _min : DEFAULT_INT_MIN;
        int max = objMax is int _max ? _max : DEFAULT_INT_MAX;

        value = random.Next(min, max + 1);
      } else if (x10Attr.DataType == DataTypes.Singleton.Float) {
        double min = objMin is double _min ? _min : DEFAULT_FLOAT_MIN;
        double max = objMax is double _max ? _max : DEFAULT_FLOAT_MAX;

        value = random.NextDouble() * (max - min) + min;
      } else if (x10Attr.DataType == DataTypes.Singleton.Date) {
        // For date, min/max is offset from today's date
        int min = objMin is int _min ? _min : DEFAULT_DATE_OFFSET_DAYS_MIN;
        int max = objMax is int _max ? _max : DEFAULT_DATE_OFFSET_DAYS_MAX;

        int offsetDays = random.Next(min, max + 1);
        value = DateTime.Today.AddDays(offsetDays);
      } else if (x10Attr.DataType == DataTypes.Singleton.Timestamp) {
        // For date, min/max is offset from today's date
        double min = objMin is double _min ? _min : DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN;
        double max = objMax is double _max ? _max : DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX;

        double offsetDays = random.NextDouble() * (max - min) + min;
        value = DateTime.Now.AddDays(offsetDays);
      } else if (x10Attr.DataType == DataTypes.Singleton.String)
        value = GenerateForString(random, context, x10Attr, externalRow);

      return new MemberAndValue() {
        Member = x10Attr,
        Value = value,
      };
    }

    #region Generate For String
    private static string GenerateForString(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {
      string fromSource = x10Attr.FindValue<string>(DataGenLibrary.FROM_SOURCE);
      string pattern = x10Attr.FindValue<string>(DataGenLibrary.PATTERN);
      bool capitalize = x10Attr.FindValue<bool>(DataGenLibrary.CAPITALIZE);

      string text = null;

      if (fromSource != null)
        text = GenerateFromSource(externalRow, fromSource);
      else if (pattern != null)
        text = GenerateFromPattern(random, context, pattern);

      if (capitalize)
        text = NameUtils.Capitalize(text);

      return text;
    }

    #region Generate From Pattern
    private static string GenerateFromPattern(Random random, DataGenerationContext context, string pattern) {
      Node node = DataGenLanguageParser.Parse(pattern);
      StringBuilder builder = new StringBuilder();

      GenerateForNode(random, context, builder, node);

      return builder.ToString();
    }

    private static void GenerateForNode(Random random, DataGenerationContext context, StringBuilder builder, Node node) {
      if (node is NodeConcat nodeConcat) {
        GenerateForNodeConcat(random, context, builder, nodeConcat);
      } else if (node is NodeText nodeText) {
        GenerateForNodeText(random, context, builder, nodeText);
      } else if (node is NodeProbabilities nodeProbabilities) {
        GenerateForNodeProbabilities(random, context, builder, nodeProbabilities);
      } else
        throw new Exception("Unexpected node type: " + node.GetType().Name);
    }

    private static void GenerateForNodeConcat(Random random, DataGenerationContext context, StringBuilder builder, NodeConcat nodeConcat) {
      foreach (Node child in nodeConcat.Children)
        GenerateForNode(random, context, builder, child);
    }

    private static void GenerateForNodeText(Random random, DataGenerationContext context, StringBuilder builder, NodeText nodeText) {
      if (nodeText.Delimiter == null)
        builder.Append(nodeText.Text);
      else {
        DelimiterType type = nodeText.Delimiter.Type;
        switch (type) {
          case DelimiterType.CharacterReplace:
            GenerateCharacterReplace(random, builder, nodeText.Text);
            break;
          case DelimiterType.DictionaryReplace:
            GenerateDictionaryReplace(context, builder, nodeText.Text);
            break;
        }
      }
    }

    private static void GenerateForNodeProbabilities(Random random, DataGenerationContext context, StringBuilder builder, NodeProbabilities nodeProbabilities) {
      Node selectedNode = GenSqlUtils.GetRandom(random, nodeProbabilities.Children);
      GenerateForNode(random, context, builder, selectedNode);
    }

    private static void GenerateDictionaryReplace(DataGenerationContext context, StringBuilder builder, string dictionaryName) {
      string value = context.GetRandomDictionaryEntry(dictionaryName.Trim());
      builder.Append(value);
    }

    private static void GenerateCharacterReplace(Random random, StringBuilder builder, string pattern) {
      foreach (char c in pattern) {
        char txC = c;

        switch (c) {
          case 'D':
            txC = (char)(random.Next(10) + '0');
            break;
          case 'L':
            txC = (char)(random.Next(26) + 'A');
            break;
        }

        builder.Append(txC);
      }
    }
    #endregion

    #region Generate From Source
    private static string GenerateFromSource(DataFileRow externalRow, string rulesAsString) {
      if (externalRow == null)
        return null;

      Dictionary<string, string> rules = GenSqlUtils.ToDictionary(rulesAsString);
      if (rules == null)
        throw new Exception("Invalid from_source format: " + rulesAsString);


      string alias = externalRow.Owner.Alias;
      if (!rules.TryGetValue(alias, out string field))
        throw new Exception(string.Format("Alias '{0}' not found in from_source rules: {1}.", alias, rulesAsString));

      if (NameUtils.IsQuoted(field))
        return NameUtils.StripQuotes(field);

      return externalRow.GetValue(field);
    }
    #endregion
    #endregion
  }
}
