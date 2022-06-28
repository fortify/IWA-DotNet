using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureWebApp.Data;
using MicroFocus.InsecureWebApp.Models;
using System.Xml;
using System.Globalization;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Claims;

namespace MicroFocus.InsecureWebApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        private static IConfiguration _iconfiguration;

        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;
        public string CurrencySymbol { get; set; }

        public IndexModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }
        [BindProperty(SupportsGet = true)]
        public string Keywords { get; set; }
        public int ProductCount { get; set; }
        public int SearchCount { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            GetAppSettingsFile();
            Controllers.ProductsController pc = new Controllers.ProductsController(_context);

            await pc.InsertSearchText(Keywords, userId);

            ActionResult<IEnumerable<Product>> p =  pc.GetProducts(Keywords, 50);



            var productBAL = new ProductBAL();

            CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;

            ProductCount = _context.Product.Count();
            //var products = productDAL.GetAllProduct(Keywords);

            //var products = from p in _context.Product select p;
            //if (!string.IsNullOrEmpty(Keywords))
            //{
                var products = productBAL.GetAllProduct(Keywords, _iconfiguration);
            //products = products.Where(s => s.Name.Contains(Keywords));
            //}

            //Product = await products.ToListAsync();
            Product = products.ToList();

            SearchCount = Product.Count();

            //Product = await _context.Product.ToListAsync();
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",
                    optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }
    }
}
