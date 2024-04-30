using Credinissan.Affinity.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Credinissan.Affinity.Web.Common
{
    public class Security
    {
        public List<PermisosUsuario> Permisos { get; set; }
        public Security(List<PermisosUsuario> permisos)
        {
            this.Permisos = permisos;
        }
        public bool IsInRol( string rol) 
        {
            if (this.Permisos == null)
                return false;

            PermisosUsuario perm = this.Permisos.FirstOrDefault(p => p.Rol.Descripcion.Equals(rol));

            if (perm == null)
                return false;

            return (perm.Asignado);

        }
    }
}