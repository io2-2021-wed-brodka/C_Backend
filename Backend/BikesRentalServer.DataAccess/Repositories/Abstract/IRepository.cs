using System.Collections.Generic;

namespace BikesRentalServer.DataAccess.Repositories.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        T Get(int id);
        T Add(T entity);
        T Remove(string id);
        T Remove(int id);
    }
}
