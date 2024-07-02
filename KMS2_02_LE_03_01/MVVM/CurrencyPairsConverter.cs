using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace KMS2_02_LE_03_01.MVVM
{
    public class CurrencyPairsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currencyPairs = value as IEnumerable<string>;
            if (currencyPairs != null)
            {
                // Vytvorenie dlhého reťazca opakovaním forexových párov
                string longString = string.Join(" | ", currencyPairs);
                return string.Concat(Enumerable.Repeat(longString, 10));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
