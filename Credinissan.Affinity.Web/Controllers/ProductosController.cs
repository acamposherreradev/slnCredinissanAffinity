using Credinissan.Affinity.Web.Common;
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
    public class ProductosController : Controller
    {

        const string _msgAccesoDenegado = "No tienes acceso a este módulo";



        public ActionResult Index(string msgExito, string msgError, string msgInfo)
        {
            ViewBag.Exito = msgExito;
            ViewBag.Error = msgError;
            ViewBag.Info = msgInfo;

            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion


            return View(GetProductos());
        }

        public ActionResult Edit(int id)
        {
            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion

            return View(GetProductos().FirstOrDefault(p=> p.Id.Equals(id)));
        }

        [HttpPost]
        public ActionResult Edit(Producto producto)
        {

            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion

            string path = Server.MapPath("/") + "Parameters/Productos.json";

            var productos = GetProductos();

            Producto  producToChange = productos.FirstOrDefault(p => p.Id.Equals(producto.Id));

            producToChange.Id = producto.Id;
            producToChange.Name = producto.Name;
            producToChange.AllVersion = producto.AllVersion;
            producToChange.AllUsers = producto.AllUsers;
            producToChange.Active = producto.Active;

            string jsonString = JsonConvert.SerializeObject(productos);

           System.IO.File.WriteAllText(path, jsonString);
        
           return RedirectToAction("Index", new { msgExito = "Save, OK!! "  });
        }

        public ActionResult Tasas(int id)
        {
            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion

            Producto producto = GetProductos().FirstOrDefault(p => p.Id.Equals(id));
            ViewBag.Producto =producto;

            return View(producto.Tasas);
        }

        //EditTasa
        public ActionResult EditTasa(int idTasa, int idProducto)
        {
            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion


            Producto producto = GetProductos().FirstOrDefault(p => p.Id.Equals(idProducto));
            
            ViewBag.IdProducto = idProducto;

            return View(producto.Tasas.FirstOrDefault(t=> t.Id.Equals(idTasa)));
        }

        [HttpPost]
        public ActionResult EditTasa(Tasa tasa, int idProducto)
        {

            #region Validacion de Seguridad de acceso
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("inteligencia"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            #endregion


            string path = Server.MapPath("/") + "Parameters/Productos.json";

            var productos = GetProductos();

            Producto producto = productos.FirstOrDefault(p => p.Id.Equals(idProducto));
            Tasa tasaForUpdate = producto.Tasas.FirstOrDefault(t => t.Id.Equals(tasa.Id));

            tasaForUpdate.PlazoMax = tasa.PlazoMax;
            tasaForUpdate.PlazoMin = tasa.PlazoMin;
            tasaForUpdate.UFMax = tasa.UFMax;
            tasaForUpdate.UFMin = tasa.UFMin;
            tasaForUpdate.TasaPagare = tasa.TasaPagare;
            tasaForUpdate.TasaPiso = tasa.TasaPiso;

            string jsonString = JsonConvert.SerializeObject(productos);

            System.IO.File.WriteAllText(path, jsonString);

            return RedirectToAction("Index", new { msgExito = "Save, OK!! " });

        }
        private List<Producto> GetProductos()
        {

            string path = Server.MapPath("/") + "Parameters/Productos.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<Producto>>(json);

            }

        }
    }
}