using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RaddyCenter.Data.Models
{
    public class Book : BaseDataModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }

        public int Year { get; set; }

        [Required]
        [MaxLength(1000)]
        public string CoverImagePath { get; set; }

        [MaxLength(1500)]
        public string Description { get; set; }

        [Required]
        [Range(0,100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 100)]
        public int Available { get; set; }
    }
}
