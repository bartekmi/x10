﻿using System;
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

namespace wpf_lib.lib {
  public partial class EntitySelector : UserControl {

    public static readonly DependencyProperty EntityProperty = DependencyProperty.Register(
      nameof(Entity),
      typeof(EntityBase),
      typeof(EntitySelector),
      new FrameworkPropertyMetadata() {
        BindsTwoWayByDefault = true,
        DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
      }
    );
    public EntityBase Entity {
      get { return (EntityBase)GetValue(EntityProperty); }
      set { SetValue(EntityProperty, value); }
    }

    public static readonly DependencyProperty EntitiesSourceProperty = DependencyProperty.Register(
      nameof(EntitiesSource),
      typeof(IEnumerable<EntityBase>),
      typeof(EntitySelector)
    );
    public IEnumerable<EntityBase> EntitiesSource {
      get { return (IEnumerable<EntityBase>)GetValue(EntitiesSourceProperty); }
      set { SetValue(EntitiesSourceProperty, value); }
    }

    public EntitySelector() {
      InitializeComponent();
    }
  }
}
