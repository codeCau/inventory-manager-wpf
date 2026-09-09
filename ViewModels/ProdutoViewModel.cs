using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        [RelayCommand]
        public async Task CarregarProdutosAsync()
        {
            try
            {
                var lista = await _context.Produtos
                    .AsNoTracking()
                    .OrderBy(p => p.Id)
                    .ToListAsync();

                Produtos = new ObservableCollection<Produto>(lista);
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro de Leitura", $"Falha ao carregar registros: {ex.Message}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public async Task SalvarProdutoAsync()
        {
            ValidateAllProperties();

            if (HasErrors)
            {
                var primeiroErro = GetErrors().FirstOrDefault()?.ErrorMessage ?? "Existem campos inválidos no formulário.";
                ExibirMensagem("Atenção", primeiroErro, InfoBarSeverity.Warning);
                return;
            }

            try
            {
                if (IsEditing && Id > 0)
                {
                    var produtoDb = await _context.Produtos.FindAsync(Id);
                    if (produtoDb == null)
                    {
                        ExibirMensagem("Erro", "Produto não encontrado para atualização.", InfoBarSeverity.Error);
                        return;
                    }

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
                ExibirMensagem("Erro de Persistência", $"Falha ao salvar no banco: {ex.Message}", InfoBarSeverity.Error);
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
            if (produto == null) return;

            try
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    _context.Produtos.Remove(produtoDb);
                    await _context.SaveChangesAsync();

                    if (Id == produto.Id)
                    {
                        LimparFormulario();
                    }

                    ExibirMensagem("Excluído", $"Produto \"{produto.Nome}\" removido do estoque.", InfoBarSeverity.Success);
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
            if (quantidade <= 5)
            {
                ExibirMensagem(acao, $"Estoque em estado CRÍTICO ({quantidade} un.). Reposição urgente necessária!", InfoBarSeverity.Error);
            }
            else if (quantidade <= 10)
            {
                ExibirMensagem(acao, $"Estoque em nível NORMAL ({quantidade} un.). Operação regular.", InfoBarSeverity.Success);
            }
            else
            {
                ExibirMensagem(acao, $"Estoque EXCEDENTE ({quantidade} un.). Capacidade máxima atingida.", InfoBarSeverity.Warning);
            }
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
