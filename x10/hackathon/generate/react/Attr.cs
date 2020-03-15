using System;

namespace x10.generate.react {

    internal abstract class Attr {
        internal string Name;
        internal abstract string Value { get; }

    }

    // Represents an Attribute on a React <Element>
    internal class AttrLit : Attr {

        private bool _translate;
        private object _value;
        internal override string Value {
            get {
                if (_translate)
                    return string.Format("{{t(\"{0}\")}}", _value);
                if (_value is String) {
                    string asString = _value.ToString();
                    if (asString.StartsWith("{") && asString.EndsWith("}"))
                        return asString;
                    return string.Format("\"{0}\"", asString);
                } else
                    return "{" + _value.ToString().ToLower() + "}";
            }
        }

        internal AttrLit(string name, object value, bool translate = false) {
            Name = name;
            _value = value;
            _translate = translate;
        }
    }

    internal class AttrExp : Attr {
        private string _expression;

        internal override string Value {
            get { return "{" + _expression + "}"; }
        }

        internal AttrExp(string name, string expression) {
            Name = name;
            _expression = expression;
        }
    }
}