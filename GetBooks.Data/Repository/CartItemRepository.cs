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
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext db) : base(db) { }
        public void Update(CartItem item)
        {
            dbset.Update(item);
        }
    }
}
