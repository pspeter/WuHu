using System;
using System.Reflection;

namespace WuHu.Dal.Common
{
    public static class DalFactory
    {
        private static string assemblyName;
        private static Assembly dalAssembly;

        static DalFactory()
        {
            assemblyName = ConfigurationManager.AppSettings["DalAssembly"];
            dalAssembly = Assembly.Load(assemblyName);
        }

        public static IDatabase CreateDatabase()
        {
            string connectionString = ConfigurationManager.
                ConnectionStrings["DefaultConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        public static IDatabase CreateDatabase(string connectionString)
        {
            Type dbClass = dalAssembly.GetType(assemblyName + ".Database"); // ausgefeilter möglich
            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }

        public static IZoneDao CreateZoneDao(IDatabase database)
        {
            return CreateDao<IZoneDao>(database, "ZoneDao");
        }


        public static ITariffDao CreateTariffDao(IDatabase database)
        {
            return CreateDao<ITariffDao>(database, "TariffDao");
        }

        public static IRateDao CreateRateDao(IDatabase database)
        {
            return CreateDao<IRateDao>(database, "RateDao");
        }

        private static T CreateDao<T>(IDatabase database, string typeName)
            where T : class // T must be a reference type
        {
            Type daoType = dalAssembly.GetType(assemblyName + "." + typeName);
            return Activator.CreateInstance(daoType, database) as T;
        }
    }
}
