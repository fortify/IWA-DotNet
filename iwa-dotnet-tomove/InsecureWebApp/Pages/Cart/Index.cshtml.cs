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

namespace MicroFocus.InsecureWebApp.Pages.Cart
{
    public class IndexModel : PageModel
    {
        public string CurrencySymbol { get; set; }

        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;

        public IndexModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; }
        [BindProperty(SupportsGet = true)]
        public int ProductCount { get; set; }

        public async Task OnGetAsync()
        {
            CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;

            Product = await _context.Product.ToListAsync();
        }
    }
}
