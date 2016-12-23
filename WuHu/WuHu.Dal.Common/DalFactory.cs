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
            var connectionString = ConfigurationManager.
                ConnectionStrings["DefaultConnectionString"].ConnectionString;
            return CreateDatabase(connectionString);
        }

        public static IDatabase CreateDatabase(string connectionString)
        {
            var dbClass = dalAssembly.GetType(assemblyName + ".Database");
            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }

        public static IPlayerDao CreatePlayerDao(IDatabase database)
        {
            return CreateDao<IPlayerDao>(database, "PlayerDao");
        }
        public static ITournamentDao CreateTournamentDao(IDatabase database)
        {
            return CreateDao<ITournamentDao>(database, "TournamentDao");
        }

        public static IRatingDao CreateRatingDao(IDatabase database)
        {
            return CreateDao<IRatingDao>(database, "RatingDao");
        }

        public static IMatchDao CreateMatchDao(IDatabase database)
        {
            return CreateDao<IMatchDao>(database, "MatchDao");
        }

        public static IScoreParameterDao CreateScoreParameterDao(IDatabase database)
        {
            return CreateDao<IScoreParameterDao>(database, "ScoreParameterDao");
        }

        private static T CreateDao<T>(IDatabase database, string typeName)
            where T : class // T must be a reference type
        {
            var daoType = dalAssembly.GetType(assemblyName + "." + typeName);
            return Activator.CreateInstance(daoType, database) as T;
        }
    }
}
