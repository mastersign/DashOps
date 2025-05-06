using System;
using System.Globalization;
using System.Windows.Data;

namespace Mastersign.WpfTools
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public sealed class NegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => Convert(value, targetType, parameter, culture);
    }
}
