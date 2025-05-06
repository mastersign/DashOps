using System.Globalization;
using System.Windows.Data;

namespace Mastersign.WpfTools
{
    [ValueConversion(typeof(int), typeof(bool))]
    public sealed class IntegerComparisonConverter : IValueConverter
    {
        public int Value { get; set; } = 1;

        public NumericComparison Comparator { get; set; } = NumericComparison.EqualTo;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is int number ? Comparator switch
            {
                NumericComparison.EqualTo => number == Value,
                NumericComparison.NotEqualTo => number != Value,
                NumericComparison.GreaterThen => number > Value,
                NumericComparison.GreaterThenOrEqualTo => number >= Value,
                NumericComparison.LessThen => number < Value,
                NumericComparison.LessThenOrEqualTo => number <= Value,
                _ => false,
            } : false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    public enum NumericComparison
    {
        EqualTo,
        NotEqualTo,
        GreaterThen,
        GreaterThenOrEqualTo,
        LessThen,
        LessThenOrEqualTo,
    }
}
