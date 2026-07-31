# LabUseCase01 - Desenvolvimento ASP.NET Core .NET 8 (Database First)

Seja bem-vindo ao **LabUseCase01**! Neste laboratório prático, você trabalhará com um projeto ASP.NET Core .NET 8 MVC pré-configurado utilizando a abordagem **Database First** do Entity Framework Core.

O projeto já possui a estrutura completa, a conexão com o SQL Server e o **CRUD de Tarefas** 100% funcional. Sua missão será implementar o **CRUD de Funcionários** seguindo a mesma arquitetura e padrões apresentados.

---

## 📋 Arquitetura e Modelagem do Banco de Dados

O banco de dados utilizado é o `dbTasks`. Ele possui duas tabelas relacionadas (**1 Funcionário : N Tarefas**):

### Tabela `Funcionario`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Nome`**: `VARCHAR(100)` (Not Null)
* **`Cargo`**: `VARCHAR(50)` (Not Null)

### Tabela `Tarefa`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Descricao`**: `VARCHAR(200)` (Not Null)
* **`DataPlanejada`**: `DATETIME` (Not Null)
* **`DataIniciada`**: `DATETIME` (Nullable)
* **`DataFinalizada`**: `DATETIME` (Nullable)
* **`DataCancelada`**: `DATETIME` (Nullable)
* **`StatusTarefa`**: `VARCHAR(30)` (Not Null) - *Ex: Pendente, Em Andamento, Concluída, Cancelada*
* **`Prazo`**: `VARCHAR(20)` (Not Null) - *Ex: Em dia, Em atraso*
* **`CodigoFuncionario`**: `INT` (Foreign Key -> `Funcionario.Codigo`)

---

## 🚀 Passo 1: Preparação do Banco de Dados (SQL Server)

1. Abra o **SQL Server Management Studio (SSMS)** ou o **Azure Data Studio**.
2. Conecte-se à sua instância local do SQL Server com a autenticação do usuário `sa`.
3. Execute o script SQL abaixo para criar o banco `dbTasks`, as tabelas e inserir dados de teste iniciais:

```sql
CREATE DATABASE dbTasks;
GO

USE dbTasks;
GO

-- Tabela Funcionario
CREATE TABLE Funcionario (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL
);
GO

-- Tabela Tarefa
CREATE TABLE Tarefa (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Descricao VARCHAR(200) NOT NULL,
    DataPlanejada DATETIME NOT NULL,
    DataIniciada DATETIME NULL,
    DataFinalizada DATETIME NULL,
    DataCancelada DATETIME NULL,
    StatusTarefa VARCHAR(30) NOT NULL,
    Prazo VARCHAR(20) NOT NULL,
    CodigoFuncionario INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario FOREIGN KEY (CodigoFuncionario) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Inserindo Dados Iniciais para Teste
INSERT INTO Funcionario (Nome, Cargo) VALUES 
('Carlos Silva', 'Desenvolvedor Senior'),
('Ana Oliveira', 'Analista de QA'),
('Roberto Santos', 'Gerente de Projetos');

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, CodigoFuncionario) VALUES 
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);
GO
```

---

## 🛠️ Passo 2: Estrutura do Projeto Pré-Pronto

Abaixo está a estrutura do projeto disponibilizada no repositório na branch `LabUseCase01`.

### 1. `appsettings.json`
Certifique-se de ajustar a senha do usuário `sa` na Connection String caso a sua senha local seja diferente:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConexaoSqlServer": "Server=LOCALHOST;Database=dbTasks;User Id=sa;Password=SUA_SENHA_AQUI;TrustServerCertificate=True;"
  }
}
```

---

### 2. Models (Mapeamento do Entity Framework)

#### `Models/Funcionario.cs`
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabUseCase01.Models;

public partial class Funcionario
{
    [Key]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O cargo é obrigatório.")]
    [StringLength(50)]
    public string Cargo { get; set; } = null!;

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
```

#### `Models/Tarefa.cs`
```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabUseCase01.Models;

public partial class Tarefa
{
    [Key]
    public int Codigo { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(200)]
    [Display(Name = "Descrição")]
    public string Descricao { get; set; } = null!;

    [Required(ErrorMessage = "A data planejada é obrigatória.")]
    [DataType(DataType.Date)]
    [Display(Name = "Data Planejada")]
    public DateTime DataPlanejada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Iniciada")]
    public DateTime? DataIniciada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Finalizada")]
    public DateTime? DataFinalizada { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Cancelada")]
    public DateTime? DataCancelada { get; set; }

    [Required]
    [Display(Name = "Status")]
    public string StatusTarefa { get; set; } = null!;

    [Required]
    public string Prazo { get; set; } = null!;

    [Display(Name = "Funcionário Responsável")]
    public int CodigoFuncionario { get; set; }

    [ForeignKey("CodigoFuncionario")]
    [Display(Name = "Funcionário")]
    public virtual Funcionario? Funcionario { get; set; }
}
```

#### `Models/DbTasksContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;

namespace LabUseCase01.Models;

public partial class DbTasksContext : DbContext
{
    public DbTasksContext()
    {
    }

    public DbTasksContext(DbContextOptions<DbTasksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }
    public virtual DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Funcionario");
            entity.Property(e => e.Cargo).HasMaxLength(50).IsUnicode(false);
            entity.Property(e => e.Nome).HasMaxLength(100).IsUnicode(false);
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.Codigo);
            entity.ToTable("Tarefa");
            entity.Property(e => e.DataCancelada).HasColumnType("datetime");
            entity.Property(e => e.DataFinalizada).HasColumnType("datetime");
            entity.Property(e => e.DataIniciada).HasColumnType("datetime");
            entity.Property(e => e.DataPlanejada).HasColumnType("datetime");
            entity.Property(e => e.Descricao).HasMaxLength(200).IsUnicode(false);
            entity.Property(e => e.Prazo).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.StatusTarefa).HasMaxLength(30).IsUnicode(false);

            entity.HasOne(d => d.Funcionario)
                .WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.CodigoFuncionario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tarefa_Funcionario");
        });
    }
}
```

---

### 3. Configuração Principal (`Program.cs`)

```csharp
using LabUseCase01.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuração da Injeção de Dependência do DbContext
builder.Services.AddDbContext<DbTasksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoSqlServer")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tarefas}/{action=Index}/{id?}");

app.Run();
```

---

### 4. Controller do CRUD de Tarefas (`Controllers/TarefasController.cs`)

```csharp
using LabUseCase01.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LabUseCase01.Controllers
{
    public class TarefasController : Controller
    {
        private readonly DbTasksContext _context;

        public TarefasController(DbTasksContext context)
        {
            _context = context;
        }

        // GET: Tarefas
        public async Task<IActionResult> Index()
        {
            var dbTasksContext = _context.Tarefas.Include(t => t.Funcionario);
            return View(await dbTasksContext.ToListAsync());
        }

        // GET: Tarefas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (tarefa == null) return NotFound();

            return View(tarefa);
        }

        // GET: Tarefas/Create
        public IActionResult Create()
        {
            ViewData["CodigoFuncionario"] = new SelectList(_context.Funcionarios, "Codigo", "Nome");
            return View();
        }

        // POST: Tarefas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,CodigoFuncionario")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tarefa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoFuncionario"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.CodigoFuncionario);
            return View(tarefa);
        }

        // GET: Tarefas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return NotFound();

            ViewData["CodigoFuncionario"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.CodigoFuncionario);
            return View(tarefa);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Descricao,DataPlanejada,DataIniciada,DataFinalizada,DataCancelada,StatusTarefa,Prazo,CodigoFuncionario")] Tarefa tarefa)
        {
            if (id != tarefa.Codigo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarefa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarefaExists(tarefa.Codigo)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoFuncionario"] = new SelectList(_context.Funcionarios, "Codigo", "Nome", tarefa.CodigoFuncionario);
            return View(tarefa);
        }

        // GET: Tarefas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tarefa = await _context.Tarefas
                .Include(t => t.Funcionario)
                .FirstOrDefaultAsync(m => m.Codigo == id);

            if (tarefa == null) return NotFound();

            return View(tarefa);
        }

        // POST: Tarefas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa != null)
            {
                _context.Tarefas.Remove(tarefa);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TarefaExists(int id)
        {
            return _context.Tarefas.Any(e => e.Codigo == id);
        }
    }
}
```

---

### 5. Views do CRUD de Tarefas

#### `Views/Tarefas/Index.cshtml`
```html
@model IEnumerable<LabUseCase01.Models.Tarefa>

@{
    ViewData["Title"] = "Listagem de Tarefas";
}

<h1 class="my-4">Gerenciamento de Tarefas</h1>

<p>
    <a asp-action="Create" class="btn btn-primary">Nova Tarefa</a>
</p>

<table class="table table-striped table-hover">
    <thead class="table-dark">
        <tr>
            <th>@Html.DisplayNameFor(model => model.Descricao)</th>
            <th>@Html.DisplayNameFor(model => model.DataPlanejada)</th>
            <th>@Html.DisplayNameFor(model => model.StatusTarefa)</th>
            <th>@Html.DisplayNameFor(model => model.Prazo)</th>
            <th>@Html.DisplayNameFor(model => model.Funcionario)</th>
            <th>Ações</th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in model) {
        <tr>
            <td>@Html.DisplayFor(modelItem => item.Descricao)</td>
            <td>@item.DataPlanejada.ToString("dd/MM/yyyy")</td>
            <td>
                <span class="badge @(item.StatusTarefa == "Concluída" ? "bg-success" : "bg-warning text-dark")">
                    @item.StatusTarefa
                </span>
            </td>
            <td>
                <span class="badge @(item.Prazo == "Em dia" ? "bg-info" : "bg-danger")">
                    @item.Prazo
                </span>
            </td>
            <td>@Html.DisplayFor(modelItem => item.Funcionario.Nome)</td>
            <td>
                <a asp-action="Edit" asp-route-id="@item.Codigo" class="btn btn-sm btn-outline-warning">Editar</a> |
                <a asp-action="Details" asp-route-id="@item.Codigo" class="btn btn-sm btn-outline-info">Detalhes</a> |
                <a asp-action="Delete" asp-route-id="@item.Codigo" class="btn btn-sm btn-outline-danger">Excluir</a>
            </td>
        </tr>
}
    </tbody>
</table>
```

#### `Views/Tarefas/Create.cshtml`
```html
@model LabUseCase01.Models.Tarefa

@{
    ViewData["Title"] = "Cadastrar Tarefa";
}

<h1>Cadastrar Nova Tarefa</h1>
<hr />

<div class="row">
    <div class="col-md-6">
        <form asp-action="Create">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            
            <div class="mb-3">
                <label asp-for="Descricao" class="form-label"></label>
                <input asp-for="Descricao" class="form-control" />
                <span asp-validation-for="Descricao" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="DataPlanejada" class="form-label"></label>
                <input asp-for="DataPlanejada" class="form-control" type="date" />
                <span asp-validation-for="DataPlanejada" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="StatusTarefa" class="form-label"></label>
                <select asp-for="StatusTarefa" class="form-select">
                    <option value="Pendente">Pendente</option>
                    <option value="Em Andamento">Em Andamento</option>
                    <option value="Concluída">Concluída</option>
                    <option value="Cancelada">Cancelada</option>
                </select>
                <span asp-validation-for="StatusTarefa" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="Prazo" class="form-label"></label>
                <select asp-for="Prazo" class="form-select">
                    <option value="Em dia">Em dia</option>
                    <option value="Em atraso">Em atraso</option>
                </select>
                <span asp-validation-for="Prazo" class="text-danger"></span>
            </div>

            <div class="mb-3">
                <label asp-for="CodigoFuncionario" class="form-label"></label>
                <select asp-for="CodigoFuncionario" class="form-select" asp-items="ViewBag.CodigoFuncionario"></select>
            </div>

            <button type="submit" class="btn btn-success">Salvar</button>
            <a asp-action="Index" class="btn btn-secondary">Voltar</a>
        </form>
    </div>
</div>
```

---

## 🎯 Desafio Prático para os Alunos

### **Sua Tarefa (Alunos):**
Com o projeto base clonado e rodando perfeitamente em sua máquina, você deve desenvolver o **CRUD de Funcionários**.

### **Passos Obrigatórios:**
1. **Clonar e Mudar de Branch:**
   ```bash
   git clone <URL-DO-REPOSITORIO>
   cd LabUseCase01
   git checkout LabUseCase01
   ```
2. **Executar o Script SQL** no seu SQL Server local.
3. **Ajustar a Connection String** no `appsettings.json` com a sua senha do `sa`.
4. **Criar o Controller `FuncionariosController`:**
   * Crie o controller na pasta `Controllers`.
   * Implemente as Actions e Views correspondentes às operações do CRUD:
     * `Index` (Listar funcionários)
     * `Details` (Visualizar detalhes de um funcionário)
     * `Create` (GET/POST - Cadastrar novo funcionário)
     * `Edit` (GET/POST - Alterar dados do funcionário)
     * `Delete` (GET/POST - Excluir funcionário)
5. **Criar as Views** na pasta `Views/Funcionarios/`:
   * `Index.cshtml`
   * `Create.cshtml`
   * `Edit.cshtml`
   * `Details.cshtml`
   * `Delete.cshtml`
6. **Adicionar o Link no Menu de Navegação:**
   * Abra `Views/Shared/_Layout.cshtml` e adicione o item no menu para navegar até a controller de Funcionários:
   ```html
   <li class="nav-item">
       <a class="nav-link text-dark" asp-area="" asp-controller="Funcionarios" asp-action="Index">Funcionários</a>
   </li>
   ```

---

## 💡 Dica de Scaffold Automático para os Alunos

Caso seu ambiente esteja com o **Visual Studio** configurado, você pode gerar a Controller e Views do Funcionário automaticamente utilizando Scaffolding:

1. Clique com o botão direito na pasta **Controllers** > **Adicionar** > **Novo Item Scaffolded...**
2. Escolha **Controlador MVC com exibições, usando o Entity Framework**.
3. Selecione:
   * **Classe de Modelo:** `Funcionario (LabUseCase01.Models)`
   * **Classe do contexto de dados:** `DbTasksContext (LabUseCase01.Models)`
4. Clique em **Adicionar**.
