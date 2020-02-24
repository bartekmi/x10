using System.Collections.Generic;

namespace x10.error {
    public class ErrorBucket {
        public List<Error> Errors { get; private set; }
        private string _path;

        public void SetPath(string path) {
            _path = path;
        }

        // Derived
        public bool HasErrors { get { return Errors.Count > 0; } }

        public ErrorBucket() {
            Errors = new List<Error>();
        }

        public void Add(Error error) {
            if (error.Path == null)
                error.Path = _path;
            Errors.Add(error);
        }

        public void Add(ErrorBucket bucket) {
            Errors.AddRange(bucket.Errors);
        }
    }
}