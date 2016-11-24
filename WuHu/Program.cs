using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Dal.Common;
using WuHu.Dal.SqlServer;
using WuHu.Test;

namespace WuHu.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            IDatabase database = DalFactory.CreateDatabase();
            CommonData.InsertTestData(database);
        }
    }
}
