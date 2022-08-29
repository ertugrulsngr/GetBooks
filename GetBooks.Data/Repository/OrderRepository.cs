using GetBooks.DataAccess.Data;
using GetBooks.DataAccess.IRepository;
using GetBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext db) : base(db) { }
        public void Update(Order item)
        {
            dbset.Update(item);
        }
    }
}
