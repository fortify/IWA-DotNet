using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Models
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int Total { get { return Products.Count(); } }
        public int TotalOnSale {  get { return Products.Count(product => product.OnSale);  } }
    }
}
