using System;
using System.Configuration;
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
            Type dbClass = dalAssembly.GetType(assemblyName + ".Database");
            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }

        public static IPlayerDao CreatePlayerDao(IDatabase database)
        {
            return CreateDao<IPlayerDao>(database, "PlayerDao");
        }

        public static IRatingDao CreateRatingDao(IDatabase database)
        {
            return CreateDao<IRatingDao>(database, "RatingDao");
        }

        private static T CreateDao<T>(IDatabase database, string typeName)
            where T : class // T must be a reference type
        {
            Type daoType = dalAssembly.GetType(assemblyName + "." + typeName);
            return Activator.CreateInstance(daoType, database) as T;
        }
    }
}
