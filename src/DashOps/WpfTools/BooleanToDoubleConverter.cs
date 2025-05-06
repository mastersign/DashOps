using System.Globalization;
using System.Windows.Data;

namespace Mastersign.WpfTools
{
    [ValueConversion(typeof(bool), typeof(double))]
    public sealed class BooleanToDoubleConverter : IValueConverter
    {
        public double IfFalse { get; set; } = 0.0;

        public double IfTrue { get; set; } = 1.0;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is true ? IfTrue : IfFalse;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
