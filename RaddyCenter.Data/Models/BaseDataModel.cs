using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Text;

namespace RaddyCenter.Data.Models
{
    public class BaseDataModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool Disabled { get; set; } = false;

        public DateTime? DisabledOn { get; set; }

    }
}
