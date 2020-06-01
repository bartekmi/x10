using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using wpf_lib;
using wpf_lib.lib.utils;

namespace wpf_lib.lib {
  public partial class CardSelector : UserControl {
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
        nameof(Selected),
        typeof(object),
        typeof(CardSelector),
        new FrameworkPropertyMetadata() {
          BindsTwoWayByDefault = true,
          DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
          PropertyChangedCallback = new PropertyChangedCallback((o, ea) => {
            ((CardSelector)o).SelectCard();
          }),
        }
      );
    public object Selected {
      get { return GetValue(SelectedProperty); }
      set { SetValue(SelectedProperty, value); }
    }

    public static readonly DependencyProperty ItemsSourceEnumProperty = DependencyProperty.Register(
        nameof(ItemsSourceEnum),
        typeof(string),
        typeof(CardSelector),
        new FrameworkPropertyMetadata(
          new PropertyChangedCallback((o, ea) => {
            Type enumType = WpfLibConfig.GetType(ea.NewValue.ToString());
            if (enumType == null || !enumType.IsEnum)
              throw new Exception(string.Format("Type '{0}' either does not exist or is not an enum", ea.NewValue));
            ((CardSelector)o).SetCardInfos(enumType.GetEnumValues().Cast<object>().Select(x => new CardInfo(x)));
          })
        )
      );
    public string ItemsSourceEnum {
      get { return (string)GetValue(ItemsSourceEnumProperty); }
      set { SetValue(ItemsSourceEnumProperty, value); }
    }

    IEnumerable<CardInfo> _cardInfos;
    internal void SetCardInfos(IEnumerable<CardInfo> cardInfos) {
      _cardInfos = cardInfos;
    }

    private void SelectCard() {
      foreach (Card card in uxRoot.Children)
        card.IsSelected = card.Value.Equals(Selected);
    }

    public CardSelector() {
      InitializeComponent();

      Loaded += (s, e) => {
        uxRoot.Children.Clear();
        foreach (CardInfo cardInfo in _cardInfos)
          uxRoot.Children.Add(new Card(cardInfo, () => Selected = cardInfo.Value));
        SelectCard();
      };
    }
  }

  class CardInfo {
    internal string Label;
    internal object Value;
    internal string IconName; // TODO

    internal CardInfo() { }

    internal CardInfo(object enumValue) {
      Value = enumValue;
      Label = NameUtils.CamelCaseToHumanReadable(enumValue.ToString());
    }
  }
}
