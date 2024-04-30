using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Rol
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string DefaultView { get; set; }
        public string DefaultControlller { get; set; }

    }
}