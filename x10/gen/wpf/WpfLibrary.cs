using System;
using System.Collections.Generic;
using System.Text;

namespace x10.gen.wpf {
  // The WPF Implementation of the x10 Base Library

  // Roghly, the code-generation strategy is as follows:
  // 1. One-for-one converstion from UiComponents to WPF XAML
  // 2. Each x10 UI Component has a WPF equivalent
  // 3. x10 Attributes also have equivalents, but if not specified, the name is assumed to be same (uppercased for C#)
  // 4. WPF components may have styles applied - e.g. Heading3 is just Text with a style



  // Mappings: 
  public class WpfLibrary {
  }
}
