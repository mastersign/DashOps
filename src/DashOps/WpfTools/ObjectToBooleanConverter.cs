using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Mastersign.WpfTools
{
    [ValueConversion(typeof(object), typeof(bool))]
    public sealed class ObjectToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is not null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
