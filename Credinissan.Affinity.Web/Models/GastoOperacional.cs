using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Models
{
    public class GastoOperacional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Tolat { get; set; }
        public List<ItemGasto> Items { get; set; }
    }

    public class ItemGasto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Valor { get; set; }
    }

}