using System;
using System.Collections.Generic;

namespace Clinica.Models;

public partial class Médico
{
    public int Codigo { get; set; }

    public string Nome { get; set; } = null!;

    public string Crm { get; set; } = null!;

    public string Especialidade { get; set; } = null!;

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();
}
