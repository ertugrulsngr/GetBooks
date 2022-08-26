using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GetBooks.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Description { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}