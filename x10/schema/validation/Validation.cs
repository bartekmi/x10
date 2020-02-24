using System;
using System.Linq;
using System.Text;

namespace x10.schema.validation {
    public abstract class Validation {
        public string ErrorMessage { get; set; }

        // Name of function to invoke for validation
        // The function should return TRUE if validation passes and FALSE if it fails
        // At this time, we assume one Function is sufficient for all languages
        public string Function { get; set; }
    }
}