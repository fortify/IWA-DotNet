using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace MicroFocus.InsecureWebApp.Pages
{
    public class PrescriptionsModel : PageModel
    {
        private readonly ILogger<PrescriptionsModel> _logger;

        public PrescriptionsModel(ILogger<PrescriptionsModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
