using System;
using System.ComponentModel.DataAnnotations;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class UpdatePacijentDto
    {
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must be exactly 13 digits.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG must contain only digits.")]
        public string JMBG { get; set; }

        [Required]
        [StringLength(50)]
        public string Ime { get; set; }

        [Required]
        [StringLength(50)]
        public string Prezime { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DatumRodjenja { get; set; }

        [Required]
        [StringLength(50)]
        public string Sifra { get; set; }
    }
}
