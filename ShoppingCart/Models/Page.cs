using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Models
{
    public class Page
    {
        [Display(Name ="ID")]
        public int id { get; set; }
        [Required, MinLength(2,ErrorMessage="Minium length is 2")]
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required]
        public string Content { get; set; }
        public int Sorting { get; set; }

    }
}
