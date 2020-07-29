using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using x10.ui;
using x10.ui.composition;
using x10.ui.platform;

namespace x10.gen.wpf.codelet {
  public enum CodeletTarget {
    XamlCs,
    VM,
  }

  public static class CodeletGenerator {
    // Generate "Codelets"
    // Algorithm:
    // 1. Find all instances in document that have codelets of required target type
    // 2. Group them by PlatformClassDef
    // 3. For each group
    //    - Write a comment
    //    - For each instance
    //        - Run the code generation
    public static void Generate(WpfCodeGenerator generator, CodeletTarget target, Instance rootInstance) {
      IEnumerable<Instance> allInstances = UiUtils.ListSelfAndDescendants(rootInstance);
      IEnumerable<IGrouping<PlatformClassDef, Instance>> codeletGroups = allInstances.GroupBy(x => generator.FindPlatformClassDef(x))
        .Where(x => x.Key != null)
        .Where(x => HasCodeletForTarget(x.Key, target));

      foreach (var codeletGroup in codeletGroups) {
        PlatformClassDefWithCodelet withCodelet = (PlatformClassDefWithCodelet)codeletGroup.Key;
        generator.WriteLine(2, "// " + withCodelet.Codelet.Comment);
        foreach (Instance instance in codeletGroup) {
          Action<WpfCodeGenerator, Instance> codelet = CodeletForTarget(withCodelet, target);
          codelet(generator, instance);
        }
        generator.WriteLine();
      }

    }

    private static bool HasCodeletForTarget(PlatformClassDef classDef, CodeletTarget target) {
      return CodeletForTarget(classDef, target) != null;
    }

    private static Action<WpfCodeGenerator, Instance> CodeletForTarget(PlatformClassDef classDef, CodeletTarget target) {
      if (classDef is PlatformClassDefWithCodelet withCodeliet) {
        CodeletRecipee recipee = withCodeliet.Codelet;
        switch (target) {
          case CodeletTarget.XamlCs: return recipee.GenerateInXamlCs;
          case CodeletTarget.VM: return recipee.GenerateInVM;
          default:
            throw new Exception("Unexpected codelet target: " + target);
        }
      } else
        return null;
    }
  }
}
