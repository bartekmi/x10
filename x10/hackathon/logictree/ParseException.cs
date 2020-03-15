using System;

namespace x10.logictree {
    public class ParseException : Exception {

        public string Path { get; private set; }

        public ParseException(string path, string message) : base(message) {
            Path = path;
        }
    }
}