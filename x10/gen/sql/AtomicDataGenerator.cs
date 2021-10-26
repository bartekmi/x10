using System;
using System.Collections.Generic;
using System.Text;

using NLipsum.Core;

using x10.utils;
using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using x10.gen.sql.primitives;
using x10.gen.sql.parser;

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
      } else if (x10Attr.DataType == DataTypes.Singleton.Boolean) {
        value = random.Next(2) == 0;
      } else if (x10Attr.DataType == DataTypes.Singleton.Date) {
        if (objMin is DateTime min && objMax is DateTime max) {
          TimeSpan span = max - min;
          int offsetDays = random.Next(span.Days + 1);
          value = min + TimeSpan.FromDays(offsetDays);
        } else {
          int offsetDays = random.Next(DEFAULT_DATE_OFFSET_DAYS_MIN, DEFAULT_DATE_OFFSET_DAYS_MAX + 1);
          value = DateTime.Today.AddDays(offsetDays);
        }
      } else if (x10Attr.DataType == DataTypes.Singleton.Time) {
        double dayFraction = random.NextDouble();
        long ticks = (long)(dayFraction * 24 * 3600 * 10000000);
        value = new TimeSpan(ticks);
      } else if (x10Attr.DataType == DataTypes.Singleton.Timestamp) {
        // For date, min/max is offset from today's date
        // TODO... This is wrong! min/max will never be double - they are constrained to be same type as field
        double min = objMin is double _min ? _min : DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN;
        double max = objMax is double _max ? _max : DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX;

        double offsetDays = random.NextDouble() * (max - min) + min;
        value = DateTime.Now.AddDays(offsetDays);
      } else if (x10Attr.DataType == DataTypes.Singleton.String)
        value = GenerateForString(random, context, x10Attr, externalRow);
      else if (x10Attr.DataType is DataTypeEnum dataTypeEnum) {
        value = GenerateForString(random, context, x10Attr, externalRow);
        if (string.IsNullOrEmpty(value?.ToString())) {
          if (x10Attr.IsMandatory) {
            int index = random.Next(dataTypeEnum.EnumValues.Count);
            value = dataTypeEnum.EnumValues[index].Value;
          }
        } else
          if (!dataTypeEnum.HasEnumValue(value))
          _messages.AddError(x10Attr.TreeElement, "Invalid Enum value '{0}' generated for Enum '{1}'",
            value, dataTypeEnum.Name);
      } else
        _messages.AddError(x10Attr.TreeElement, "Unknown data type: " + x10Attr.DataType.Name);

      if (value == null && x10Attr is X10RegularAttribute regular && regular.DefaultValue != null)
        value = regular.DefaultValue;

      return value;
    }

    #region Generate For String
    private string GenerateForString(Random random, DataGenerationContext context, X10Attribute x10Attr, DataFileRow externalRow) {

      ModelAttributeValue attrValue = null;
      string fromSource = x10Attr.FindValue<string>(DataGenLibrary.FROM_SOURCE, out attrValue);
      string pattern = x10Attr.FindValue<string>(DataGenLibrary.PATTERN, out attrValue);
      string randomTextSpecs = x10Attr.FindValue<string>(DataGenLibrary.RANDOM_TEXT, out attrValue);

      string capitalization = x10Attr.FindValue<string>(DataGenLibrary.CAPITALIZATION);

      string text = null;

      try {
        if (fromSource != null)
          text = GenerateFromSource(externalRow, fromSource);
        else if (pattern != null)
          text = GenerateFromPattern(random, context, pattern);
        else if (randomTextSpecs != null)
          text = GenerateRandomText(random, randomTextSpecs);
      } catch (Exception e) {
        var treeElement = attrValue == null ? x10Attr.TreeElement : attrValue.TreeElement;
        _messages.AddError(treeElement, e.Message);
      }

      if (capitalization == "wordCaps")
        text = NameUtils.Capitalize(text);
      else if (capitalization == "allCaps")
        text = text?.ToUpper();
      if (capitalization == "allDown")
        text = text?.ToLower();

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
          case 'N':
            txC = (char)(random.Next(9) + '1'); // Non-zero digit
            break;
          case 'D':
            txC = (char)(random.Next(10) + '0');  // Digit
            break;
          case 'L':
            txC = (char)(random.Next(26) + 'A');  // Letter
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
    
    #region Generate Random Text
    private string GenerateRandomText(Random random, string randomTextSpecs) {
      string message = "The format must be something like '30..50 <words|sentences|paragraphs>'";

      string[] pieces = randomTextSpecs.Split(' ');
      if (pieces.Length != 2)
        throw new Exception(message);

      SqlRange range = SqlRange.Parse(pieces[0]);
      if (range == null)
        throw new Exception(message);


      Features? features = EnumUtils.Parse<Features>(pieces[1]);
      if (features == null)
        throw new Exception(message);

      LipsumGenerator generator = new LipsumGenerator();
      int count = range.GetRandom(random);
      string text = null;

      switch (features.Value) {
        case Features.Words:
          string[] words = generator.GenerateWords(count);
          text = string.Join(" ", words);
          break;
        case Features.Sentences:
          string[] sentences = generator.GenerateSentences(count, Sentence.Medium);
          text = string.Join(" ", sentences);
          break;
        case Features.Paragraphs:
          string[] paragraphs = generator.GenerateParagraphs(count, Paragraph.Short);
          text = string.Join("\r\n\r\n", paragraphs);
          text += ".";
          break;
      }

      return text;
    }
    #endregion
    #endregion
  }
}
