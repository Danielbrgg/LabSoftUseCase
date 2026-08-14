using System.ComponentModel.DataAnnotations;

namespace AppTask.Models
{
    public class Departamento
    {
        [Key]
        public int Codigo { get; set; }

        public string Descricao { get; set; } = null!;

        public Boolean Ativo { get; set; }
    }
}
