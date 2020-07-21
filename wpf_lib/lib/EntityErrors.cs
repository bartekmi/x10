using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib.lib {
  public class EntityError {
    public string Message { get; private set; }
    public List<string> Fields { get; private set; }  // Some errors might affect multiple fields

    public EntityError(string message, params string[] fields) {
      Message = message;
      Fields = new List<string>(fields);
    }

    public override string ToString() {
      return string.Format("{0}: {1}", string.Join(", ", Fields), Message);
    }
  }

  public class EntityErrors {
    public List<EntityError> Errors { get; private set; }

    // Derived
    public bool HasErrors { get { return Errors.Count > 0; } }

    public EntityErrors() {
      Errors = new List<EntityError>();
    }

    public void Add(string message, params string[] fields) {
      Errors.Add(new EntityError(message, fields));
    }

    public IEnumerable<EntityError> ErrorsForField(string field) {
      return Errors.Where(x => x.Fields.Contains(field));
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      foreach (EntityError error in Errors)
        builder.AppendLine(error.ToString());

      return builder.ToString();
    }
  }
}
