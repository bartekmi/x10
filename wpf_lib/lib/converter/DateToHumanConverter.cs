using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace wpf_lib.lib.converter {

  public class DateToHumanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameterObj, CultureInfo culture) {
      if (value is DateTime dateTime) {
        return dateTime.ToString("yyyy/MM/dd");
      }
      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
