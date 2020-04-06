using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.model.definition;
using x10.parsing;

namespace x10.compiler {
  // TODO: follow the pattern of UI - move this into the "All" containers
  internal static class UniquenessChecker {
    internal static void Check(string uniqueAttribute,
      IEnumerable<IAcceptsModelAttributeValues> elements,
      MessageBucket messages,
      string errorMessageWithPlaceholder) {

      var collisions = elements.GroupBy(x => AttributeUtils.FindValue(x, uniqueAttribute))
        .Where(g => g.Count() > 1);

      foreach (var collision in collisions) {
        foreach (IAcceptsModelAttributeValues element in collision) {
          if (collision.Key == null)
            continue;

          ModelAttributeValue attribute = AttributeUtils.FindAttribute(element, uniqueAttribute);
          messages.AddError(attribute.TreeElement,
            String.Format(errorMessageWithPlaceholder, collision.Key));
        }
      }
    }
  }
}
