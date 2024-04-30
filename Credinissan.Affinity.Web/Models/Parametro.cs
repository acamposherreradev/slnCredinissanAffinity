using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Credinissan.Affinity.Web.Models
{
    public class Parametro
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
        public List<DailyValue> DailyValue { get; set; }

    }

    public class DailyValue
    {
        public DateTime Day { get; set; }
        public double Value { get; set; }
    }


}