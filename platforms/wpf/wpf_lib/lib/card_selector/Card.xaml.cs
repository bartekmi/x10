using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace wpf_lib.lib {
  public partial class Card : UserControl {

    public string Label { get; private set; }
    public object Value { get; private set; }

    private bool _isSelected;
    public bool IsSelected {
      get { return _isSelected; }
      set {
        _isSelected = value;
        uxBorder.Background = value ? Brushes.SkyBlue : Brushes.WhiteSmoke;
      }
    }

    internal Card(CardInfo cardInfo, Action clickHandler) {
      InitializeComponent();

      uxName.Text = cardInfo.Label;
      Value = cardInfo.Value;
      this.MouseDown += (s, e) => clickHandler();
      IsSelected = false;
    }
  }
}
