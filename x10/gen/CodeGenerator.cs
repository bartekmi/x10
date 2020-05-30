using System;
using System.Collections.Generic;
using System.IO;

using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.parsing;
using x10.ui.composition;

namespace x10.gen {
  public abstract class CodeGenerator {

    public abstract void Generate(ClassDefX10 classDef);
    public abstract void Generate(Entity entity);

    protected String RootGenerateDir;
    protected AllEntities AllEntities;
    protected AllEnums AllEnums;
    protected AllUiDefinitions AllUiDefinitions;

    protected CodeGenerator(string rootGenerateDir, AllEntities allEntities, AllEnums allEnums, AllUiDefinitions allUiDefinitions) {
      RootGenerateDir = rootGenerateDir;
      AllEntities = allEntities;
      AllEnums = allEnums;
      AllUiDefinitions = allUiDefinitions;
    }

    public void Generate() {
      foreach (Entity entity in AllEntities.All)
        Generate(entity);

      foreach (ClassDefX10 classDef in AllUiDefinitions.All)
        Generate(classDef);
    }

    protected TextWriter CreateTextWriter(IParseElement element, string extension) {
      return new StreamWriter(@"C:\TEMP\Booking" + extension);
    }
  }
}
