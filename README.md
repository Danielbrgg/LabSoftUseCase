# LabUseCase02 - Refatoração do Front-end (HTML Puro & Bootstrap)

Seja bem-vindo ao LabUseCase02! Nesta aula prática, focaremos nos fundamentos do desenvolvimento web. Vamos abandonar os Tag Helpers automáticos do ASP.NET Core e trabalhar diretamente com HTML Puro, aplicando também o framework Bootstrap para estilizar a interface de forma profissional.

---

## 🚀 PREPRAÇÃO: Preparação do Banco de Dados Inicial

Só faça esse passo caso ainda não tenha o banco de dados dbTasks em seu computador.

1. Abra o **SQL Server Management Studio (SSMS)** ou o **Azure Data Studio**.
2. Conecte-se à sua instância local do SQL Server.
3. Execute o script SQL abaixo para criar o banco de dados `dbTasks` e as tabelas iniciais:

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
    FuncionarioId INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario FOREIGN KEY (FuncionarioId) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Inserindo Dados Iniciais para Teste
INSERT INTO Funcionario (Nome, Cargo) VALUES 
('Carlos Silva', 'Desenvolvedor Senior'),
('Ana Oliveira', 'Analista de QA'),
('Roberto Santos', 'Gerente de Projetos');

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, FuncionarioId) VALUES 
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);
GO
```

---

## 🛠️ Passo 1: Configuração da Nova Branch

Antes de colocar a mão no código, precisamos garantir que o projeto está organizado seguindo o fluxo de ramificações (GitFlow).
1. Certifique-se de que você está na sua branch principal de desenvolvimento (develop) atualizada.
2. Caso ainda não tenha feito o fork ou precise sincronizar sua base, garanta que seu repositório local esteja atualizado.
3. Crie e mude para uma nova branch específica para esta atividade chamada LabSofUseCase02-TaskFrontSemRazor.

---

## 📝 Passo 2: Migração do Front-end para HTML Puro

O ASP.NET MVC utiliza recursos do Razor (como asp-action e propriedades automáticas) para agilizar o desenvolvimento. Porém, entender como o HTML bruto se comunica com as rotas do servidor é essencial para qualquer desenvolvedor.

Abra o arquivo Views/Tarefa/Index.cshtml do seu projeto, substitua todo o conteúdo atual pelo código em HTML puro abaixo e salve o arquivo:


```html
@model IEnumerable<AppTask.Models.Tarefa>

@{
    ViewData["Title"] = "Index";
}

<h1>Index</h1>

<p>
    <a href="/Tarefa/Create">Create New</a>
</p>
<table class="table">
    <thead>
        <tr>
            <th>Descrição</th>
            <th>Data Planejada</th>
            <th>Data Iniciada</th>
            <th>Data Finalizada</th>
            <th>Data Cancelada</th>
            <th>Status da Tarefa</th>
            <th>Prazo</th>
            <th>Funcionário</th>
            <th></th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model) {
            <tr>
                <td>@item.Descricao</td>
                <td>@item.DataPlanejada</td>
                <td>@item.DataIniciada</td>
                <td>@item.DataFinalizada</td>
                <td>@item.DataCancelada</td>
                <td>@item.StatusTarefa</td>
                <td>@item.Prazo</td>
                <td>@item.Funcionario?.Nome</td>
                <td>
                    <a href="/Tarefa/Edit/@item.Codigo">Edit</a> |
                    <a href="/Tarefa/Details/@item.Codigo">Details</a> |
                    <a href="/Tarefa/Delete/@item.Codigo">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

---

## ⚡🎨 Passo 3: Customizando o Front-end com Bootstrap

Agora que a estrutura está em HTML puro, vamos aplicar classes de estilo do Bootstrap para transformar a tabela e os links em botões modernos e amigáveis.

Substitua novamente o conteúdo do arquivo Views/Tarefa/Index.cshtml pelo código estilizado abaixo:


```html
@model IEnumerable<AppTask.Models.Tarefa>

@{
    ViewData["Title"] = "Index";
}

<h1 class="mb-4">Lista de Tarefas</h1>

<p>
    <a href="/Tarefa/Create" class="btn btn-success text-white">Novo</a>
</p>

<table class="table table-striped table-hover">
    <thead class="table-dark">
        <tr>
            <th>Descrição</th>
            <th>Data Planejada</th>
            <th>Data Iniciada</th>
            <th>Data Finalizada</th>
            <th>Data Cancelada</th>
            <th>Status</th>
            <th>Prazo</th>
            <th>Funcionário</th>
            <th>Ações</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model) {
            <tr>
                <td>@item.Descricao</td>
                <td>@item.DataPlanejada</td>
                <td>@item.DataIniciada</td>
                <td>@item.DataFinalizada</td>
                <td>@item.DataCancelada</td>
                <td>@item.StatusTarefa</td>
                <td>@item.Prazo</td>
                <td>@item.Funcionario?.Nome</td>
                <td>
                    <a href="/Tarefa/Edit/@item.Codigo" class="btn btn-primary btn-sm text-white">Alterar</a>
                    <a href="/Tarefa/Details/@item.Codigo" class="btn btn-success btn-sm text-white" style="background-color: #006400;">Detalhe</a>
                    <a href="/Tarefa/Delete/@item.Codigo" class="btn btn-danger btn-sm text-white">Delete</a>
                </td>
            </tr>
        }
    </tbody>
</table>

```

## 🗑️ Passo 4: Implementando a Confirmação de Exclusão com Modal do Bootstrap

Para melhorar a experiência do usuário, vamos substituir o redirecionamento para a página de Delete por uma caixa de diálogo elegante (Modal) utilizando os componentes nativos do Bootstrap.

Atualmente para excluir uma tarefa você clica em delete e ele envia para outra página.
![Fluxo de exclusão atual](./imagens/fluxoexclusaoatual.jpg)


Nosso objetivo é ao clicar em delete, ele abrir um modal e ao confirmar excluir o elemento
![Fluxo Proposto](./imagens/fluxocommodal.jpg)

#### 4.1 Adicionando caix de DIALOGO

Substituia o código que está no botão delete para ficar assim.

```html
 <button type="button" class="btn btn-danger btn-sm text-white" data-bs-toggle="modal" data-bs-target="#deleteModal" data-id="@item.Codigo" data-descricao="@item.Descricao">
 Delete
</button>

```

Vamos adicionar  o modal para ficar similar a imagem a seguir:

![Fluxo Modal](./imagens/fluxocommodal.jpg)


Adicione esse código na index de tarefa 

```html
<!-- Modal de Confirmação de Exclusão -->
<div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header bg-danger text-white">
                <h5 class="modal-title" id="deleteModalLabel">Confirmação de Exclusão</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                Deseja realmente excluir a tarefa: <strong id="tarefaDescricao"></strong>?
            </div>
            <div class="modal-footer">
                <button type="button" class="modal-btn-cancel btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <!-- Formulário que dispara o POST de Delete para a Controller -->
                <form id="deleteForm" method="post" action="">
                    @Html.AntiForgeryToken()
                    <button type="submit" class="btn btn-danger">Sim, Excluir</button>
                </form>
            </div>
        </div>
    </div>
</div>

```

Rode sua aplicação. Ao tentar rodar agora ele exibe O MODAL, mas se confirmar não vai funcionar. Para que funcione iremos para o proximo passo, que é JavaScript