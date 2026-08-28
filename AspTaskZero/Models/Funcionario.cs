using System;
using System.Collections.Generic;

namespace AspTaskZero.Models;

public partial class Funcionario
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public int? IdGerente { get; set; }

    public virtual Funcionario? IdGerenteNavigation { get; set; }

    public virtual ICollection<Funcionario> InverseIdGerenteNavigation { get; set; } = new List<Funcionario>();

    public virtual ICollection<Tarefa> Tarefas { get; set; } = new List<Tarefa>();
}
