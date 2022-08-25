using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GetBooks.DataAccess.Data;
using GetBooks.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GetBooks.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;

        protected DbSet<T> dbset;
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            dbset = db.Set<T>();
        }

        public void Add(T item)
        {
            dbset.Add(item);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string[]? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (string property in includeProperties)
                {
                    query.Include(property);
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string[]? includeProperties = null)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (string property in includeProperties)
                {
                    query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public void Remove(T item)
        {
            dbset.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            dbset.RemoveRange(items);
        }
    }
}
