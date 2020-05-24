using System;
using System.Globalization;
using System.Windows.Data;

namespace wpf_sample.lib.converter {
  // By default null => false. Use Converter Parameter 'reverse' to reverse logic.
  public class NullToBooleanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      bool isTrue = value != null;
      if (parameter != null && parameter.ToString().ToLower() == "reverse")
        isTrue = !isTrue;
      return isTrue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
