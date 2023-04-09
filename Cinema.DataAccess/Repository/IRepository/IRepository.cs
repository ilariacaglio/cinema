using System;
using System.Linq.Expressions;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IRepository<T> where T : class
    {
        T? GetFirstOrDefault(int? id);

        IEnumerable<T> GetAll();

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}

