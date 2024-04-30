using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Models
{
    public class Tasa
    {
        public int Id { get; set; }
        public int UFMin { get; set; }
        public int UFMax { get; set; }

        public int PlazoMin { get; set; }
        public int PlazoMax { get; set; }
        public double TasaPagare { get; set; }
        public double TasaPiso { get; set; }
    }
}