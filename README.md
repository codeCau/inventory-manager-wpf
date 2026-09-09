# 📦 Sistema de Controle de Estoque & Cadastro de Produtos

Sistema desktop para gestão e controle de inventário em tempo real, construído em **C# / .NET** com interface moderna em **WPF (WPF-UI)**, persistência relacional com **PostgreSQL via Entity Framework Core** e arquitetura reativa **MVVM**.

O sistema conta com monitoramento inteligente e visual dos níveis de estoque para prevenir desabastecimento ou sobrecarga de inventário.

---

## 🎯 Regras de Negócio: Níveis de Estoque

A tabela de produtos e os alertas visuais categorizam automaticamente a quantidade em estoque em três estados:

| Faixa de Quantidade | Classificação | Destaque Visual | Status / Ação Recomendada |
|---|---|---|---|
| **0 a 5 unidades** | Crítico | 🔴 Vermelho | **Reposição Imediata** (Risco de falta/ruptura) |
| **6 a 10 unidades** | Normal | 🟢 Verde | **Estoque Regular** (Nível operacional ideal) |
| **Mais de 10 unidades** | Excedente | 🟠 Laranja | **Capacidade Máxima / Cheio** (Pausar novos pedidos) |

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem:** C# (.NET 8 / 10)
* **Interface Gráfica:** WPF (Windows Presentation Foundation) com XAML
* **Design System / Componentes:** WPF-UI (Fluent Design moderno)
* **Padrão Arquitetural:** MVVM (Model-View-ViewModel) via CommunityToolkit.Mvvm
* **Mapeamento Objeto-Relacional (ORM):** Entity Framework Core (Npgsql.EntityFrameworkCore.PostgreSQL)
* **Banco de Dados:** PostgreSQL 15+

---

## 📂 Estrutura do Projeto

ControleEstoqueWPF/
├── Converters/                 # Conversores de valor XAML para cores e badges
│   ├── EstoqueBadgeConverter.cs
│   └── EstoqueColorConverter.cs
├── Data/                       # Contexto EF Core e fábrica em tempo de design
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── Models/                     # Entidades de domínio e validações
│   └── Produto.cs
├── ViewModels/                 # Lógica de apresentação reativa e comandos CRUD
│   └── ProdutoViewModel.cs
├── appsettings.json            # String de conexão com o PostgreSQL
├── MainWindow.xaml             # Interface gráfica principal
├── seed.sql                    # Script SQL para criação e carga inicial
└── README.md

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* .NET SDK (versão 8.0 ou superior)
* PostgreSQL ativo na porta local 5432

### 1. Inicializar e Popular o Banco de Dados
PGPASSWORD=postgres psql -h localhost -U postgres -d estoque_db -f seed.sql

### 2. Configurar a Conexão
Edite o arquivo appsettings.json com suas credenciais:
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=postgres"
  }
}

### 3. Compilar e Executar
dotnet restore
dotnet build
dotnet run
