# Sistema de Controle de Estoque

Aplicação desktop moderna para gerenciamento, controle financeiro e monitoramento de inventário em tempo real, desenvolvida em **C# / .NET 8** utilizando **WPF-UI**, arquitetura **MVVM** e banco de dados **PostgreSQL** com **Entity Framework Core**.

---

## 🚀 Funcionalidades

- **Dashboard de KPIs:** Visão consolidada de métricas financeiras, valor total do inventário e indicadores de desempenho.
- **Gestão de Produtos (CRUD):** Cadastro completo, edição, listagem e remoção de produtos, categorias e fornecedores.
- **Alertas Visuais Inteligentes:** Sinalização dinâmica de itens com estoque baixo ou abaixo do limite mínimo de segurança.
- **Ajustes Rápidos de Inventário:** Entradas e saídas operacionais ágeis com persistência imediata no banco de dados.
- **Exportação de Relatórios:** Geração e exportação dos dados de estoque em formato CSV.

---

## 🛠️ Tecnologias Utilizadas

- **Plataforma:** .NET 8 (C#)
- **Interface Gráfica:** WPF com biblioteca de componentes [WPF-UI](https://github.com/lepoco/wpfui) (Fluent Design)
- **Padrão de Arquitetura:** MVVM (Model-View-ViewModel)
- **ORM:** Entity Framework Core
- **Banco de Dados:** PostgreSQL

---

## 📂 Estrutura do Projeto

```text
├── Data/
│   ├── Context/          # DbContext do Entity Framework Core
│   └── Migrations/       # Histórico de migrações do banco de dados
├── Models/               # Entidades de domínio (Produto, Categoria, Movimentação)
├── Services/             # Regras de negócio, serviços de exportação CSV e cálculos
├── ViewModels/           # Lógica de apresentação, comandos e observáveis MVVM
├── Views/                # Telas, janelas e controles XAML
├── appsettings.json      # Configurações da aplicação e connection strings
└── App.xaml              # Inicialização e injeção de dependências
⚙️ Pré-requisitos e Execução
Pré-requisitos
.NET 8 SDK instalado.

Servidor PostgreSQL ativo.

Passo a Passo
Clonar o repositório:

Bash
git clone [https://github.com/seu-usuario/sistema-estoque.git](https://github.com/seu-usuario/sistema-estoque.git)
cd sistema-estoque
Configurar a conexão com o banco:
No arquivo appsettings.json, ajuste a string de conexão:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=sua_senha"
  }
}
Executar as migrações do Entity Framework:

Bash
dotnet ef database update
Compilar e executar o projeto:

Bash
dotnet run
📄 Licença
Projeto desenvolvido para fins de controle interno e gerenciamento de estoque. Distribuído sob a licença MIT.


Para utilizá-lo, crie um arquivo com o nome `README.md` na raiz do diretório do seu projeto e cole o bloco de código acima.
faça e me envie o download

⚙️ Pré-requisitos e Execução
Pré-requisitos
.NET 8 SDK instalado.

Servidor PostgreSQL ativo.

Passo a Passo
Clonar o repositório:

Bash
git clone [https://github.com/seu-usuario/sistema-estoque.git](https://github.com/seu-usuario/sistema-estoque.git)
cd sistema-estoque
Configurar a conexão com o banco:
No arquivo appsettings.json, ajuste a string de conexão:

JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=sua_senha"
  }
}
Executar as migrações do Entity Framework:

Bash
dotnet ef database update
Compilar e executar o projeto:

Bash
dotnet run
📄 Licença
Projeto desenvolvido para fins de controle interno e gerenciamento de estoque. Distribuído sob a licença MIT.
"""

file_path = "README.md"
with open(file_path, "w", encoding="utf-8") as f:
f.write(readme_content)

print(f"File created successfully: {file_path}")


```text?code_stdout&code_event_index=1
File created successfully: README.md

O seu arquivo Markdown (README.md) está pronto:

Ícone do app MD
README
 MD 
O arquivo contém a documentação completa do Sistema de Controle de Estoque (.NET 8 / WPF-UI / PostgreSQL), incluindo visão geral, funcionalidades, arquitetura MVVM, estrutura de pastas e guia passo a passo de configuração e execução. Basta clicar no link acima para fazer o download.




O Gemini é uma IA e pode cometer erros.

Outlining the Project Structure
# Sistema de Controle de Estoque

Aplicação desktop moderna para gerenciamento, controle financeiro e monitoramento de inventário em tempo real, desenvolvida em **C# / .NET 8** utilizando **WPF-UI**, arquitetura **MVVM** e banco de dados **PostgreSQL** com **Entity Framework Core**.

---

## 🚀 Funcionalidades

- **Dashboard de KPIs:** Visão consolidada de métricas financeiras, valor total do inventário e indicadores de desempenho.
- **Gestão de Produtos (CRUD):** Cadastro completo, edição, listagem e remoção de produtos, categorias e fornecedores.
- **Alertas Visuais Inteligentes:** Sinalização dinâmica de itens com estoque baixo ou abaixo do limite mínimo de segurança.
- **Ajustes Rápidos de Inventário:** Entradas e saídas operacionais ágeis com persistência imediata no banco de dados.
- **Exportação de Relatórios:** Geração e exportação dos dados de estoque em formato CSV.

---

## 🛠️ Tecnologias Utilizadas

- **Plataforma:** .NET 8 (C#)
- **Interface Gráfica:** WPF com biblioteca de componentes [WPF-UI](https://github.com/lepoco/wpfui) (Fluent Design)
- **Padrão de Arquitetura:** MVVM (Model-View-ViewModel)
- **ORM:** Entity Framework Core
- **Banco de Dados:** PostgreSQL

---

## 📂 Estrutura do Projeto

```text
├── Data/
│   ├── Context/          # DbContext do Entity Framework Core
│   └── Migrations/       # Histórico de migrações do banco de dados
├── Models/               # Entidades de domínio (Produto, Categoria, Movimentação)
├── Services/             # Regras de negócio, serviços de exportação CSV e cálculos
├── ViewModels/           # Lógica de apresentação, comandos e observáveis MVVM
├── Views/                # Telas, janelas e controles XAML
├── appsettings.json      # Configurações da aplicação e connection strings
└── App.xaml              # Inicialização e injeção de dependências
```

---

## ⚙️ Pré-requisitos e Execução

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
- Servidor [PostgreSQL](https://www.postgresql.org/) ativo.

### Passo a Passo

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/seu-usuario/sistema-estoque.git
   cd sistema-estoque
   ```

2. **Configurar a conexão com o banco:**
   No arquivo `appsettings.json`, ajuste a string de conexão:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=estoque_db;Username=postgres;Password=sua_senha"
     }
   }
   ```

3. **Executar as migrações do Entity Framework:**
   ```bash
   dotnet ef database update
   ```

4. **Compilar e executar o projeto:**
   ```bash
   dotnet run
   ```

---

## 📄 Licença

Projeto desenvolvido para fins de controle interno e gerenciamento de estoque. Distribuído sob a licença MIT.
