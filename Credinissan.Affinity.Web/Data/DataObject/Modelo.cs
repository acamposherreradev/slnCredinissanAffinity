using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Modelo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual Marca Marca { get; set; }
    }
}