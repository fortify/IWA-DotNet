using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureWebApp.Data;
using MicroFocus.InsecureWebApp.Models;
using System.Globalization;

namespace MicroFocus.InsecureWebApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;
        public string CurrencySymbol { get; set; }

        public DetailsModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;

            if (id == null)
            {
                return NotFound();
            }

            Product = await _context.Product.FirstOrDefaultAsync(m => m.ID == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
