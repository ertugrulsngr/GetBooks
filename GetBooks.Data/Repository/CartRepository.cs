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
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext db) : base(db) { }
        public void Update(Cart item)
        {
            dbset.Update(item);
        }
    }
}
