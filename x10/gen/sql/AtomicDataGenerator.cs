using System;
using System.Collections.Generic;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;
using x10.model.metadata;
using x10.parsing;
using x10.utils;

namespace x10.gen.sql {
  internal class AtomicDataGenerator {
    private const int DEFAULT_INT_MIN = 1;
    private const int DEFAULT_INT_MAX = 10;
    private const double DEFAULT_FLOAT_MIN = 1.0;
    private const double DEFAULT_FLOAT_MAX = 10.0;

    private const int DEFAULT_DATE_OFFSET_DAYS_MIN = -20;
    private const int DEFAULT_DATE_OFFSET_DAYS_MAX = +5;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN = -20.0;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX = +5.0;

    private readonly MessageBucket _messages;
    private readonly DataGenLanguageParser _parser;

    internal AtomicDataGenerator(MessageBucket messages) {
      _messages = messages;
      _parser = new DataGenLanguageParser(messages);
    }

    internal MemberAndValue Generate(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {
      object value = null;

      try {
        value = GenerateWithException(random, context, x10Attr, externalRow);
      } catch (Exception e) {
        _messages.AddError(x10Attr.TreeElement, e.Message);
      }

      return new MemberAndValue() {
        Member = x10Attr,
        Value = value,
      };
    }

    internal object GenerateWithException(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {
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
      else if (x10Attr.DataType is DataTypeEnum dataTypeEnum) {
        value = GenerateForString(random, context, x10Attr, externalRow);
        if (value == null) {
          if (x10Attr.IsMandatory) {
            int index = random.Next(dataTypeEnum.EnumValues.Count);
            value = dataTypeEnum.EnumValues[index].Value;
          }
        } else
          if (!dataTypeEnum.HasEnumValue(value))
          _messages.AddError(x10Attr.TreeElement, "Invalide Enum value '{0}' generated for Enum '{1}'",
            value, dataTypeEnum.Name);
      } else
        _messages.AddError(x10Attr.TreeElement, "Unknown data type: " + x10Attr.DataType.Name);

      return value;
    }

    #region Generate For String
    private string GenerateForString(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {

      string fromSource = x10Attr.FindValue<string>(DataGenLibrary.FROM_SOURCE, out ModelAttributeValue fromSourceValue);
      string pattern = x10Attr.FindValue<string>(DataGenLibrary.PATTERN, out ModelAttributeValue patternValue);
      bool capitalize = x10Attr.FindValue<bool>(DataGenLibrary.CAPITALIZE);

      string text = null;

      if (fromSource != null)
        try {
          text = GenerateFromSource(externalRow, fromSource);
        } catch (Exception e) {
          _messages.AddError(fromSourceValue.TreeElement, e.Message);
        }
      else if (pattern != null)
        try {
          text = GenerateFromPattern(random, context, pattern);
        } catch (Exception e) {
          _messages.AddError(patternValue.TreeElement, e.Message);
        }

      if (capitalize)
        text = NameUtils.Capitalize(text);

      return text;
    }

    #region Generate From Pattern
    private string GenerateFromPattern(Random random, DataGenerationContext context, string pattern) {
      Node node = _parser.Parse(pattern);
      StringBuilder builder = new StringBuilder();

      GenerateForNode(random, context, builder, node);

      return builder.ToString();
    }

    private void GenerateForNode(Random random, DataGenerationContext context, StringBuilder builder, Node node) {
      if (node is NodeConcat nodeConcat) {
        GenerateForNodeConcat(random, context, builder, nodeConcat);
      } else if (node is NodeText nodeText) {
        GenerateForNodeText(random, context, builder, nodeText);
      } else if (node is NodeProbabilities nodeProbabilities) {
        GenerateForNodeProbabilities(random, context, builder, nodeProbabilities);
      } else
        throw new Exception("Unexpected node type: " + node.GetType().Name);
    }

    private void GenerateForNodeConcat(Random random, DataGenerationContext context, StringBuilder builder, NodeConcat nodeConcat) {
      foreach (Node child in nodeConcat.Children)
        GenerateForNode(random, context, builder, child);
    }

    private void GenerateForNodeText(Random random, DataGenerationContext context, StringBuilder builder, NodeText nodeText) {
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

    private void GenerateForNodeProbabilities(Random random, DataGenerationContext context, StringBuilder builder, NodeProbabilities nodeProbabilities) {
      Node selectedNode = GenSqlUtils.GetRandom(random, nodeProbabilities.Children);
      if (selectedNode == null)
        return;
      GenerateForNode(random, context, builder, selectedNode);
    }

    private void GenerateDictionaryReplace(DataGenerationContext context, StringBuilder builder, string dictionaryName) {
      string value = context.GetRandomDictionaryEntry(dictionaryName.Trim());
      builder.Append(value);
    }

    private void GenerateCharacterReplace(Random random, StringBuilder builder, string pattern) {
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
    private string GenerateFromSource(DataFileRow externalRow, string rulesAsString) {
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
