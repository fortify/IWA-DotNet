using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MicroFocus.InsecureProductService.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "On sale")]
        public bool OnSale { get; set; }

        [Display(Name = "Sale price")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SalePrice { get; set; }

        [Display(Name = "In Stock")]
        public bool InStock { get; set; }

        [Display(Name = "Time to stock")]
        public int TimeToStock { get; set; }

        public int Rating { get; set; }

        public bool Available { get; set; }
    }
}
