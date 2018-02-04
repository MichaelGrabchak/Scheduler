using System.Collections.Generic;

namespace Scheduler.Domain.Data.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(object key);
        void Add(T obj);
        void Update(T obj);
        void Delete(object key);
    }
}
