using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models.DTOs
{
    public class ObrisiPacijentaDto
    {
        [Required]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "JMBG must be exactly 13 digits.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG must contain only digits.")]
        public string JMBG { get; set; }
    }
}