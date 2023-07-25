using System.Collections.Generic;

namespace NewsReader.DataStorage.Interfaces.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();

        T GetById(string id);

        void Add(string id, T entity);

        void Update(string id, T entity);

        void Upsert(string id, T entity);

        void Remove(string id);
    }
}