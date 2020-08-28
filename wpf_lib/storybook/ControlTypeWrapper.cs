using System;
using System.Collections.Generic;
using wpf_lib.lib;

namespace wpf_lib.storybook {
  internal class ControlTypeWrapper {
    private readonly Type _type;
    private TopLevelControlBase _userControl;
    private List<string> _urlFixedComponents = new List<string>();
    private List<string> _urlParameters = new List<string>();

    internal ControlTypeWrapper(Type type) {
      _type = type;
    }

    internal TopLevelControlBase GetUserControl() {
      if (_userControl == null) {
        _userControl = (TopLevelControlBase)Activator.CreateInstance(_type);
        string[] pieces = _userControl.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string piece in pieces) {
          if (piece.StartsWith("{") && piece.EndsWith("}"))
            _urlParameters.Add(piece.Substring(1, piece.Length - 2));
          else
            _urlFixedComponents.Add(piece);
        }
      }
      return _userControl;
    }

    internal bool CorrespondsToUrl(string url, out Parameters parameters) {
      GetUserControl();
      parameters = new Parameters();
      string[] pieces = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
      if (pieces.Length != _urlFixedComponents.Count + _urlParameters.Count)
        return false;

      // Check if fixed part matches
      for (int ii = 0; ii < _urlFixedComponents.Count; ii++)
        if (_urlFixedComponents[ii] != pieces[ii])
          return false;

      // Extract parameters
      for (int ii = 0; ii < _urlParameters.Count; ii++)
        parameters.Add(_urlParameters[ii], pieces[_urlFixedComponents.Count + ii]);

      return true;
    }

    public override string ToString() {
      return _type.Name;
    }
  }
}
