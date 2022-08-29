using GetBooks.DataAccess.Data;
using GetBooks.DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IBookRepository Book { get; private set; }

        public ICartItemTempRepository CartItemTemp { get; private set; }

        public ICartRepository Cart { get; private set; }

        public ICartItemRepository CartItem { get; private set; }

        public IOrderRepository Order { get; private set; }

        private readonly ApplicationDbContext db;

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            Book = new BookRepository(db);
            CartItemTemp = new CartItemTempRepository(db);
            Cart = new CartRepository(db);
            CartItem = new CartItemRepository(db);
            Order = new OrderRepository(db);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
