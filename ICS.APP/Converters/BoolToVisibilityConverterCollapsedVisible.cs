using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeamsManager.APP.Converters
{
    public class BoolToVisibilityConverterCollapsedVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (Boolean)value == false ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
