using System.Collections.Generic;
using System.Linq;
using F23.StringSimilarity;

namespace x10.parsing {
  public enum CompileMessageSeverity {
    Info,
    Warning,
    Error,
  }

  public class CompileMessage {
    public string Message { get; set; }
    public CompileMessageSeverity Severity { get; set; }
    public IParseElement ParseElement { get; set; }

    // For situations where we are expecing a value from a particular set, and no such match is found...
    public string ActualValue { get; set; }
    public IEnumerable<string> AllowedValues { get; set; }

    public override string ToString() {
      IEnumerable<string> didYouMean = DidYouMean(3);
      string didYouMeanString = null;
      if (didYouMean.Count() > 0)
        didYouMeanString = string.Format("\r\n\tDid you mean: {0}?", string.Join(", ", didYouMean));

      return string.Format("{0}:{1}:{2} - {3}: {4}{5}", 
        ParseElement?.FileInfo?.FilePath, 
        ParseElement?.Start?.LineNumber, 
        ParseElement?.Start?.CharacterPosition, 
        Severity, 
        Message,
        didYouMeanString);
    }

    public override int GetHashCode() {
      return ToString().GetHashCode();
    }

    public override bool Equals(object obj) {
      return ToString() == obj.ToString();
    }

    class ValueAndDistance {
      internal string Value;
      internal double Distance;
    }

    private IEnumerable<string> DidYouMean(int max) {
      if (ActualValue == null || AllowedValues == null)
        return new string[0];

      List<ValueAndDistance> valuesWithDistance = new List<ValueAndDistance>();
      var measurer = new Damerau(); // Number of additions, deletions, substitutions or transpositions 

      foreach (string allowed in AllowedValues) {
        valuesWithDistance.Add(new ValueAndDistance() {
          Value = allowed,
          Distance = measurer.Distance(ActualValue, allowed),
        });
      }

      return valuesWithDistance
        .OrderBy(x => x.Distance)
        .Take(max)
        .Select(x => x.Value);
    }
  }
}