using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Solicitud
    {
        public int Id { get; set; }
        public virtual List<Contratante> Contratantes { get; set; }
        public Enums.EstadoSolicitud EstadoSolicitud { get; set; }
        public DateTime FechaEnvioSolicitud { get; set; }
        public DateTime FechaEvaluacion { get; set; }
        public virtual Usuario UsuarioEvaluacion { get; set; }
        public virtual Usuario UsuarioSolicitante { get; set; }

        public string DocumentosSolicitados { get; set; }
        public string MotivosRechazo { get; set; }
        public string ComentarioEvaluacion { get; set; }

        public int IdMonaco { get; set; }

    }
}