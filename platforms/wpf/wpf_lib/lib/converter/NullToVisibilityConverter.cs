using System;
using System.Globalization;
using System.Windows.Data;

namespace wpf_lib.lib.converter {
  public class NullToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameterObj, CultureInfo culture) {
      return BooleanToVisibilityConverter.ConvertToVisibility(value != null, parameterObj);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }

}
