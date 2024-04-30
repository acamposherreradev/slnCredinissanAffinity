using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Credinissan.Affinity.Web.Data
{
    public class MySqlEFConfiguration : DbConfiguration
    {

        public MySqlEFConfiguration()
        {
            SetHistoryContext("MySql.Data.MySqlClient",
                (conn, schema) => new History.MySqlHistoryContext(conn, schema));
        }
    }
}