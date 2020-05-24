using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_sample.lib {
  public partial class EntitySelector : UserControl {

    public static readonly DependencyProperty EntityProperty = DependencyProperty.Register(
      nameof(Entity),
      typeof(EntityBase),
      typeof(EntitySelector)
    );
    public EntityBase Entity {
      get { return (EntityBase)GetValue(EntityProperty); }
      set { SetValue(EntityProperty, value); }
    }

    public EntitySelector() {
      InitializeComponent();
    }
  }
}
