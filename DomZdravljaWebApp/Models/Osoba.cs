using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models
{
    public abstract class Osoba
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        public string KorisnickoIme { get; set; }
        public string Sifra { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public Role Role { get; set; }
        public DateTime DatumRodjenja { get; set; }
    }
}