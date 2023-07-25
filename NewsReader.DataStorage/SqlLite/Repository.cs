using System.Collections.Generic;
using LiteDB;
using NewsReader.DataStorage.Interfaces.Repository;

namespace NewsReader.DataStorage.SqlLite
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ILiteCollection<T> _collection;

        public Repository(ILiteCollection<T> collection)
        {
            _collection = collection;
        }

        public IEnumerable<T> GetAll() => _collection.FindAll();

        public T GetById(string id) => _collection.FindById(id);

        public void Add(string id, T entity) => _collection.Insert(id, entity);

        public void Update(string id, T entity) => _collection.Update(id, entity);

        public void Upsert(string id, T entity) => _collection.Upsert(id, entity);

        public void Remove(string id) => _collection.Delete(id);
    }
}
