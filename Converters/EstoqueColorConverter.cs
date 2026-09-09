using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ControleEstoqueWPF.Converters;

public class EstoqueColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int qtd)
        {
            if (qtd <= 5) return new SolidColorBrush(Color.FromRgb(220, 53, 69));
            if (qtd <= 10) return new SolidColorBrush(Color.FromRgb(40, 167, 69));
            return new SolidColorBrush(Color.FromRgb(255, 140, 0));
        }
        return new SolidColorBrush(Colors.Gray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
