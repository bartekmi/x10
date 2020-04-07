﻿using System;
using System.Collections.Generic;
using System.Text;

namespace x10.ui.composition {
  public class UiChildComponentUse : UiChild {
    // Nested Ui child components (either Component Use or model references)
    public List<UiChild> Children { get; private set; }

    public UiChildComponentUse() {
      Children = new List<UiChild>();
    }

    internal void AddChild(UiChild uiChild) {
      Children.Add(uiChild);
      // Likely, in the future, we'll need to keep track of parent. Do it here.
    }
  }
}
