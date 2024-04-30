using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Sucursal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual Distribuidor Distribuidor { get; set; }
    }
}