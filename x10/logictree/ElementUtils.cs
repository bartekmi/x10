using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace x10.logictree {
    public static class ElementUtils {

        // Recursively execute <action>  starting from <eleemnt>
        // The trim function, if present, will stop recursion if the condition is met
        public static void Traverse<T>(
            ElementBase element,
            Action<T> action,
            Func<ElementBase, bool> trimFunc = null) where T : ElementBase {

            if (trimFunc != null && trimFunc(element))
                return;

            if (element is T)
                action((T)element);
            foreach (ElementBase child in element.Children)
                Traverse(child, action, trimFunc);
        }

        // Collect element and all descendents
        // The trim function, if present, will stop recursion if the condition is met
        public static List<T> Collect<T>(ElementBase element, Func<ElementBase, bool> trimFunc = null) where T : ElementBase {
            List<T> list = new List<T>();
            Traverse<T>(element, (x) => list.Add(x), trimFunc);
            return list;
        }
    }
}