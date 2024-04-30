using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public string Rut { get; set; }

        public virtual Sucursal Sucursal { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Clave { get; set; }

        public string Preferencias { get; set; }

        public Enums.EstadoUsuario Estado { get; set; }

        public int CantidadIntentos { get; set; }

        public string Foto { get; set; }

        public string Token { get; set; }

        public bool AprobarTerminosCondiciones { get; set; }


    }
}