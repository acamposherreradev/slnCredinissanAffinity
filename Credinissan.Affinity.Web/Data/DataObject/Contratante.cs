using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Contratante
    {
        public int Id { get; set; }
        public Enums.TipoContratante TipoContratante { get; set; }
        public Enums.TipoPersona TipoPersona { get; set; }
        public string Rut { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Enums.Nacionalidad Nacionalidad { get; set; }
        public Enums.EstadoCivil EstadoCivil { get; set; }
        public string Direccion { get; set; }
        public virtual Comuna Comuna { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Actividad { get; set; }
        public Enums.TipoTrabajador TipoTrabajador { get; set; }
        public int Renta { get; set; }
        public DateTime FechaInicioActividad { get; set; }
        public string RutEmpleador { get; set; }
        public string NombreEmpleador { get; set; }





    }
}