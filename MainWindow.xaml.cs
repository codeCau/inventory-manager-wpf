using Wpf.Ui.Controls;
using ControleEstoqueWPF.ViewModels;

namespace ControleEstoqueWPF;

public partial class MainWindow : FluentWindow
{
    public MainWindow(ProdutoViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
