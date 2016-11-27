using System.Configuration;
using WuHu.Dal.Common;
using WuHu.Test;

namespace WuHu.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            IDatabase database = DalFactory.CreateDatabase();
            TestHelper.InsertTestData(database);
        }
    }
}
