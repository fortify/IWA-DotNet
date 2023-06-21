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
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Text;

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
        public string sHTML { get; set; }

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

            var products = from p1 in _context.Product select p1;
            if (!string.IsNullOrEmpty(Keywords))
            {
                //var products = productBAL.GetAllProduct(Keywords, _iconfiguration);
                products = products.Where(s => s.Name.Contains(Keywords));
            }

            //Product = await products.ToListAsync();
            Product = products.ToList();

            SearchCount = Product.Count();

            sHTML = "<div class=\"body_padded\"><h1>" + Keywords + " trying seach</h1><div id =\"code\"><table width = '100%' bgcolor='white' style=\"border:2px #C0C0C0 solid\">" +
                "<tr>	<td><div id =\"code\">" + ProductCount.ToString() + "</div></td></tr>	</table>	</div>	<br />	<br />	<FORM><INPUT TYPE =\"BUTTON\" VALUE=\"Compare\" ONCLICK=\"window.location.href='javascript:alert(1)'\">" +
                "</FORM></div>";
            ViewData["ScriptInput"] = "alert(1);";

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


        public static async Task WriteOutput(HttpResponse Response, string Title, string Body)
        {
            //Controllers.ProductsController pc = new Controllers.ProductsController(contextnew);
            var sb = new StringBuilder();
            //sb.Append("<!DOCTYPE html><head><meta charset=\"utf-8\">");
            //if (Title != null)
            //{
            //    sb.Append("<title>" + Title + "</title>");
            //}
            //sb.Append("</head><body>");
            //sb.Append(Body);
            //sb.Append("</body></html>");
            //Task<IActionResult> result = pc.GetTestResponse();
            sb.Append("<div>");
            sb.Append(Body);
            //sb.Append(result.ToString()) ;
            sb.Append("</div>");
            await HttpResponseWritingExtensions.WriteAsync(Response, sb.ToString());
        }
    }


}
