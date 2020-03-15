using System.Text;

namespace x10.utils {
    public class NameUtils {
        public static string Capitalize(string text) {
            if (text == null || text.Length == 0)
                return text;
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public static string Uncapitalize(string text) {
            if (text == null || text.Length == 0)
                return text;
            return char.ToLower(text[0]) + text.Substring(1);
        }

        // myName => My Name
        public static string ToHuman(string text) {
            StringBuilder builder = new StringBuilder();
            char? previous = null;

            foreach (char c in text) {
                if (previous != null && char.IsLower(previous.Value) && char.IsUpper(c))
                    builder.Append(" ");
                builder.Append(previous == null ? char.ToUpper(c) : c);
                previous = c;
            }

            return builder.ToString();
        }

    }
}