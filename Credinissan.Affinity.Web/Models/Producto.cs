using Credinissan.Affinity.Web.Data.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool AllUsers { get; set; }
        public bool AllVersion { get; set; }
        public Cotizacion.TipoCredito TipoCredito { get; set; }
        public List<Plazo> Plazos { get; set; }
        public List<Tasa> Tasas { get; set; }
        public  List<Data.DataObject.Version> Versions { get; set; }
        public List<Usuario> Users { get; set; }


    }
}