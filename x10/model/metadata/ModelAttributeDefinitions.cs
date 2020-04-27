using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using x10.gen.sql;
using x10.model.libraries;

namespace x10.model.metadata {

  public static class ModelAttributeDefinitions {

    public static List<ModelAttributeDefinition> All { get; private set; }

    static ModelAttributeDefinitions() {
      All = new List<ModelAttributeDefinition>();
      All.AddRange(BaseLibrary.Singleton().Attributes);
      All.AddRange(DataGenLibrary.Singleton().Attributes); // TODO: MEF-in
    }

    public static ModelAttributeDefinition Find(AppliesTo appliesTo, string name) {
      return All.SingleOrDefault(x => x.AppliesToType(appliesTo) && x.Name == name);
    }
  }
}
