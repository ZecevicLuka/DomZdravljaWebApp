using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DomZdravljaWebApp.Models
{
    public class ZdravstveniKarton
    {
        public List<Termin> ListaTermina { get; set; }
        public Pacijent Pacijent { get; set; }
    }
}