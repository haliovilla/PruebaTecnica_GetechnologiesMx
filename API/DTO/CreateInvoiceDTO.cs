using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CreateInvoiceDTO
    {
        [Required(ErrorMessage = "El campo Date es obligatorio.")]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = "El campo Amount es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo Amount debe ser mayor a 0.")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "El campo PersonId es obligatorio.")]
        public int? PersonId { get; set; }
    }
}
