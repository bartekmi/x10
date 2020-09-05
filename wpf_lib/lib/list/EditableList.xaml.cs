using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace wpf_lib.lib {
  // [ContentProperty(nameof(TemplateContent))]
  public partial class EditableList : UserControl {
    public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(EditableList),
        new FrameworkPropertyMetadata() {
          PropertyChangedCallback = new PropertyChangedCallback((s, e) => ((EditableList)s).PART_ListBox.ItemTemplate = (DataTemplate)e.NewValue),
        });
    public DataTemplate ItemTemplate {
      get { return (DataTemplate)GetValue(ItemTemplateProperty); }
      set { SetValue(ItemTemplateProperty, value); }
    }

    public static readonly DependencyProperty AddLabelProperty = DependencyProperty.Register(
        nameof(AddLabel),
        typeof(string),
        typeof(EditableList),
        new FrameworkPropertyMetadata() {
          PropertyChangedCallback = new PropertyChangedCallback((s, e) => ((EditableList)s).PART_AddButton.Content = (string)e.NewValue),
        });
    public string AddLabel {
      get { return (string)GetValue(AddLabelProperty); }
      set { SetValue(AddLabelProperty, value); }
    }

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(EditableList),
        new FrameworkPropertyMetadata() {
          PropertyChangedCallback = new PropertyChangedCallback((s, e) => ((EditableList)s).PART_ListBox.ItemsSource = (IEnumerable)e.NewValue),
        });
    public IEnumerable ItemsSource {
      get { return (IEnumerable)GetValue(ItemsSourceProperty); }
      set { SetValue(ItemsSourceProperty, value); }
    }

    public EditableList() {
      InitializeComponent();
    }

    private void PART_AddButton_Click(object sender, RoutedEventArgs e) {
      // TODO: Pass event upward
      Console.WriteLine("Clicked: " + sender);
    }
  }
}
