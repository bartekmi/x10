using System;
using System.IO;
using System.Text.RegularExpressions;

namespace x10.utils {
  public static class DebugUtils {
    // Annotates text with the call site. This is a debugging help so we can trace the 
    // generated code to where we generated it.
    // The default value of skipLevels assumes there is on aggregator function
    // between this method and the "interesting" code we want to track.
    public static string StampWithCallerSource(string text, int skipLevels = 1, bool excludeFilename = false) {
      using StringReader reader = new StringReader(Environment.StackTrace);

      reader.ReadLine();  // Skip one level since Environment.StackTrace is own func
      for (int ii = 0; ii < skipLevels + 1; ii++)
        reader.ReadLine();

      string secondFrame = reader.ReadLine();

      string pattern = @".*/(.*):line\s(\d+)$";
      Match match = Regex.Match(secondFrame, pattern);

      string annotation = " [NO REGEX MATCH] ";
      if (match.Success) {
        string file = match.Groups[1].Value;
        string line = match.Groups[2].Value;

        annotation = excludeFilename ?
          string.Format(" [{0}] ", line) :
          string.Format(" [{0}:{1}] ", file, line);
      }      
      
      return text + annotation;
    }
  }
}