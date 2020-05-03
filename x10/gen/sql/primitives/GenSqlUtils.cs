﻿using System;
using System.Collections.Generic;
using System.Text;

namespace x10.gen.sql.primitives {
  internal static class GenSqlUtils {
    internal static Dictionary<string, string> ToDictionary(string poorMansJson) {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string[] pairs = poorMansJson.Split(";");

      foreach (string pair in pairs) {
        string[] keyValue = pair.Split("=>");
        if (keyValue.Length != 2)
          throw new Exception("Expecting format 'key => value', but got: " + pair);

        string key = keyValue[0];
        string value = keyValue[1];
        if (dictionary.ContainsKey(key))
          throw new Exception("Duplicate key: " + key);
        dictionary[key] = value;
      }

      return dictionary;
    }

    internal static double ParsePercentage(string percentage) {
      percentage = percentage.Trim();
      if (!percentage.EndsWith("%"))
        throw new Exception("Percentage expected to end with '%': " + percentage);
      double percentDouble = double.Parse(percentage.Substring(0, percentage.Length - 1));
      return percentDouble / 100.0;
    }

    internal static T GetRandom<T>(Random random, IEnumerable<T> options) where T : class, IWithProbability {
      double randomDouble = random.NextDouble();
      double cumulativeProbability = 0.0;

      foreach (T option in options) {
        cumulativeProbability += option.Probability;
        if (randomDouble < cumulativeProbability)
          return option;
      }

      return null;
    }
  }

  internal interface IWithProbability {
    double Probability { get; }
  }

}
