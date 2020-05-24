using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace wpf_sample.lib.converter {

  // Converts Boolean value to Visibility
  // By default: true => Visible, false => Collapsed
  // If parameter contains 'reverse', boolean is reversed
  // If parameter contains 'hidden', Hidden is used instead of Collapsed
  public class BooleanToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameterObj, CultureInfo culture) {
      bool boolean = (bool)value;
      return ConvertToVisibility(boolean, parameterObj);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }

    public static Visibility ConvertToVisibility(bool boolean, object parameterObj) {
      Visibility whenFalse = Visibility.Collapsed;

      if (parameterObj != null) {
        string parameter = parameterObj.ToString().ToLower();
        if (parameter.Contains("reverse"))
          boolean = !boolean;

        if (parameter.Contains("hidden"))
          whenFalse = Visibility.Hidden;
      }

      return boolean ? Visibility.Visible : whenFalse;
    }
  }

}
