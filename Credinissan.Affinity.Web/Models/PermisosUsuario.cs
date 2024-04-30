using Credinissan.Affinity.Web.Data.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Models
{
    public class PermisosUsuario
    {

        public virtual Usuario Usuario { get; set; }
        public virtual Rol Rol { get; set; }
        public bool Asignado { get; set; }
    }
}