using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using wpf_sample.lib.utils;

namespace wpf_sample.lib {
  public partial class CardSelector : UserControl {
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
        nameof(Selected),
        typeof(object),
        typeof(CardSelector),
        new FrameworkPropertyMetadata() {
          BindsTwoWayByDefault = true,
          DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
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
            Type enumType = Type.GetType(ea.NewValue.ToString());
            if (enumType == null || !enumType.IsEnum)
              throw new Exception(string.Format("Type '{0}' either does not exist or is not an enum", ea.NewValue));
            ((CardSelector)o).CreateCards(enumType.GetEnumValues().Cast<object>().Select(x => new CardInfo(x)));
          })
        ) 
      );
    public string ItemsSourceEnum {
      get { return (string)GetValue(ItemsSourceEnumProperty); }
      set { SetValue(ItemsSourceEnumProperty, value); }
    }

    private void CreateCards(IEnumerable<CardInfo> cardInfos) {
      uxRoot.Children.Clear();
      foreach (CardInfo cardInfo in cardInfos) {
        uxRoot.Children.Add(new Card(cardInfo, () => SelectCard(cardInfo.Value)));
      }
    }

    private void SelectCard(object value) {
      foreach (Card card in uxRoot.Children)
        card.IsSelected = card.Value.Equals(value);
      Selected = value;
    }

    public CardSelector() {
      InitializeComponent();
    }
  }

  class CardInfo {
    internal string Label;
    internal object Value;
    internal string IconName; // TODO

    internal CardInfo(object enumValue) {
      Value = enumValue;
      Label = NameUtils.CamelCaseToHumanReadable(enumValue.ToString());
    }
  }
}
