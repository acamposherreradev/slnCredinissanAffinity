using Credinissan.Affinity.Web.Data.DataObject;
using Credinissan.Affinity.Web.Data.Models;
using Credinissan.Affinity.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Credinissan.Affinity.Web.Controllers
{
    public class EvaluacionController : Controller
    {
        // GET: Evaluacion
        public ActionResult Index(string msgExito)
        {

            Context context = new Context();

            ViewBag.Exito = msgExito;

            ViewBag.Cotizaciones = context.Cotizaciones.Where(c => c.Solicitud.EstadoSolicitud == Enums.EstadoSolicitud.EnEvaluacion).ToList();

            return View();
        }


        public ActionResult Evaluate(int id)
        {

            Context context = new Context();
            ViewBag.Cotizacion = context.Cotizaciones.Find(id);
            ViewBag.DocumentosEvaluacion = GetDocumentosEvaluacion();
            ViewBag.MotivosRechazo = GetMotivosRechazo();

            return View();
        }

        [HttpPost]
        public ActionResult Aprobar(int idCotizacion, string comentario, int[] documentos)
        {

            Context context = new Context();
            Data.DataObject.Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);
            cotizacion.Solicitud.EstadoSolicitud = Enums.EstadoSolicitud.AprobadaConCondiciones;
            cotizacion.Solicitud.FechaEvaluacion = DateTime.Now;
            cotizacion.Solicitud.UsuarioEvaluacion = context.Usuarios.FirstOrDefault(u => u.Username.Equals(User.Identity.Name));
            List<DocumentosEvaluacion> docs = GetDocumentosEvaluacion().Where(d=> documentos.Contains(d.Id)).ToList() ;
            cotizacion.Solicitud.ComentarioEvaluacion = comentario;
            cotizacion.Solicitud.DocumentosSolicitados = JsonConvert.SerializeObject(docs);

            context.SaveChanges();

            return RedirectToAction("Index", new { msgExito = "Evaluación completada. Solicitud " + idCotizacion + " ha sido APROBADA" });

        }
         

        [HttpPost]
        public ActionResult Rechazar(int idCotizacion, string comentario, int[] motivos)
        {

            Context context = new Context();
            Data.DataObject.Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);
            cotizacion.Solicitud.EstadoSolicitud = Enums.EstadoSolicitud.Rechazada;
            cotizacion.Solicitud.FechaEvaluacion = DateTime.Now;
            cotizacion.Solicitud.UsuarioEvaluacion = context.Usuarios.FirstOrDefault(u => u.Username.Equals(User.Identity.Name));
            List<MotivoRechazo> motivoRechazos = GetMotivosRechazo().Where(d => motivos.Contains(d.Id)).ToList();
            cotizacion.Solicitud.ComentarioEvaluacion = comentario;
            cotizacion.Solicitud.MotivosRechazo = JsonConvert.SerializeObject(motivoRechazos);

            context.SaveChanges();

            return RedirectToAction("Index", new {msgExito = "Evaluación completada. Solicitud " + idCotizacion + " ha sido RECHAZADA" });
        }

        private List<DocumentosEvaluacion> GetDocumentosEvaluacion()
        {
            string path = Server.MapPath("/") + "Parameters/DocumentosEvaluacion.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<DocumentosEvaluacion>>(json);

            }

        }

        private List<MotivoRechazo> GetMotivosRechazo()
        {
            string path = Server.MapPath("/") + "Parameters/MotivoRechazo.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<MotivoRechazo>>(json);

            }

        }

    }
}