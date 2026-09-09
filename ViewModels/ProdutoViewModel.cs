using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleEstoqueWPF.Data;
using ControleEstoqueWPF.Models;
using Microsoft.EntityFrameworkCore;
using Wpf.Ui.Controls;

namespace ControleEstoqueWPF.ViewModels
{
    public partial class ProdutoViewModel : ObservableValidator
    {
        private readonly AppDbContext _context;
        private List<Produto> _todosProdutos = new();
        private ContentPresenter? _dialogPresenter; // Host para o ContentDialog

        [ObservableProperty]
        private ObservableCollection<Produto> _produtos = new();

        [ObservableProperty]
        private Produto? _produtoSelecionado;

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [MinLength(2, ErrorMessage = "O nome deve ter no mínimo 2 caracteres.")]
        [MaxLength(150, ErrorMessage = "O nome não pode exceder 150 caracteres.")]
        private string _nome = string.Empty;

        [ObservableProperty]
        [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        private string? _descricao;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(typeof(decimal), "0.01", "999999.99", ErrorMessage = "O preço deve ser maior que R$ 0,00.")]
        private decimal _preco = 0.01m;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, 1000000, ErrorMessage = "A quantidade de estoque não pode ser negativa.")]
        private int _quantidadeEstoque = 0;

        [ObservableProperty]
        private string _termoPesquisa = string.Empty;

        [ObservableProperty]
        private string _filtroStatus = "Todos";

        [ObservableProperty]
        private int _totalGeral;

        [ObservableProperty]
        private int _totalCriticos;

        [ObservableProperty]
        private int _totalNormais;

        [ObservableProperty]
        private int _totalExcedentes;

        [ObservableProperty]
        private bool _isInfoBarOpen;

        [ObservableProperty]
        private string _infoBarTitle = string.Empty;

        [ObservableProperty]
        private string _infoBarMessage = string.Empty;

        [ObservableProperty]
        private InfoBarSeverity _infoBarSeverity = InfoBarSeverity.Informational;

        [ObservableProperty]
        private bool _isEditing;

        public ProdutoViewModel(AppDbContext context)
        {
            _context = context;
        }

        // Método chamado pelo Code-Behind para injetar o Host do Dialog
        public void DefinirDialogPresenter(ContentPresenter presenter)
        {
            _dialogPresenter = presenter;
        }

        partial void OnTermoPesquisaChanged(string value) => AplicarFiltros();
        partial void OnFiltroStatusChanged(string value) => AplicarFiltros();

        [RelayCommand]
        public void FiltrarPorStatus(string status) => FiltroStatus = status;

        [RelayCommand]
        public async Task CarregarProdutosAsync()
        {
            try
            {
                _todosProdutos = await _context.Produtos.AsNoTracking().OrderBy(p => p.Id).ToListAsync();
                AtualizarContadores();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro", $"Falha ao carregar registros: {ex.Message}", InfoBarSeverity.Error);
            }
        }

        public void AplicarFiltros()
        {
            var consulta = _todosProdutos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(TermoPesquisa))
            {
                consulta = consulta.Where(p =>
                    p.Nome.Contains(TermoPesquisa, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(p.Descricao) && p.Descricao.Contains(TermoPesquisa, StringComparison.OrdinalIgnoreCase)));
            }

            consulta = FiltroStatus switch
            {
                "Critico" => consulta.Where(p => p.QuantidadeEstoque <= 5),
                "Normal" => consulta.Where(p => p.QuantidadeEstoque >= 6 && p.QuantidadeEstoque <= 10),
                "Excedente" => consulta.Where(p => p.QuantidadeEstoque > 10),
                _ => consulta
            };

            Produtos = new ObservableCollection<Produto>(consulta);
        }

        private void AtualizarContadores()
        {
            TotalGeral = _todosProdutos.Count;
            TotalCriticos = _todosProdutos.Count(p => p.QuantidadeEstoque <= 5);
            TotalNormais = _todosProdutos.Count(p => p.QuantidadeEstoque >= 6 && p.QuantidadeEstoque <= 10);
            TotalExcedentes = _todosProdutos.Count(p => p.QuantidadeEstoque > 10);
        }

        [RelayCommand]
        public async Task SalvarProdutoAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                var primeiroErro = GetErrors().FirstOrDefault()?.ErrorMessage ?? "Campos inválidos.";
                ExibirMensagem("Atenção", primeiroErro, InfoBarSeverity.Warning);
                return;
            }

            try
            {
                if (IsEditing && Id > 0)
                {
                    var produtoDb = await _context.Produtos.FindAsync(Id);
                    if (produtoDb == null) return;

                    produtoDb.Nome = Nome.Trim();
                    produtoDb.Descricao = Descricao?.Trim();
                    produtoDb.Preco = Preco;
                    produtoDb.QuantidadeEstoque = QuantidadeEstoque;

                    await _context.SaveChangesAsync();
                    ExibirMensagemEstoque("Produto Atualizado", QuantidadeEstoque);
                }
                else
                {
                    var novoProduto = new Produto
                    {
                        Nome = Nome.Trim(),
                        Descricao = Descricao?.Trim(),
                        Preco = Preco,
                        QuantidadeEstoque = QuantidadeEstoque,
                        DataCadastro = DateTime.UtcNow
                    };

                    await _context.Produtos.AddAsync(novoProduto);
                    await _context.SaveChangesAsync();
                    ExibirMensagemEstoque("Produto Cadastrado", QuantidadeEstoque);
                }

                LimparFormulario();
                await CarregarProdutosAsync();
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro", $"Falha ao salvar: {ex.Message}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public void PrepararEdicao(Produto produto)
        {
            if (produto == null) return;
            ClearErrors();
            Id = produto.Id;
            Nome = produto.Nome;
            Descricao = produto.Descricao;
            Preco = produto.Preco;
            QuantidadeEstoque = produto.QuantidadeEstoque;
            IsEditing = true;
        }

        [RelayCommand]
        public async Task ExcluirProdutoAsync(Produto produto)
        {
            if (produto == null || _dialogPresenter == null) return;

            // Cria e configura o modal do WPF-UI
            var dialog = new ContentDialog(_dialogPresenter)
            {
                Title = "Confirmar Exclusão",
                Content = $"Tem certeza que deseja excluir permanentemente o produto \"{produto.Nome}\"?\nEsta ação não poderá ser desfeita.",
                PrimaryButtonText = "Sim, Excluir",
                CloseButtonText = "Cancelar",
                PrimaryButtonAppearance = ControlAppearance.Danger
            };

            // Aguarda a decisão do usuário
            var result = await dialog.ShowAsync();
            
            // Se ele não clicou no botão primário (Sim, Excluir), cancela a operação
            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            try
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    _context.Produtos.Remove(produtoDb);
                    await _context.SaveChangesAsync();

                    if (Id == produto.Id) LimparFormulario();

                    ExibirMensagem("Excluído", $"O produto \"{produto.Nome}\" foi removido.", InfoBarSeverity.Success);
                    await CarregarProdutosAsync();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro", $"Falha ao excluir produto: {ex.Message}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public void LimparFormulario()
        {
            ClearErrors();
            Id = 0;
            Nome = string.Empty;
            Descricao = string.Empty;
            Preco = 0.01m;
            QuantidadeEstoque = 0;
            IsEditing = false;
        }

        private void ExibirMensagemEstoque(string acao, int quantidade)
        {
            if (quantidade <= 5) ExibirMensagem(acao, $"Estoque CRÍTICO ({quantidade} un.).", InfoBarSeverity.Error);
            else if (quantidade <= 10) ExibirMensagem(acao, $"Estoque NORMAL ({quantidade} un.).", InfoBarSeverity.Success);
            else ExibirMensagem(acao, $"Estoque EXCEDENTE ({quantidade} un.).", InfoBarSeverity.Warning);
        }

        private void ExibirMensagem(string titulo, string mensagem, InfoBarSeverity severidade)
        {
            InfoBarTitle = titulo;
            InfoBarMessage = mensagem;
            InfoBarSeverity = severidade;
            IsInfoBarOpen = true;
        }
    }
}
