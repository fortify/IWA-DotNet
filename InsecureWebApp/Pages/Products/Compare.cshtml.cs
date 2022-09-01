using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Pages.Products
{
    public class CompareModel : PageModel
    {
        private static readonly IConfiguration _iconfiguration;

        public List<MicroFocus.InsecureWebApp.Models.Product> Products { get; set; }
        public string jSonResult { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;

        public CompareModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string url { get; set; }

        public void OnGetAsync()
        {

        }

        public void OnPost(string url)
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(url))
            //    {
            //        WebRequest request = WebRequest.Create(url);
            //        WebResponse response = request.GetResponse();
            //        if (response != null)
            //        {
            //            Controllers.ProductsController pc = new Controllers.ProductsController(_context);
            //            var products = await pc.GetProductFromOtherSource(url);
            //            jSonResult = products;
            //            this.url = url;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

        }

        public async Task<IActionResult> OnPostPullProducts(string url)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            if (!string.IsNullOrEmpty(url))
            {
                this.url = url;
                HttpClient client = new HttpClient(httpClientHandler);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Controllers.ProductsController pc = new Controllers.ProductsController(_context);
                    var products = await pc.GetProductFromOtherSource(url);
                    jSonResult = products;
                }
            }
            return null;
        }
    }
}
