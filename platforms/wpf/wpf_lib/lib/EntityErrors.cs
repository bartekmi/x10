using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib.lib {
  public class EntityError {
    public string Message { get; private set; }
    public string Prefix { get; private set; }
    public List<string> Fields { get; private set; }  // Some errors might affect multiple fields

    public EntityError(string message, string prefix, params string[] fields) {
      Message = message;
      Prefix = prefix;
      Fields = new List<string>(fields);
    }

    public override string ToString() {
      return string.Format("{0}: {1}", string.Join(", ", Fields), Message);
    }

    internal bool RelatesTo(string fieldWithPrefix) {
      return Fields.Any(x => Prefix + x == fieldWithPrefix);
    }
  }

  public class EntityErrors {
    public List<EntityError> Errors { get; private set; }

    // Derived
    public bool HasErrors { get { return Errors.Count > 0; } }

    public EntityErrors() {
      Errors = new List<EntityError>();
    }

    public void Add(string message, string prefix, params string[] fields) {
      Errors.Add(new EntityError(message, prefix, fields));
    }

    public IEnumerable<EntityError> ErrorsForField(string fieldWithPrefix) {
      return Errors.Where(x => x.RelatesTo(fieldWithPrefix));
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      foreach (EntityError error in Errors)
        builder.AppendLine(error.ToString());

      return builder.ToString();
    }
  }
}
