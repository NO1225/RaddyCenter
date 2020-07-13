using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RaddyCenter.Models
{
    public class CreateBookViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

        public int Year { get; set; }

        [Required]
        [Display(Name ="Cover Image")]
        public IFormFile CoverImage { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100)]
        public int Quantity { get; set; }

    }
}
