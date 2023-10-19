using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib.lib {
  public class Parameters {
    private List<Tuple<string, string>> _parameters = new List<Tuple<string, string>>();

    internal void Add(string name, string value) {
      _parameters.Add(new Tuple<string, string>(name, value));
    }

    public string GetParameter(string name) {
      Tuple<string, string> parameter = _parameters.SingleOrDefault(x => x.Item1 == name);
      if (parameter == null)
        throw new Exception(string.Format("Parameter {0} does not exist", name));
      return parameter.Item2;
    }

    public int GetParameterAsInt(string name) {
      return int.Parse(GetParameter(name));
    }

    public string Single() {
      if (_parameters.Count != 1)
        throw new Exception("Expected single parameter, but was: " + this);

      return _parameters.Single().Item2;
    }

    public override string ToString() {
      return string.Join("; ", _parameters
        .Select(x => string.Format("{0} = {1}", x.Item1, x.Item2)));
    }
  }
}
