using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace wpf_lib.lib.url_parsing {
  public class ParsedUrl {
    private List<string> _fixedComponents = new List<string>();
    private List<string> _parameters = new List<string>();

    public ParsedUrl(string url) {
      if (!url.StartsWith("/"))
        throw new Exception("Url must start with '/': " + url);

      string[] pieces = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
      foreach (string piece in pieces) {
        if (piece.StartsWith("{") && piece.EndsWith("}"))
          _parameters.Add(piece.Substring(1, piece.Length - 2));
        else
          _fixedComponents.Add(piece);
      }
    }

    public bool CorrespondsToUrl(string url, out Parameters parameters) {
      parameters = new Parameters();
      string[] pieces = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
      if (pieces.Length != _fixedComponents.Count + _parameters.Count)
        return false;

      // Check if fixed part matches
      for (int ii = 0; ii < _fixedComponents.Count; ii++)
        if (_fixedComponents[ii] != pieces[ii])
          return false;

      // Extract parameters
      for (int ii = 0; ii < _parameters.Count; ii++)
        parameters.Add(_parameters[ii], pieces[_fixedComponents.Count + ii]);

      return true;
    }

    public string Substitute(EntityBase model) {
      List<object> substitutions = new List<object>();
      
      foreach (string variable in _parameters) {
        PropertyInfo info = model.GetType().GetProperty(variable);
        if (info == null)
          throw new Exception(string.Format("Property '{0}' does not exist on class '{1}'",
            variable, model.GetType().Name));

        substitutions.Add(info.GetValue(model));
      }

      return "/" + string.Join("/", _fixedComponents.Concat(substitutions));
    }

    public static string Substitute(string url, EntityBase model) {
      ParsedUrl parsedUrl = new ParsedUrl(url);
      return parsedUrl.Substitute(model);
    }
  }
}
