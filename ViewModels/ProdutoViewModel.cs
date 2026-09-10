using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleEstoqueWPF.Data;
using ControleEstoqueWPF.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace ControleEstoqueWPF.ViewModels
{
    public partial class ProdutoViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private List<Produto> _todosProdutos = new();
        private ContentPresenter? _dialogPresenter;

        [ObservableProperty]
        private ObservableCollection<Produto> _produtos = new();

        [ObservableProperty]
        private Produto? _produtoSelecionado;

        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _nome = string.Empty;

        [ObservableProperty]
        private string? _descricao;

        [ObservableProperty]
        private decimal _preco = 0.00m;

        [ObservableProperty]
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
        private decimal _valorTotalEstoque;

        [ObservableProperty]
        private int _quantidadeTotalItens;

        [ObservableProperty]
        private decimal _precoMedioUnitario;

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
                _todosProdutos = await _context.Produtos
                    .AsNoTracking()
                    .OrderBy(p => p.Id)
                    .ToListAsync();

                AtualizarMetricas();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ExibirMensagem("Erro de Leitura", $"Falha ao carregar registros: {msg}", InfoBarSeverity.Error);
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

        private void AtualizarMetricas()
        {
            TotalGeral = _todosProdutos.Count;
            TotalCriticos = _todosProdutos.Count(p => p.QuantidadeEstoque <= 5);
            TotalNormais = _todosProdutos.Count(p => p.QuantidadeEstoque >= 6 && p.QuantidadeEstoque <= 10);
            TotalExcedentes = _todosProdutos.Count(p => p.QuantidadeEstoque > 10);

            QuantidadeTotalItens = _todosProdutos.Sum(p => p.QuantidadeEstoque);
            ValorTotalEstoque = _todosProdutos.Sum(p => p.Preco * p.QuantidadeEstoque);
            PrecoMedioUnitario = _todosProdutos.Count > 0 ? _todosProdutos.Average(p => p.Preco) : 0m;
        }

        [RelayCommand]
        public async Task IncrementarEstoqueAsync(Produto produto)
        {
            if (produto == null) return;

            try
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    produtoDb.QuantidadeEstoque += 1;
                    await _context.SaveChangesAsync();
                    await CarregarProdutosAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ExibirMensagem("Erro", $"Falha ao atualizar estoque: {msg}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public async Task DecrementarEstoqueAsync(Produto produto)
        {
            if (produto == null || produto.QuantidadeEstoque <= 0) return;

            try
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null && produtoDb.QuantidadeEstoque > 0)
                {
                    produtoDb.QuantidadeEstoque -= 1;
                    await _context.SaveChangesAsync();
                    await CarregarProdutosAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ExibirMensagem("Erro", $"Falha ao decrementar estoque: {msg}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public async Task SalvarProdutoAsync()
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                ExibirMensagem("Atenção", "O nome do produto é obrigatório.", InfoBarSeverity.Warning);
                return;
            }

            if (Preco <= 0m)
            {
                ExibirMensagem("Atenção", "O preço unitário deve ser maior que R$ 0,00.", InfoBarSeverity.Warning);
                return;
            }

            if (QuantidadeEstoque < 0)
            {
                ExibirMensagem("Atenção", "A quantidade em estoque não pode ser negativa.", InfoBarSeverity.Warning);
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
                    produtoDb.Descricao = string.IsNullOrWhiteSpace(Descricao) ? null : Descricao.Trim();
                    produtoDb.Preco = Preco;
                    produtoDb.QuantidadeEstoque = QuantidadeEstoque;

                    _context.Produtos.Update(produtoDb);
                    await _context.SaveChangesAsync();
                    ExibirMensagemEstoque("Produto Atualizado", QuantidadeEstoque);
                }
                else
                {
                    var novoProduto = new Produto
                    {
                        Nome = Nome.Trim(),
                        Descricao = string.IsNullOrWhiteSpace(Descricao) ? null : Descricao.Trim(),
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
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ExibirMensagem("Erro no Banco", $"Falha ao salvar: {msg}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public void PrepararEdicao(Produto produto)
        {
            if (produto == null) return;

            Id = produto.Id;
            Nome = produto.Nome;
            Descricao = produto.Descricao;
            Preco = produto.Preco;
            QuantidadeEstoque = produto.QuantidadeEstoque;
            IsEditing = true;

            ExibirMensagem("Modo Edição", $"Editando: {produto.Nome} (ID: {produto.Id})", InfoBarSeverity.Informational);
        }

        [RelayCommand]
        public async Task ExcluirProdutoAsync(Produto produto)
        {
            if (produto == null) return;

            if (_dialogPresenter != null)
            {
                var dialog = new ContentDialog(_dialogPresenter)
                {
                    Title = "Confirmar Exclusão",
                    Content = $"Deseja realmente excluir o produto \"{produto.Nome}\"?\nEsta ação é irreversível.",
                    PrimaryButtonText = "Sim, Excluir",
                    CloseButtonText = "Cancelar",
                    PrimaryButtonAppearance = ControlAppearance.Danger
                };

                var result = await dialog.ShowAsync();
                if (result != ContentDialogResult.Primary) return;
            }

            try
            {
                var produtoDb = await _context.Produtos.FindAsync(produto.Id);
                if (produtoDb != null)
                {
                    _context.Produtos.Remove(produtoDb);
                    await _context.SaveChangesAsync();

                    if (Id == produto.Id) LimparFormulario();

                    ExibirMensagem("Excluído", $"Produto \"{produto.Nome}\" removido com sucesso.", InfoBarSeverity.Success);
                    await CarregarProdutosAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ExibirMensagem("Erro", $"Falha ao excluir produto: {msg}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public void ExportarCsv()
        {
            if (Produtos.Count == 0)
            {
                ExibirMensagem("Aviso", "Não há produtos na listagem para exportar.", InfoBarSeverity.Warning);
                return;
            }

            try
            {
                var dialog = new SaveFileDialog
                {
                    Title = "Exportar Relatório de Estoque",
                    Filter = "Arquivo CSV (*.csv)|*.csv",
                    FileName = $"relatorio_estoque_{DateTime.Now:yyyyMMdd_HHmm}.csv"
                };

                if (dialog.ShowDialog() == true)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("ID;Nome;Descrição;Preço Unitário;Quantidade;Status;Data Cadastro");

                    foreach (var p in Produtos)
                    {
                        var status = p.QuantidadeEstoque switch
                        {
                            <= 5 => "Crítico (Repor)",
                            <= 10 => "Normal",
                            _ => "Excedente (Cheio)"
                        };

                        var linha = $"{p.Id};\"{p.Nome.Replace("\"", "\"\"")}\";\"{p.Descricao?.Replace("\"", "\"\"")}\";{p.Preco:F2};{p.QuantidadeEstoque};{status};{p.DataCadastro:dd/MM/yyyy HH:mm}";
                        sb.AppendLine(linha);
                    }

                    File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
                    ExibirMensagem("Exportação Concluída", $"Relatório gerado em: {Path.GetFileName(dialog.FileName)}", InfoBarSeverity.Success);
                }
            }
            catch (Exception ex)
            {
                ExibirMensagem("Erro na Exportação", $"Falha ao gravar arquivo: {ex.Message}", InfoBarSeverity.Error);
            }
        }

        [RelayCommand]
        public void LimparFormulario()
        {
            Id = 0;
            Nome = string.Empty;
            Descricao = string.Empty;
            Preco = 0.00m;
            QuantidadeEstoque = 0;
            IsEditing = false;
        }

        private void ExibirMensagemEstoque(string acao, int quantidade)
        {
            if (quantidade <= 5) ExibirMensagem(acao, $"Estoque CRÍTICO ({quantidade} un.). Reposição urgente!", InfoBarSeverity.Error);
            else if (quantidade <= 10) ExibirMensagem(acao, $"Estoque NORMAL ({quantidade} un.). Operação regular.", InfoBarSeverity.Success);
            else ExibirMensagem(acao, $"Estoque EXCEDENTE ({quantidade} un.). Capacidade máxima atingida.", InfoBarSeverity.Warning);
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
