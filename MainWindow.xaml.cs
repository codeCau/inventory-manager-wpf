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
            
            // Define o local onde o Modal de Confirmação será renderizado
            viewModel.DefinirDialogPresenter(RootContentDialogPresenter);
            
            Loaded += async (s, e) => await viewModel.CarregarProdutosAsync();
        }
    }
}
