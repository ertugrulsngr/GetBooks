using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, params string[]? includeProperties);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, params string[]? includeProperties);

        void Add(T item);

        void Remove(T item);

        void RemoveRange(IEnumerable<T> items);
    }
}
