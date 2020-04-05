using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.composition {
  public class UiChildComponentUse : UiChild {
    // This is analogous to Path in WPF... it is a dot-separated list of
    // model members descending down the graph of model objects
    public string Path { get; set; }

    // Nested Ui child components (either Component Use or model references)
    public List<UiChild> Children { get; private set; }

    public UiChildComponentUse() {
      Children = new List<UiChild>();
    }
  }
}
