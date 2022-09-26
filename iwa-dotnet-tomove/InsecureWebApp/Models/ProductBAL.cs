using MicroFocus.InsecureWebApp.Data;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MicroFocus.InsecureWebApp.Models
{
    public class ProductBAL
    {
        public List<Product> GetAllProduct(string sSearchText, IConfiguration config)
        {
            string sKeyword = string.Empty;
            if (!string.IsNullOrEmpty(sSearchText))
            {
                sKeyword = sSearchText.Trim().ToLower();
            }
            ProductDAL p = new ProductDAL(config);
            return p.GetAllProduct(sKeyword);
        }
    }
}
