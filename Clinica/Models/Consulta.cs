using System;
using System.Collections.Generic;

namespace Clinica.Models;

public partial class Consulta
{
    public int Codigo { get; set; }

    public DateTime DataHora { get; set; }

    public string StatusConsulta { get; set; } = null!;

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public virtual Médico? Medico { get; set; }

    public virtual Paciente? Paciente { get; set; }
}
