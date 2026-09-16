# Sistema de Controle de Estoque

Aplicação desktop para gerenciamento e controle de estoque, desenvolvida em **C# / .NET 8**, utilizando **WPF-UI**, arquitetura **MVVM** e **PostgreSQL** com **Entity Framework Core**.

## 🚀 Funcionalidades

- **Dashboard de KPIs:** Visão consolidada de métricas financeiras, valor total do inventário e indicadores de desempenho.
- **Gestão de Produtos (CRUD):** Cadastro, edição, listagem e remoção de produtos, categorias e fornecedores.
- **Alertas de Estoque:** Sinalização de itens com estoque baixo ou abaixo do limite mínimo.
- **Movimentação de Inventário:** Registro de entradas e saídas de estoque.
- **Exportação de Relatórios:** Exportação dos dados de estoque em formato CSV.

## 🛠️ Tecnologias Utilizadas

- **Plataforma:** .NET 8 (C#)
- **Interface Gráfica:** WPF com [WPF-UI](https://github.com/lepoco/wpfui)
- **Arquitetura:** MVVM
- **ORM:** Entity Framework Core
- **Banco de Dados:** PostgreSQL

## 📂 Estrutura do Projeto

```text
├── Data/
│   ├── Context/          # DbContext do Entity Framework Core
│   └── Migrations/       # Histórico de migrações do banco de dados
├── Models/               # Entidades de domínio
├── Services/             # Regras de negócio e serviços
├── ViewModels/           # Lógica de apresentação e comandos MVVM
├── Views/                # Telas, janelas e controles XAML
├── appsettings.json      # Configurações da aplicação
└── App.xaml              # Inicialização da aplicação
```

## ⚙️ Pré-requisitos e Execução

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- Servidor [PostgreSQL](https://www.postgresql.org/) ativo.

### Passo a Passo

1. **Clone o repositório:**

   ```bash
   git clone <URL_DO_REPOSITORIO>
   cd sistema-estoque
   ```

2. **Configure a conexão com o banco:**

   No arquivo `appsettings.json`, configure a string de conexão do PostgreSQL.

   Exemplo:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=sua_senha"
     }
   }
   ```

3. **Execute as migrações do Entity Framework:**

   ```bash
   dotnet ef database update
   ```

4. **Compile e execute o projeto:**

   ```bash
   dotnet run
   ```

## 📄 Licença

Projeto desenvolvido para fins de controle interno e gerenciamento de estoque. Distribuído sob a licença MIT.
