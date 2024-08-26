using System;
using System.ComponentModel.DataAnnotations;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class PacijentDto
    {
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must be exactly 13 digits.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG must contain only digits.")]
        public string JMBG { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string Sifra { get; set; }
    }
}
