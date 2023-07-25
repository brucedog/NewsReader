using System;
using NewsReader.DataStorage.Interfaces.Repository;

namespace NewsReader.DataStorage.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        void SaveChanges();
    }
}