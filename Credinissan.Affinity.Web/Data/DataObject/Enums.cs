using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Data.DataObject
{
    public class Enums
    {

        public enum TipoCredito
        {
            Inteligente = 1,
            Convencional = 2,

        }

        public enum TipoPersona
        {
            Natural = 1,
            Juridica = 2,

        }
        public enum TipoTrabajador
        {
            Dependiente = 1,
            Independiente = 2,

        }

    public enum TipoContratante
        {
            Titular = 1,
            Aval = 2,
            AvalCompraPara =3,

        }

        public enum Nacionalidad
        {
            Chilena = 56,
            Extranjera = 0,

        }

        public enum EstadoCivil
        {
            Soltero = 0,
            Casado = 1,
            Divorciado= 2,
            Viudo =3,

        }
        public enum Sexo
        {
            Masculino = 0,
            Femenino = 1,
            NoInformado = 3,
        }

        public enum DiaDeLaSemana
        {
            Lunes = 1,
            Martes = 2,
            Miercoles = 3,
            Jueves = 4,
            Viernes = 5,
            Sabado = 6,
            Domingo = 7,
        }

        public enum EstadoUsuario
        {
            Inactivo = 0,
            Acivo = 1,
            Bloqueado = 3,
        }

        public enum EstadoCotizacion
        {
            Simulación = 0,
            Cotización = 1,
            Evaluación = 2,
            Aprobada = 3,
            Rechazada = 4,
        }

        public enum EstadoSolicitud
        {
            SinEnviarEvaluar = 0,
            EnEvaluacion = 1,
            AprobadaConCondiciones = 3,
            Rechazada = 4,
        }

        public enum EstadoValidacion
        {
            SinEnviarValidar = 0,
            EvValidacion = 1,
            Aprobada = 3,
            Rechazada = 4,
        }

    }
}