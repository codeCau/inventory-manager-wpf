using ControleEstoqueWPF.ViewModels;
using Wpf.Ui.Controls;

namespace ControleEstoqueWPF
{
    public partial class MainWindow : FluentWindow
    {
        public MainWindow(ProdutoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += async (s, e) => await viewModel.CarregarProdutosAsync();
        }
    }
}
