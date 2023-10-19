using System;
using System.Collections.Generic;
using wpf_lib.lib;
using wpf_lib.lib.url_parsing;

namespace wpf_lib.storybook {
  internal class ControlTypeWrapper {
    private readonly Type _type;
    private TopLevelControlBase _userControl;
    private ParsedUrl _url;

    internal ControlTypeWrapper(Type type) {
      _type = type;
    }

    internal TopLevelControlBase GetUserControl() {
      if (_userControl == null) {
        _userControl = (TopLevelControlBase)Activator.CreateInstance(_type);
        _url = new ParsedUrl(_userControl.Url);
      }
      return _userControl;
    }

    internal bool CorrespondsToUrl(string url, out Parameters parameters) {
      GetUserControl();
      return _url.CorrespondsToUrl(url, out parameters);
    }

    public override string ToString() {
      return _type.Name;
    }
  }
}
