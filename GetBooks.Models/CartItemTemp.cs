using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.Models
{
    public class CartItemTemp
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int BookId { get; set; }
        
        [ForeignKey("BookId")]
        [ValidateNever]
        public Book Book { get; set; }
        
        [Required]
        public int CartId { get; set; }

        [ForeignKey("BookId")]
        [ValidateNever]
        public Cart Cart { get; set; }
    }
}
