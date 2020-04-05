using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.metadata {
  public class UiLibrary {
    public string Name { get; set; }
    public string Description { get; set; }

    public List<UiDefinition> UiDefinitions { get; set; }
  }
}
