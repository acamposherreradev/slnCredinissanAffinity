using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Common
{
    public class Utilidades
    {

        public static int CalculateAge(DateTime fromDate, DateTime toDate)
        {
            int age = toDate.Year - fromDate.Year;

            // Si aún no ha llegado al mes de nacimiento, o está en el mes pero no ha llegado al día, restamos un año.
            if (toDate.Month < fromDate.Month || (toDate.Month == fromDate.Month && toDate.Day < fromDate.Day))
            {
                age--;
            }

            return age;
        }

    }
}