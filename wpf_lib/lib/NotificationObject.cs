﻿using System.ComponentModel;

namespace wpf_lib.lib {
  public abstract class NotificationObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    protected NotificationObject() {
      PropertyChanged = delegate { };
    }
    public virtual void RaisePropertyChanged(string name) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}