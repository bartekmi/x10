using System;
using System.Collections.Generic;
using System.Text;

namespace x10.utils {
    public static class EnumUtils {
        public static T? Parse<T>(string value) where T : struct {
            if (value == null)
                return null;
            Enum.TryParse(typeof(T), value, true, out object result);
            return (T?)result;
        }

        public static T TryParse<T>(string input, T defaultValue) where T : struct {
            if (Enum.TryParse<T>(input, true, out T result))
                return result;
            return defaultValue;
        }
    }
}
