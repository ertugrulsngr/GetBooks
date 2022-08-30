using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.Models.ViewModels
{
    public class OrderVM
    {
        public Order order { get; set; }
        public IEnumerable<CartItem> cartItems { get; set; } = Enumerable.Empty<CartItem>();
    }
}
