using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace KMS2_02_LE_03_01.MVVM
{
    /// <summary>
    /// Converts a collection of currency pairs to a long string suitable for display.
    /// </summary>
    public class CurrencyPairsConverter : IValueConverter
    {
        /// <summary>
        /// Converts a collection of currency pairs to a long string where each pair is separated by " | " and the whole sequence is repeated 10 times.
        /// </summary>
        /// <param name="value">The value produced by the binding source, typically an IEnumerable of currency pair strings.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A concatenated string of currency pairs repeated 10 times, or an empty string if the input is null or not an IEnumerable of strings.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currencyPairs = value as IEnumerable<string>;
            if (currencyPairs != null)
            {
                // Create a long string by repeating the forex pairs
                string longString = string.Join(" | ", currencyPairs);
                return string.Concat(Enumerable.Repeat(longString, 10));
            }
            return string.Empty;
        }

        /// <summary>
        /// This method is not implemented and will throw a NotImplementedException if called.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
