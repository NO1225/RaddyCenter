using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RaddyCenter.Data.Models
{
    public class Rent : BaseDataModel
    {
        public DateTime RentDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReceiveDate { get; set; }

        [Required]
        public string UserId { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }

        public Book Book { get; set; }

        [Required]
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }
    }
}
