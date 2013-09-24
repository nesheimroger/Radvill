using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radvill.Configuration
{
    public class Database
    {
        public static string ConnectionStringName
        {
            get { return "RadvillContext"; }
        }

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString; }
        }
    }
}
