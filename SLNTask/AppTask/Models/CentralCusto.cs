using System.ComponentModel.DataAnnotations;

namespace AppTask.Models
{
    public class CentralCusto
    {
        [Key]
        public int Codigo { get; set; }

        public string NomeCusto { get; set; } = null!;

        public Decimal ValorAnualMeta { get; set; }
    }
}
