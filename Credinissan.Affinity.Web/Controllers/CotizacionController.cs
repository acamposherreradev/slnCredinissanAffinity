using Credinissan.Affinity.Web.Common;
using Credinissan.Affinity.Web.Data.Models;
using Credinissan.Affinity.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Credinissan.Affinity.Web.Controllers
{
    public class CotizacionController : Controller
    {
        // GET: Cotizacion

        const string _msgAccesoDenegado = "Acceso denegado, contacta a tu supervisor para solicitar acceso";
        public ActionResult Index(string msgExito, string msgError, string msgInfo)
        {
            ViewBag.Exito = msgExito;
            ViewBag.Error = msgError;
            ViewBag.Info = msgInfo;

            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("cotizador"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }

            Context context = new Context();

            ViewBag.Modelos = context.Modelos.ToList();

            //var result = Evaluar("(3 * 5 * ( 5 / 6 ) * 4 -2.9 )");
            //ViewBag.Result = result;

            ViewBag.Productos = GetProductos();

            return View();
        }
        public ActionResult Calculate(int valorVehiculo, int pie, int retoma, int plazo, DateTime vencimiento, int idProducto, int idVersion)
        {
            Security security = new Security((List<PermisosUsuario>)Session["PermisosUsuario"]);
            if (!security.IsInRol("cotizador"))
            {
                return RedirectToAction("index", "home", new { msgError = _msgAccesoDenegado });

            }
            //VALORES POR CAPTURAR
            DateTime fechaPagare = DateTime.Today;
            int cantidadVehiculos = 1;
            bool incluyeSeguroDesgravamen = true;
            bool incluyePatente = true;
            //fin

            int vfmg = 0;
            double totalSegurosCarrito = 0;

            Producto producto = GetProductos().FirstOrDefault(p => p.Id.Equals(idProducto));

            if (producto.TipoCredito== Cotizacion.TipoCredito.Inteligente)
                vfmg = producto.Plazos.FirstOrDefault(p => p.Id.Equals(plazo)).Vfmg;

            Cotizacion cotizacion = new Cotizacion(fechaPagare, valorVehiculo, cantidadVehiculos, pie, retoma, plazo, vencimiento, producto.TipoCredito, idVersion, vfmg );


            double valorUF = GetDailyParameterByName("uf", fechaPagare);
            if (valorUF == 0)
            {
                return RedirectToAction("Index", new { msgError = "No se encontró valor de la UF para el día " + fechaPagare.ToShortDateString() });

            }

            totalSegurosCarrito += cotizacion.Accesorios.Sum(a => a.Valor);
            totalSegurosCarrito += cotizacion.Seguros.Sum(a => a.Valor);


            if (incluyeSeguroDesgravamen)
            {
                // intDesgravamen = GetDesgravamen(cotizacion.SaldoPrecio + montoGastoOperacional + totalSegurosCarrito , edad)
                cotizacion.Seguros.Add(new Seguro { Codigo="desgravamen-menor", Valor= 0 });
            }

            if (incluyePatente)
            {
                cotizacion.Accesorios.Add(new Accesorio { Codigo = "patente", Valor = 0 });

            }

            
            

            cotizacion.GastoOperacional  = GetGastoByName("nuevos");
            double montoGastoOperacional = cotizacion.GastoOperacional.Items.Sum(g => g.Valor);
            double montoEnUf = (cotizacion.SaldoPrecio + montoGastoOperacional + totalSegurosCarrito) / valorUF;
            Tasa tasa = producto.Tasas.FirstOrDefault(r => r.PlazoMin <= cotizacion.Plazo && cotizacion.Plazo <= r.PlazoMax && r.UFMin < montoEnUf && montoEnUf <= r.UFMax);
            //Tasa TMC = producto.Tasas.FirstOrDefault(r => r.PlazoMin <= cotizacion.Plazo && cotizacion.Plazo <= r.PlazoMax && r.UFMin < montoEnUf && montoEnUf <= r.UFMax);
            if (tasa == null)
            {
                return RedirectToAction("Index", new { msgError = "No se encontró tasa para los criterios ingresados - Monto: UF " + montoEnUf });

            }

            cotizacion.Tasa = tasa.TasaPagare;

            cotizacion.MontoDesfase  = GetDesfase(cotizacion.SaldoPrecio, montoGastoOperacional, 0, totalSegurosCarrito, cotizacion.DiasDesfase, cotizacion.Tasa);
            cotizacion.ITE = GetImpuesto(cotizacion.Plazo, cotizacion.SaldoPrecio, montoGastoOperacional, cotizacion.ITE, totalSegurosCarrito);
            cotizacion.MontoDesfase = GetDesfase(cotizacion.SaldoPrecio, montoGastoOperacional, cotizacion.ITE, totalSegurosCarrito, cotizacion.DiasDesfase, cotizacion.Tasa);
            cotizacion.MAF = cotizacion.SaldoPrecio + montoGastoOperacional + totalSegurosCarrito + cotizacion.ITE;
            cotizacion.CAF = cotizacion.SaldoPrecio + montoGastoOperacional + totalSegurosCarrito + cotizacion.ITE + cotizacion.MontoDesfase;

            double cuotaInicial = cotizacion.CAF * cotizacion.Tasa / (1 - Math.Pow(1 + cotizacion.Tasa, -cotizacion.Plazo));
            cotizacion.GenerarTabla(cuotaInicial, cotizacion.CAF);

            Session["Cotizacion"] = cotizacion;

            return View(cotizacion);

            //return RedirectToAction("ResumenCotizacion");

        }

        public ActionResult ResumenCotizacion()
        {
            Cotizacion cotizacion = (Cotizacion)Session["Cotizacion"];

            return View(cotizacion);
        
        }

        public ActionResult Solicitud(int? idCotizacion)
        {

            if (Session["Cotizacion"] == null)
            {
               return RedirectToAction("Index");
            }
            Cotizacion cotizacion = (Cotizacion)Session["Cotizacion"];
            Data.DataObject.Cotizacion dbCotizacion;;

            Context context = new Context();



            if (idCotizacion is null)
            {
                dbCotizacion = new Data.DataObject.Cotizacion();
                context.Cotizaciones.Add(dbCotizacion);
            }

            else
            {
                dbCotizacion = context.Cotizaciones.Find(idCotizacion);

            }


            dbCotizacion.FechaPagare = cotizacion.FechaPagare;
            dbCotizacion.ValorVehiculo = cotizacion.ValorVehiculo;
            dbCotizacion.Pie = cotizacion.Pie;
            dbCotizacion.Retoma = cotizacion.Retoma;
            dbCotizacion.Cantidad = cotizacion.Cantidad;
            dbCotizacion.Plazo = cotizacion.Plazo;
            dbCotizacion.Tasa = cotizacion.Tasa;
            dbCotizacion.SaldoPrecio = cotizacion.SaldoPrecio;
            dbCotizacion.Tipo = (Data.DataObject.Enums.TipoCredito)cotizacion.Tipo;
            dbCotizacion.FechaPrimerVencimiento = cotizacion.FechaPrimerVencimiento;
            dbCotizacion.ITE = cotizacion.ITE;
            dbCotizacion.DiasDesfase = cotizacion.DiasDesfase;
            dbCotizacion.MontoDesfase = cotizacion.MontoDesfase;
            dbCotizacion.MontoGastoOperacional = cotizacion.GastoOperacional.Items.Sum(g => g.Valor);
            dbCotizacion.MontoCuota = cotizacion.TablaDesarrollo.First().Cuota;
            dbCotizacion.VFMG = cotizacion.VFMG;
            dbCotizacion.MAF = cotizacion.MAF;
            dbCotizacion.CAF = cotizacion.CAF;
            dbCotizacion.ComisionDealer = cotizacion.ComisionDealer;
            dbCotizacion.GastoOperacional = JsonConvert.SerializeObject(cotizacion.GastoOperacional);
            dbCotizacion.Accesorios = JsonConvert.SerializeObject(cotizacion.Accesorios);
            dbCotizacion.Seguros = JsonConvert.SerializeObject(cotizacion.Seguros);
            dbCotizacion.TablaDesarrollo = JsonConvert.SerializeObject(cotizacion.TablaDesarrollo);
            dbCotizacion.Version = context.Versiones.Find(cotizacion.IdVersion);
            dbCotizacion.FechaCreacion = DateTime.Now;
            dbCotizacion.FechaUltModificacion = DateTime.Now;
            dbCotizacion.UsuarioCreacion = context.Usuarios.FirstOrDefault(usu => usu.Username.Equals(User.Identity.Name));
            dbCotizacion.UsuarioUltModificacion = context.Usuarios.FirstOrDefault(usu => usu.Username.Equals(User.Identity.Name));

            context.SaveChanges();


            return RedirectToAction("Index","Solicitud", new { idCotizacion = dbCotizacion.Id});

        }
        public JsonResult SendCuotation()
        {

            Cotizacion cotizacion = (Cotizacion)Session["Cotizacion"];



            Session["Cotizacion"] = cotizacion;

            var envio = EnviarEmail(cotizacion);

            return Json( envio);
            
        }


        private JsonResult EnviarEmail(Cotizacion cotizacion)
        {
            Context context = new Context();



            string nombre = "Rodrigo Cerda";

            if (nombre.IndexOf(' ') > -1)
            {
                nombre = nombre.Substring(0, nombre.IndexOf(' '));
            }
            try
            {


                Notificacion notificacion = new Notificacion
                {
                    Asunto = "Cotizamos tu Nissan",
                    Body = RenderViewToString("ResumenCotizacion", cotizacion),//Path.Combine(Server.MapPath("~/Html/EnviarMensajeAlumno.html")),
                    From = "online@credinissan.cl", //ConfigurationManager.AppSettings["EmailContacto"],
                    To = "rodrigo_cerda@hotmail.com",
                    CCO = "rodrigo.cerda@credinissan.cl" , // ConfigurationManager.AppSettings["EmailContacto"],
                    Mask = "CrediNissan", /*ConfigurationManager.AppSettings["EmailMask"],*/
                };


                notificacion.SendMail();

            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = "Mensaje no pudo ser enviado "  });

            }

            return Json(new { code = 200, msg = "Envío OK"});
        }


        private string RenderViewToString(string nombreVista, object modelo)
        {
            ViewData.Model = modelo;

            using (var sw = new StringWriter())
            {
                var vista = ViewEngines.Engines.FindView(ControllerContext, nombreVista, null);
                var contexto = new ViewContext(ControllerContext, vista.View, ViewData, TempData, sw);
                vista.View.Render(contexto, sw);

                return sw.ToString();
            }
        }



        static Double Evaluar(String expression)
        {
            //Creo un DataTable
            System.Data.DataTable table = new System.Data.DataTable();
            //Realizo el cálculo..
            object result = table.Compute(expression, string.Empty);
            //Lo devuelvo convertido a Double
            return Convert.ToDouble(result);
        }


        #region Getters

        private double GetDesfase(double saldoPrecio, double gastoOperacional, double impuesto, double otros, int diasDesfase, double tasaMensual)
        {
            double monto = saldoPrecio + gastoOperacional + impuesto + otros;
            return (monto * tasaMensual * diasDesfase/30);
        }

        private double GetImpuesto(int plazo, double saldoPrecio, double gastoOperacional, double impuesto, double otros)
        {
            double monto = saldoPrecio + gastoOperacional + impuesto + otros;
            double tasaITE = plazo < 12 ? GetParameterByName("ite-menor-12-meses") : GetParameterByName("ite-mayor-12-meses");

            if (Math.Abs((monto * tasaITE) - impuesto) <= 0.01)
                return (monto * tasaITE);
            else
            {
                return GetImpuesto(plazo, saldoPrecio, gastoOperacional, (monto * tasaITE), otros);
            }
        }


       

        public JsonResult GetPlazos(int idProducto)
        {
            Producto producto = GetProductos().FirstOrDefault(p => p.Id.Equals(idProducto));
            List<Plazo> plazos;

            if (producto.TipoCredito == Cotizacion.TipoCredito.Inteligente)

                plazos = producto.Plazos;

            else
            {
                plazos = new List<Plazo>();
                int[] classicPlazos = new int[] { 6, 12, 18, 24, 36, 42, 48, 60, 72, 84, 96, 108, 120 };

                foreach (int plazo in classicPlazos)
                {
                    if (plazo >= producto.Plazos.Min(p => p.Id) && plazo <= producto.Plazos.Max(p => p.Id))
                        plazos.Add(new Plazo { Id = plazo, Vfmg = 0 });
                }


            }

            Plazo[] data = plazos.ToArray();

            return Json(new { data }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetVersiones(int idModelo)
        {
            Context context = new Context();

            Data.DataObject.Version[] data = context.Versiones.Where(v=> v.Modelo.Id.Equals(idModelo)).ToList().ToArray();

            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public GastoOperacional GetGastoByName(string name) {


           return GetGastos().FirstOrDefault(g => g.Name.Equals(name));


        }


        public double GetParameterByName(string name)
        {
            return GetParametros().FirstOrDefault(p=> p.Name.Equals(name)).Value;
        }

        public double GetDailyParameterByName(string name, DateTime day)
        {

            try {
                Parametro parametro = GetParametros().FirstOrDefault(p => p.Name.Equals(name));
                return parametro.DailyValue.FirstOrDefault(p => p.Day.Equals(day)).Value;
            }

            catch {

                return 0;

            }

            

        }

        #endregion

        #region Generic Getters
        private List<Parametro> GetParametros()
        {
            string path = Server.MapPath("/") + "Parameters/Parametros.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<Parametro>>(json);

            }

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

        private List<GastoOperacional> GetGastos()
        {
            string path = Server.MapPath("/") + "Parameters/Gastos.json";

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                return JsonConvert.DeserializeObject<List<GastoOperacional>>(json);

            }

        }



        #endregion

    }
}
