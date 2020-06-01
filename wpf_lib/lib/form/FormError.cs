using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_lib.lib {
  public class FormError {
    public string Message { get; private set; }
    public List<string> Fields { get; private set; }  // Some errors might affect multiple fields

    public FormError(string message, params string[] fields) {
      Message = message;
      Fields = new List<string>(fields);
    }

    public override string ToString() {
      return string.Format("{0}: {1}", string.Join(", ", Fields), Message);
    }
  }

  public class FormErrors {
    public List<FormError> Errors { get; private set; }

    // Derived
    public bool HasErrors { get { return Errors.Count > 0; } }

    public FormErrors() {
      Errors = new List<FormError>();
    }

    public void Add(string message, params string[] fields) {
      Errors.Add(new FormError(message, fields));
    }

    public IEnumerable<FormError> ErrorsForField(string field) {
      return Errors.Where(x => x.Fields.Contains(field));
    }

    public override string ToString() {
      StringBuilder builder = new StringBuilder();

      foreach (FormError error in Errors)
        builder.AppendLine(error.ToString());

      return builder.ToString();
    }
  }
}
