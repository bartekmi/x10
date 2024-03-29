﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace wpf_lib.lib {
  [ContentProperty(nameof(Children))]
  public partial class EditElementWrapper : UserControl {
    public static readonly DependencyPropertyKey ChildrenProperty = DependencyProperty.RegisterReadOnly(
        nameof(Children),
        typeof(UIElementCollection),
        typeof(EditElementWrapper),
        new PropertyMetadata());
    public UIElementCollection Children {
      get { return (UIElementCollection)GetValue(ChildrenProperty.DependencyProperty); }
      private set { SetValue(ChildrenProperty, value); }
    }

    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(
        nameof(Label),
        typeof(string),
        typeof(EditElementWrapper)
      );
    public string Label {
      get { return (string)GetValue(LabelProperty); }
      set { SetValue(LabelProperty, value); }
    }

    public static readonly DependencyProperty IsMandatoryProperty = DependencyProperty.Register(
        nameof(IsMandatory),
        typeof(bool),
        typeof(EditElementWrapper)
      );

    public bool IsMandatory {
      get { return (bool)GetValue(IsMandatoryProperty); }
      set { SetValue(IsMandatoryProperty, value); }
    }

    public static readonly DependencyProperty MyToolTipProperty = DependencyProperty.Register(
        nameof(MyToolTip),
        typeof(string),
        typeof(EditElementWrapper)
      );
    public string MyToolTip {
      get { return (string)GetValue(MyToolTipProperty); }
      set { SetValue(MyToolTipProperty, value); }
    }

    public static readonly DependencyProperty EditorForProperty = DependencyProperty.Register(
        nameof(EditorFor),
        typeof(string),
        typeof(EditElementWrapper)
      );
    public string EditorFor {
      get { return (string)GetValue(EditorForProperty); }
      set { SetValue(EditorForProperty, value); }
    }

    public EditElementWrapper() {
      InitializeComponent();
      Children = PART_Host.Children;
      Loaded += (s, e) => Form.RegisterEditWrapper(this);
    }

    internal void DisplayErrors(IEnumerable<EntityError> errorsForField) {
      if (errorsForField.Count() == 0) {
        uxErrorMessage.Visibility = Visibility.Collapsed;
        uxBorder.BorderBrush = null;
      } else {
        uxErrorMessage.Visibility = Visibility.Visible;
        uxErrorMessage.Text = string.Join("\r\n", errorsForField.Select(x => x.Message));
        uxBorder.BorderBrush = Brushes.Red;
      }
    }

  }
}
