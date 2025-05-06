using System.Globalization;
using System.Windows.Data;

namespace Mastersign.WpfTools
{
    [ValueConversion(typeof(bool), typeof(double))]
    public sealed class BooleanToIntegerConverter : IValueConverter
    {
        public int IfFalse { get; set; } = 0;

        public int IfTrue { get; set; } = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => value is true ? IfTrue : IfFalse;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
