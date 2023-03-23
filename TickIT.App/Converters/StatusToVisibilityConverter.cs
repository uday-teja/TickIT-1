using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static TickIT.Models.Enums;

namespace TickIT.App.Converters
{
    public class StatusToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is Status status && status == Status.InProgress) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
