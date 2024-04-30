using Credinissan.Affinity.Web.Data.DataObject;
using Credinissan.Affinity.Web.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Credinissan.Affinity.Web.Controllers
{
    public class SolicitudController : Controller
    {
        // GET: Solicitud
        public ActionResult Index(int idCotizacion)
        {
            Context context = new Context();

            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);

            if (cotizacion.Solicitud == null)
                cotizacion.Solicitud = new Solicitud();

            if (cotizacion.Solicitud.Contratantes == null)
            {
                cotizacion.Solicitud.Contratantes = new List<Contratante>();
                cotizacion.Solicitud.Contratantes.Add(new Contratante
                {
                    TipoContratante = Enums.TipoContratante.Titular,
                    Actividad = "PMO",
                    ApMaterno = "ZUÑIGA",
                    ApPaterno = "CERDA",
                    Comuna = context.Comunas.ToList().First(),
                    Direccion = "HOLANDA 1125",
                    Email = "ROD@APPFITME.COM",
                    FechaInicioActividad = DateTime.Now.AddYears(-2),
                    FechaNacimiento = DateTime.Now.AddYears(-30),
                    Nombre = "RODRIGO FRANCISCO",
                    NombreEmpleador = "NTFS RETAIL",
                    Rut = "12674021-2",
                    Renta = 3000000,
                    Telefono = 966597065, RutEmpleador="98000000-1"
                });
            }
            else 
            {

                cotizacion.Solicitud.Contratantes.Add(new Contratante { TipoContratante = Enums.TipoContratante.Aval });
            }


            ViewBag.Solicitud = cotizacion.Solicitud;
            ViewBag.IdCotizacion = idCotizacion;

            return View();
        }

        public ActionResult Create(int idCotizacion)
        {
            Context context = new Context();

            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);

            ViewBag.IdCotizacion = idCotizacion;

            return View();
        }


        [HttpPost]
        public ActionResult SaveContratante(int idCotizacion,int idContratante, string rut, string apPaterno, string apMaterno, string nombre, DateTime fechaNacimiento, Enums.Nacionalidad nacionalidad, Enums.EstadoCivil estadoCivil, string direccion, string comuna, string email, int telefono, string actividad, Enums.TipoTrabajador tipoTrabajador, int renta, DateTime fechaInicioActividad, string rutEmpleador, string nombreEmpleador, Enums.TipoContratante tipoContratante)
        {
            Context context = new Context();

            string[] aux = rut.Split('-');
            Enums.TipoPersona tipoPersona = int.Parse(aux[0]) < 50000000 ? Enums.TipoPersona.Natural : Enums.TipoPersona.Juridica;

            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);
            Contratante contratante = new Contratante();

            if (idContratante == 0)
            {
                contratante = new Contratante();

                if (cotizacion.Solicitud == null)
                    cotizacion.Solicitud = new Solicitud();
                if (cotizacion.Solicitud.Contratantes == null)
                    cotizacion.Solicitud.Contratantes = new List<Contratante>();

                cotizacion.Solicitud.Contratantes.Add(contratante);
            }
            else
                contratante = context.Contratantes.Find(idContratante);

            contratante.Rut = rut;
            contratante.ApPaterno = apPaterno;
            contratante.ApMaterno = apMaterno;
            contratante.Nombre = nombre;
            contratante.FechaNacimiento = fechaNacimiento;
            contratante.Nacionalidad = nacionalidad;
            contratante.EstadoCivil = estadoCivil;
            contratante.Direccion = direccion;
            contratante.Comuna = context.Comunas.FirstOrDefault(c => c.Nombre.Equals(comuna));
            contratante.Email = email;
            contratante.Renta = renta;
            contratante.Telefono = telefono;
            contratante.Actividad = actividad;
            contratante.TipoTrabajador = tipoTrabajador;
            contratante.FechaInicioActividad = fechaInicioActividad;
            contratante.RutEmpleador = rutEmpleador;
            contratante.NombreEmpleador = nombreEmpleador;
            contratante.TipoContratante = tipoContratante;
            contratante.TipoPersona = tipoPersona;

            context.SaveChanges();

            return RedirectToAction("Index", new { idCotizacion });
        }

        [HttpPost]
        public ActionResult SendToEvaluate(int idCotizacion)
        {
            Context context = new Context();
            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);

            cotizacion.Solicitud.EstadoSolicitud = Enums.EstadoSolicitud.EnEvaluacion;
            cotizacion.Solicitud.FechaEnvioSolicitud = DateTime.Now;
            cotizacion.Solicitud.UsuarioSolicitante = context.Usuarios.FirstOrDefault(u => u.Username.Equals(User.Identity.Name));

            context.SaveChanges();

            return RedirectToAction("Status", new { idCotizacion });

        }
        public ActionResult Status(int idCotizacion)
        {
            Context context = new Context();
            ViewBag.Cotizacion = context.Cotizaciones.Find(idCotizacion);
            return View();

        }

       
        public JsonResult RefreshStatus(int idCotizacion)
        {
            Context context = new Context();
            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);

            //   //var data = { "programs": [{ "name": "zonealarm", "price": "500" }, { "name": "kaspersky", "price": "200" }] };

            return Json(new { 
                EstadoSolicitud= cotizacion.Solicitud.EstadoSolicitud.ToString(), 
                Comentario = cotizacion.Solicitud.ComentarioEvaluacion, 
                Documentos = cotizacion.Solicitud.DocumentosSolicitados ,  
                Motivos= cotizacion.Solicitud.MotivosRechazo}, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AdjuntarDocumentos(int? id)
        {

            if (id == null)
            {

                if (Request.Files.Count > 0)
                {
                    GuardarImagen();
                }

                return Json("", JsonRequestBehavior.AllowGet);

            }


            Context context = new Context();
            ViewBag.Cotizacion = context.Cotizaciones.Find(id);

            return View();
        
        }


        [HttpPost]
        public ActionResult ImageUpload()
        {

            string ruta = "";


            Context context = new Context();

            if (Request.Files.Count > 0)
            {
                GuardarImagen();
            }

            return Json("", JsonRequestBehavior.AllowGet);

        }

        private void GuardarImagen()
        {
            Context context = new Context();

            var file = Request.Files[0];
            var ruta = "";
            if (file != null && file.ContentLength > 0)
            {

                var fileName = Path.GetFileName(file.FileName);
                var path = "";
                //if (gUser.Tipo == GUser.GEnums.TipoUsuario.Alumno)
                //{
                //    string urlImgClientes = ConfigurationManager.AppSettings["UrlImgClientes"];
                //    Cliente cliente = context.Clientes.Find(gUser.IdCliente);
                //    cliente.Foto = cliente.Id.ToString() + "_" + fileName;
                //    path = Path.Combine(Server.MapPath(urlImgClientes), cliente.Foto);
                //    file.SaveAs(path);
                //    string PathIMG = ConfigurationManager.AppSettings["PathIMG"];
                //    cliente.Imagen = System.IO.File.ReadAllBytes(PathIMG + "clientes\\" + cliente.Foto);
                //    gUser.Foto = urlImgClientes + "/" + cliente.Foto;

                //}
                //else
                //{
                //    string UrlImgUsuarios = ConfigurationManager.AppSettings["UrlImgUsuarios"];
                //    Usuario usuario = context.Usuarios.Find(gUser.IdUsuario);
                //    usuario.Foto = usuario.Id.ToString() + "_" + fileName;
                //    path = Path.Combine(Server.MapPath(UrlImgUsuarios), usuario.Foto);
                //    file.SaveAs(path);


                }
                context.SaveChanges();

            }

        



        #region en desuso 
        public JsonResult SaveContratante(int idCotizacion, string rut, string apPaterno,string apMaterno, string nombre, DateTime fechaNacimiento, Enums.Nacionalidad nacionalidad, Enums.EstadoCivil estadoCivil, string direccion, string comuna, string email, int telefono, string actividad, Enums.TipoTrabajador tipoTrabajador, int renta, DateTime fechaInicioActividad, string rutEmpleador, string nombreEmpleador, Enums.TipoContratante tipoContratante)
        {
            Context context = new Context();

            string[] aux = rut.Split('-');
            Enums.TipoPersona tipoPersona = int.Parse(aux[0]) < 50000000 ? Enums.TipoPersona.Natural : Enums.TipoPersona.Juridica;

            Cotizacion cotizacion = context.Cotizaciones.Find(idCotizacion);

            Contratante contratante = new Contratante
            {
                Rut = rut,
                ApPaterno = apPaterno,
                ApMaterno = apMaterno,
                Nombre = nombre,
                FechaNacimiento = fechaNacimiento,
                Nacionalidad =  nacionalidad,
                EstadoCivil = estadoCivil,
                Direccion = direccion,
                Comuna = context.Comunas.FirstOrDefault(c=> c.Nombre.Equals(comuna)),
                Email = email,
                Renta = renta,
                Telefono = telefono,
                Actividad = actividad,
                TipoTrabajador =  tipoTrabajador,
                FechaInicioActividad = fechaInicioActividad,
                RutEmpleador = rutEmpleador,
                NombreEmpleador = nombreEmpleador,
                TipoContratante = tipoContratante,
                TipoPersona = tipoPersona,

            };

            if (cotizacion.Solicitud== null)
                cotizacion.Solicitud = new Solicitud();

            if (cotizacion.Solicitud.Contratantes == null)
                cotizacion.Solicitud.Contratantes = new List<Contratante>();

            cotizacion.Solicitud.Contratantes.Add(contratante);


            return Json(new { code= 200, nombre= contratante.Nombre + " " + contratante.ApPaterno +" "+ contratante.ApMaterno, rut= int.Parse(aux[0]).ToString("#,#") + "-" + aux[1] });
        }
        
        #endregion

        public JsonResult GetComunas()
        {
            Context context = new Context();
            var comunas = context.Comunas.Select(c => c.Nombre).ToArray();

            return Json(comunas);
        }
    }
}