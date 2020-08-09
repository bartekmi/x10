using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace wpf_lib.lib {
  public abstract class ViewModelBaseMany<T> : ViewModelBase where T : EntityBase {

    public IEnumerable<T> Model {
      get { return (IEnumerable<T>)ModelUntyped; }
      set {
        ModelUntyped = value;
      }
    }
  }
}
