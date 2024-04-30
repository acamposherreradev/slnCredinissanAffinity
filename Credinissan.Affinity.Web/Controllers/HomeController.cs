using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Credinissan.Affinity.Web.Data.DataObject;
using Credinissan.Affinity.Web.Data.Models;
using Credinissan.Affinity.Web.Models;

namespace Credinissan.Affinity.Web.Controllers
{
    public class HomeController : Controller


    {
        //public ActionResult Index(string msgExito, string msgError, string msgInfo)
        //{

        //    return View();
        //}

        public ActionResult Index(string msgExito, string msgError, string msgInfo)
        {
            ViewBag.Exito = msgExito;
            ViewBag.Error = msgError;
            ViewBag.Info = msgInfo;

            Context context = new Context();


            Usuario usuario = context.Usuarios.FirstOrDefault(usu => usu.Username.Equals(User.Identity.Name));

            if (usuario == null)
            {
                usuario = context.Usuarios.Add(new Usuario { Username = User.Identity.Name, AprobarTerminosCondiciones = true, CantidadIntentos = 0, Clave = "", Nombre = "No enrolado" });

                context.SaveChanges();
            }

            List<PermisosUsuario> permisosUsuario = new List<PermisosUsuario>();

            List<Rol> roles = context.Roles.ToList();


            foreach (Rol rol in roles)
            {
                UsuarioRol usuRol = context.UsuariosRoles.FirstOrDefault(ur => ur.Rol.Id == rol.Id && ur.Usuario.Id == usuario.Id);
                permisosUsuario.Add(new PermisosUsuario { Usuario = usuario, Rol = rol, Asignado = (usuRol != null) });
            }

            Session["PermisosUsuario"] = permisosUsuario;


            return View();
        }

        public ActionResult Perfil()
        {
        
            ViewBag.PermisosUsuario = Session["PermisosUsuario"];
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}