using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class CreatePersonDTO
    {
        [Required(ErrorMessage = "El campo FirstName es obligatorio.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo PaternalLastname es obligatorio.")]
        [StringLength(50)]
        public string PaternalLastname { get; set; }

        [StringLength(50)]
        public string? MaternalLastname { get; set; }

        [Required(ErrorMessage = "El campo Identification es obligatorio.")]
        [StringLength(50)]
        public string Identification { get; set; }
    }
}
