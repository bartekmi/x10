using System.ComponentModel;
using Newtonsoft.Json;

namespace wpf_lib.lib {
  public abstract class NotificationObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    
    [JsonIgnore]
    public NotificationObject Owner { get; set; }

    protected NotificationObject() {
      PropertyChanged = delegate { };
    }
    public virtual void RaisePropertyChanged(string name) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
      if (Owner != null)
        Owner.RaisePropertyChanged(null);
    }
  }
}
