using System;
using System.Windows.Data;
using System.Globalization;


using Booth.Common;

namespace Booth.PortfolioManager.Client.Style
{
    [ValueConversion(typeof(Date), typeof(DateTime))]
    class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Date)value).DateTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Date((DateTime)value);
        }
    }

}
