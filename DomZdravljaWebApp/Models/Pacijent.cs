using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models
{
    public class Pacijent : Osoba
    {
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must be exactly 13 digits.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG must contain only digits.")]
        public string JMBG { get; set; }

        [Required]
        [EmailAddress]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public List<Termin> ZakazaniTermini { get; set; }
    }
}