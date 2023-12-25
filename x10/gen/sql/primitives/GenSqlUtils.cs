using System;
using System.Collections.Generic;

namespace x10.gen.sql.primitives {
  internal static class GenSqlUtils {
    internal static Dictionary<string, string> ToDictionary(string poorMansJson) {
      poorMansJson = poorMansJson.Trim();
      if (!poorMansJson.StartsWith("(") || !poorMansJson.EndsWith(")"))
        throw new Exception("Expected expression to be surrounded by parentheses: " + poorMansJson);

      poorMansJson = poorMansJson[1..^1];

      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      string[] pairs = poorMansJson.Split("|");

      foreach (string pair in pairs) {
        string[] keyValue = pair.Split("=>");
        if (keyValue.Length != 2)
          throw new Exception("Expecting format 'key => value', but got: " + pair);

        string key = keyValue[0].Trim();
        string value = keyValue[1].Trim();
        if (dictionary.ContainsKey(key))
          throw new Exception("Duplicate key: " + key);
        dictionary[key] = value;
      }

      return dictionary;
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
