using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        IBookRepository Book { get;}

        public ICartItemTempRepository CartItemTemp { get;}

        public ICartItemRepository CartItem { get;}

        public ICartRepository Cart { get; }

        public IOrderRepository Order { get; }
        void Save();
    }
}
