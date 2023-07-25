using LiteDB;
using NewsReader.DataStorage.Interfaces.Configuration;
using NewsReader.DataStorage.Interfaces.UnitOfWork;
using System.IO;

namespace NewsReader.DataStorage.SqlLite
{
    public class LiteDbUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly DatabaseConfiguration _databaseConfiguration;

        public LiteDbUnitOfWorkFactory(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public IUnitOfWork Create()
        {
            var database = _databaseConfiguration.UseInMemoryDatabase
                ? CreateInMemoryDatabase()
                : CreateDatabaseFromConnectionString();

            return new LiteDbUnitOfWork(database);
        }

        private static LiteDatabase CreateInMemoryDatabase() => new(new MemoryStream());

        private LiteDatabase CreateDatabaseFromConnectionString() =>
            new(_databaseConfiguration.ConnectionString);
    }
}