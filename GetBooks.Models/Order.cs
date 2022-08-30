using Microsoft.AspNetCore.Identity;
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
    public enum OrderStatus
    {
        [Display(Name ="Waiting For Approval")]
        WaitingForApproval,
        Approved,
        Cancelled
    }
    public class Order
    {
        [Key]
        public int Id { get; set; }

    
        [Required]
        public int CartId { get; set; }

        [ValidateNever]
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }


        [Required]
        public string UserId { get; set; }

        [ValidateNever]
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }


        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public int TotalPrice { get; set; }

        [Required]
        public string Adress { get; set; }
    }
}
