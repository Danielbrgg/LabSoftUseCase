# LabUseCase01 - Desenvolvimento ASP.NET Core .NET 8 (Database First)

Seja bem-vindo ao **LabUseCase01**! Neste laboratório prático, você trabalhará com um projeto ASP.NET Core .NET 8 MVC pré-configurado utilizando a abordagem **Database First** do Entity Framework Core.

O projeto já possui a estrutura base pronta, a conexão com o SQL Server configurada no `appsettings.json` e o **CRUD de Tarefas** 100% funcional. Sua missão será testar o sistema atual, criar o **CRUD de Funcionários** e implementar o módulo de **Incidentes**.

---

## 📋 Modelo de Dados Inicial

O banco de dados do sistema é o `dbTasks`. Ele possui duas tabelas relacionadas (**1 Funcionário : N Tarefas**):

### Tabela `Funcionario`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Nome`**: `VARCHAR(100)` (Not Null)
* **`Cargo`**: `VARCHAR(50)` (Not Null)

### Tabela `Tarefa`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Descricao`**: `VARCHAR(200)` (Not Null)
* **`DataPlanejada`**: `DATETIME` (Not Null)[cite: 2]
* **`DataIniciada`**: `DATETIME` (Nullable)[cite: 2]
* **`DataFinalizada`**: `DATETIME` (Nullable)[cite: 2]
* **`DataCancelada`**: `DATETIME` (Nullable)[cite: 2]
* **`StatusTarefa`**: `VARCHAR(30)` (Not Null)[cite: 2]
* **`Prazo`**: `VARCHAR(20)` (Not Null)[cite: 2]
* **`CodigoFuncionario`**: `INT` (Foreign Key -> `Funcionario.Codigo`)[cite: 2]

---

## 🚀 Passo 1: Preparação do Banco de Dados Inicial

1. Abra o **SQL Server Management Studio (SSMS)** ou o **Azure Data Studio**[cite: 2].
2. Conecte-se à sua instância local do SQL Server[cite: 2].
3. Execute o script SQL abaixo para criar o banco de dados `dbTasks` e as tabelas iniciais[cite: 2]:

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

-- Dados Iniciais para Teste
INSERT INTO Funcionario (Nome, Cargo) VALUES 
('Carlos Silva', 'Desenvolvedor Senior'),
('Ana Oliveira', 'Analista de QA'),
('Roberto Santos', 'Gerente de Projetos');

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, CodigoFuncionario) VALUES 
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);
GO