using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.Models.ViewModels
{
    public class SummaryVM
    {
        public IEnumerable<CartItemTemp> CartItems { get; set; }
        public int? CartId { get; set; }
    }
}
