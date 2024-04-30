using Credinissan.Affinity.Web.Common;
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
using Modelo = Credinissan.Affinity.Web.Data.DataObject.Modelo;

namespace Credinissan.Affinity.Web.Controllers
{
    public class SoporteController : Controller
    {
        // GET: Soporte
        const string _msgAccesoDenegado = "No tienes acceso a este módulo";
        public ActionResult Index(string msgExito)
        {
            ViewBag.Exito = msgExito;

            #region Validacion de Seguridad de acceso
            //Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            //if (!security.IsInRol("soporte"))
            //{
            //    return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            //}
            #endregion

            Context context = new Context();

            try {

                Usuario usuario = context.Usuarios.FirstOrDefault();

                if (usuario != null)
                    ViewBag.Exito = "Usuario: " + usuario.Nombre;

                return View();

            }

            catch (Exception ex) {

                ViewBag.Error = ex.Message;
                return View();
            }

           

        }

        public ActionResult RespaldarModelosVersiones()
        {

            RespaldarModelos();
            RespaldarVersiones();

            return RedirectToAction("Index", new { msgExito = "Modelos y Vewrsiones respaldados" });

        }

            public ActionResult CrearBD()
        {

            #region Validacion de Seguridad de acceso
            //Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            //if (!security.IsInRol("soporte"))
            //{
            //    return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            //}
            #endregion


            Context context = new Context();

            if(context.Database.Exists())
                context.Database.Delete();
            context.Database.Create();

            Distribuidor portillo = context.Distribuidores.Add(new Distribuidor { Nombre="Portillo", RazonSocial="Portillo S.A.", Rut="99999999-9" });
            Sucursal portilloEstoril = context.Sucursales.Add(new Sucursal { Nombre = "Estoril", Distribuidor = portillo });
                  
            Rol rolCotizador =  context.Roles.Add(new Rol { Descripcion="cotizador", DefaultControlller="Cotizacion", DefaultView="index"  });
            Rol rolSoporte = context.Roles.Add(new Rol { Descripcion = "soporte", DefaultControlller = "Soporte", DefaultView = "index" });
            Rol rolInteligencia = context.Roles.Add(new Rol { Descripcion = "inteligencia", DefaultControlller = "Productos", DefaultView = "index" });
            Rol rolEvaluador = context.Roles.Add(new Rol { Descripcion = "evaluacion", DefaultControlller = "Cotizacion", DefaultView = "index" });
            Rol rolValidacion = context.Roles.Add(new Rol { Descripcion = "validacion", DefaultControlller = "Cotizacion", DefaultView = "index" });
            Rol rolCompliance = context.Roles.Add(new Rol { Descripcion = "compliance", DefaultControlller = "Cotizacion", DefaultView = "index" });
            Rol rolFirmaDocumentos = context.Roles.Add(new Rol { Descripcion = "firmaDocumentos", DefaultControlller = "Cotizacion", DefaultView = "index" });
            Rol rolValidacionOps = context.Roles.Add(new Rol { Descripcion = "validacionOps", DefaultControlller = "Cotizacion", DefaultView = "index" });
            Rol rolCurse = context.Roles.Add(new Rol { Descripcion = "curse", DefaultControlller = "Cotizacion", DefaultView = "index" });

            Usuario rcerdaHome = context.Usuarios.Add(new Usuario { Sucursal= portilloEstoril,  Nombre = "Rod (casa)", Username = "LAPTOP-BFA0E8OE\\rodri", Rut = "12674021-2", AprobarTerminosCondiciones = true, CantidadIntentos = 0, Clave = "", Email = "rodrigo_cerda@hotmail.com", Estado = Enums.EstadoUsuario.Acivo });
            Usuario rcerda =  context.Usuarios.Add(new Usuario { Sucursal = portilloEstoril,  Nombre = "Rodrigo Cerda", Username= "EC2AMAZ-7MTC17K\\rcerda", Rut="12674021-2", AprobarTerminosCondiciones = true, CantidadIntentos =0, Clave="", Email= "rodrigo.cerda@credinissan.cl", Estado =  Enums.EstadoUsuario.Acivo});
            Usuario avasquez = context.Usuarios.Add(new Usuario { Sucursal = portilloEstoril,  Nombre = "Alexis Vasquez", Username = "EC2AMAZ-7MTC17K\\avasquez", Rut = "12674021-2", AprobarTerminosCondiciones = true, CantidadIntentos = 0, Clave = "", Email = "alexis.vasquez@credinissan.cl", Estado = Enums.EstadoUsuario.Acivo });



            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerdaHome, Rol = rolCotizador });
            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerdaHome, Rol = rolInteligencia });
            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerdaHome, Rol = rolSoporte });

            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerda, Rol= rolCotizador  });
            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerda, Rol = rolInteligencia });
            context.UsuariosRoles.Add(new UsuarioRol { Usuario = rcerda, Rol = rolSoporte });

            context.UsuariosRoles.Add(new UsuarioRol { Usuario = avasquez, Rol = rolCotizador });
            context.UsuariosRoles.Add(new UsuarioRol { Usuario = avasquez, Rol = rolInteligencia });


            context.SaveChanges();

            //restauramos modelos y versiones
            Marca nissan = context.Marcas.Add(new Marca { Id = 102, Nombre = "Nissan" });
            List<Modelo> _modelos = GetModelos();
            List<Data.DataObject.Version> _versiones = GetVersones();
            foreach (Modelo _modelo in _modelos)
            {
              Modelo modelo =  context.Modelos.Add(new Modelo {  Marca = nissan, Nombre = _modelo.Nombre   });
                foreach (Data.DataObject.Version _version in _versiones.Where(v=> v.Modelo.Id == _modelo.Id))
                {
                    context.Versiones.Add(new Data.DataObject.Version { Modelo = modelo, Nombre = _version.Nombre });
                }
            }
            context.SaveChanges();


            return RedirectToAction("Index", new {msgExito= "BD Creada OK" });
        }


        private  void RespaldarModelos()
        {

            Context context = new Context();
           

            string path = Server.MapPath("/") + "Parameters/Modelos.json";
            List<Modelo> modelos = context.Modelos.ToList();
            string jsonString = JsonConvert.SerializeObject(modelos);
            System.IO.File.WriteAllText(path, jsonString);

           

        }

        private void RespaldarVersiones()
        {

            Context context = new Context();
            

            string path = Server.MapPath("/") + "Parameters/Versiones.json";
            List<Data.DataObject.Version> versiones = context.Versiones.ToList();
            string jsonString = JsonConvert.SerializeObject(versiones);
            System.IO.File.WriteAllText(path, jsonString);


        }

        private List<Modelo> GetModelos()
        {

            string path = Server.MapPath("/") + "Parameters/Modelos.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<Modelo>>(json);

            }

        }

        private List<Data.DataObject.Version> GetVersones()
        {

            string path = Server.MapPath("/") + "Parameters/Versiones.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<Data.DataObject.Version>>(json);

            }

        }

    }
}