using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using WJBCommon.Lib.Data;

namespace FoodDiaryApi.App.Database
{
    public sealed class ApiDatabase : IApiDatabase
    {
        private readonly ISessionFactory _sessionFactory;

        public ApiDatabase()
        {
            _sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard
                    .ConnectionString(c => c
                        .Host("localhost")
                        .Port(5432)
                        .Database("food_diary")
                        .Username("postgres")
                        .Password("Password")))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Program>())
                .BuildSessionFactory();
        }

        public ISessionFactory SessionFactory()
        {
            return _sessionFactory;
        }
    }
}