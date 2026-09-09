using System;
using System.Globalization;
using System.Windows.Data;

namespace ControleEstoqueWPF.Converters;

public class EstoqueBadgeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int qtd)
        {
            if (qtd <= 5) return "Crítico (Repor)";
            if (qtd <= 10) return "Normal";
            return "Excedente (Cheio)";
        }
        return "Indefinido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
