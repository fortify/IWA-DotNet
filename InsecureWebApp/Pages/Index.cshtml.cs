using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Xml;

namespace MicroFocus.InsecureWebApp.Pages
{
    public class IndexModel : PageModel
    {
        public string CurrencySymbol { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            CurrencySymbol = NumberFormatInfo.CurrentInfo.CurrencySymbol;

            //XmlTextReader reader = new XmlTextReader("email-subscribers.xml");
            //reader.DtdProcessing = DtdProcessing.Parse;
            //while (reader.Read())
            //{
            //    var data = reader.Value;
            //}
        }
    }
}
