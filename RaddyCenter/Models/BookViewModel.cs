using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaddyCenter.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int Year { get; set; }
   
        public string CoverImagePath { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Available { get; set; }
    }
}
