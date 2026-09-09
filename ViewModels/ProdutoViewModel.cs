using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ControleEstoqueWPF.Data;
using ControleEstoqueWPF.Models;

namespace ControleEstoqueWPF.ViewModels;

public partial class ProdutoViewModel : ObservableObject
{
    private readonly AppDbContext _context;

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
    private decimal _preco;

    [ObservableProperty]
    private int _quantidadeEstoque;

    [ObservableProperty]
    private string _mensagemAlerta = string.Empty;

    [ObservableProperty]
    private bool _mostrarAlerta;

    public ProdutoViewModel(AppDbContext context)
    {
        _context = context;
        _ = CarregarProdutosAsync();
    }

    [RelayCommand]
    public async Task CarregarProdutosAsync()
    {
        var lista = await _context.Produtos.AsNoTracking().ToListAsync();
        Produtos = new ObservableCollection<Produto>(lista);
    }

    [RelayCommand]
    public async Task SalvarProdutoAsync()
    {
        if (string.IsNullOrWhiteSpace(Nome) || Preco < 0 || QuantidadeEstoque < 0)
        {
            ExibirAviso("Preencha todos os campos obrigatórios corretamente.");
            return;
        }

        if (Id == 0)
        {
            var novoProduto = new Produto
            {
                Nome = Nome,
                Descricao = Descricao,
                Preco = Preco,
                QuantidadeEstoque = QuantidadeEstoque,
                DataCadastro = DateTime.UtcNow
            };
            await _context.Produtos.AddAsync(novoProduto);
        }
        else
        {
            var produtoDb = await _context.Produtos.FindAsync(Id);
            if (produtoDb != null)
            {
                produtoDb.Nome = Nome;
                produtoDb.Descricao = Descricao;
                produtoDb.Preco = Preco;
                produtoDb.QuantidadeEstoque = QuantidadeEstoque;
            }
        }

        await _context.SaveChangesAsync();
        VerificarAlertaNivel(Nome, QuantidadeEstoque);
        LimparFormulario();
        await CarregarProdutosAsync();
    }

    [RelayCommand]
    public async Task ExcluirProdutoAsync(Produto? produto)
    {
        if (produto == null) return;

        var item = await _context.Produtos.FindAsync(produto.Id);
        if (item != null)
        {
            _context.Produtos.Remove(item);
            await _context.SaveChangesAsync();
            await CarregarProdutosAsync();
            ExibirAviso($"Produto '{item.Nome}' excluído com sucesso.");
        }
    }

    [RelayCommand]
    public void PrepararEdicao(Produto? produto)
    {
        if (produto == null) return;

        Id = produto.Id;
        Nome = produto.Nome;
        Descricao = produto.Descricao;
        Preco = produto.Preco;
        QuantidadeEstoque = produto.QuantidadeEstoque;
    }

    [RelayCommand]
    public void LimparFormulario()
    {
        Id = 0;
        Nome = string.Empty;
        Descricao = string.Empty;
        Preco = 0;
        QuantidadeEstoque = 0;
        ProdutoSelecionado = null;
    }

    private void VerificarAlertaNivel(string nome, int qtd)
    {
        if (qtd <= 5)
        {
            ExibirAviso($"ALERTA CRÍTICO: O produto '{nome}' possui apenas {qtd} unidade(s) em estoque!");
        }
        else if (qtd > 10)
        {
            ExibirAviso($"AVISO DE CAPACIDADE: O produto '{nome}' ultrapassou o limite ideal com {qtd} unidades!");
        }
        else
        {
            ExibirAviso($"Produto '{nome}' salvo com sucesso! Estoque normal ({qtd} unidades).");
        }
    }

    private void ExibirAviso(string mensagem)
    {
        MensagemAlerta = mensagem;
        MostrarAlerta = true;
    }
}
