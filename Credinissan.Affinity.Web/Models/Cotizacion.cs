using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Credinissan.Affinity.Web.Models ROD acampos2
/// </summary>
namespace Credinissan.Affinity.Web.Models
{

    public class ModifiedFeeValues
    {
        public int FeeNumber { get; set; }
        public double FeeAmount { get; set; }
    }

    public class ItemTablaDesarrollo
    {

        public int Mes { get; set; }
        public DateTime Vencimiento { get; set; }
        public double Saldo { get; set; }
        public double Interes { get; set; }
        public double Capital { get; set; }
        public double Cuota { get; set; }
        public bool CuotaFija { get; set; }


    }

    public class Seguro
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Description { get; set; }
        public string TipoFormula { get; set; }
        public double Valor { get; set; }
        public double MontoBruto { get; set; }
    }

    public class Accesorio
    {
        public string Codigo { get; set; }
        public double Valor { get; set; }

    }
    public class Cotizacion {

        public enum TipoCredito
        {
            Inteligente = 1,
            Convencional = 2,

        }

        public int IdVersion { get; set; }
        public DateTime FechaPagare { get; set; }
        public int ValorVehiculo { get; set; }
        public int Cantidad { get; set; }
        public int Pie { get; set; }
        public int Retoma { get; set; }
        public int SaldoPrecio { get; set; }
        public double Tasa { get; set; }
        public int Plazo { get; set; }
        public DateTime FechaPrimerVencimiento { get; set; }
        public TipoCredito Tipo { get; set; }
        public GastoOperacional GastoOperacional { get; set; }
        public int DiasDesfase { get; set; }
        public double MontoDesfase { get; set; }
        public double ITE { get; set; }
        public double MAF { get; set; }
        public double CAF { get; set; }
        public double VFMG { get; set; }
        public double ComisionDealer { get; set; }
        public List<ItemTablaDesarrollo> TablaDesarrollo { get; set; }
        public List<Seguro> Seguros { get; set; }
        public List<Accesorio> Accesorios { get; set; }

        public Cotizacion( DateTime fechaPagare, int valorVehiculo,int cantidad, int pie, int retoma,  int plazo, DateTime fechaPrimerVencimiento, TipoCredito tipoCredito, int idVersion, int vfmg=0, List<ModifiedFeeValues> pagoConvenido=null)
        {
            IdVersion = idVersion;
            FechaPagare = fechaPagare;
            ValorVehiculo = valorVehiculo;
            Cantidad = cantidad;
            Pie = pie;
            Retoma = retoma;
            SaldoPrecio = (valorVehiculo * cantidad) - pie - retoma;
            Plazo = plazo;
            FechaPrimerVencimiento = fechaPrimerVencimiento;
            DiasDesfase = (int)(fechaPrimerVencimiento - fechaPagare).TotalDays - 30;
            TablaDesarrollo = new List<ItemTablaDesarrollo>();
            Tipo = tipoCredito;
            Seguros = new List<Seguro>();
            Accesorios = new List<Accesorio>();

            if (Tipo == Cotizacion.TipoCredito.Inteligente)
            {
                Plazo++;
            }

            for (int i = 1; i <= Plazo; i++)
            { 
                TablaDesarrollo.Add(new ItemTablaDesarrollo { Mes=i, Vencimiento = fechaPrimerVencimiento.AddMonths(i-1), CuotaFija=false });
            }

            if (Tipo == Cotizacion.TipoCredito.Inteligente)
            {
                VFMG = (double)valorVehiculo * vfmg / 100;
                FijarUnaCuota(Plazo, VFMG);
            }

            if (pagoConvenido !=null && pagoConvenido.Any()) // Cuota variable
            {
                foreach (var item in pagoConvenido)
                {
                    FijarUnaCuota(item.FeeNumber, item.FeeAmount);
                }
                
            }


        }

        public void FijarUnaCuota(int mes, double cuota )
        {
            var item = this.TablaDesarrollo.FirstOrDefault(i => i.Mes.Equals(mes));
            item.Cuota = cuota;
            item.CuotaFija = true;

        
        }

        public double GenerarTabla(double cuota, double saldoFinal )
        {

            if (Math.Abs(saldoFinal) < 0.001)
                return 0;
            else
            {
                saldoFinal = CAF;
                foreach (var item in TablaDesarrollo)
                {
                    if (!item.CuotaFija)
                        item.Cuota = cuota;

                    item.Interes = saldoFinal * Tasa;
                    item.Capital = item.Cuota - item.Interes;
                    saldoFinal -= item.Capital;

                }

                return GenerarTabla(cuota + saldoFinal / Plazo/2, saldoFinal);

            }


        }

       

    }

    
    
}