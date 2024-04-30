using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public DateTime FechaPagare { get; set; }
        public int ValorVehiculo { get; set; }
        public int Cantidad { get; set; }
        public int Pie { get; set; }
        public int Retoma { get; set; }
        public int SaldoPrecio { get; set; }
        public double Tasa { get; set; }
        public int Plazo { get; set; }
        public DateTime FechaPrimerVencimiento { get; set; }
        public Enums.TipoCredito Tipo { get; set; }
        public int DiasDesfase { get; set; }
        public double MontoDesfase { get; set; }
        public double MontoGastoOperacional { get; set; }
        public double ITE { get; set; }
        public double MAF { get; set; }
        public double CAF { get; set; }
        public double VFMG { get; set; }

        public double MontoCuota { get; set; }
        public double ComisionDealer { get; set; }
        public string GastoOperacional { get; set; }
        public string TablaDesarrollo { get; set; }
        public string Seguros { get; set; }
        public string Accesorios { get; set; }
        public virtual Version Version { get; set; }
        public virtual Usuario UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual Usuario UsuarioUltModificacion { get; set; }
        public DateTime FechaUltModificacion { get; set; }
        public virtual Solicitud Solicitud { get; set; }

    }
}