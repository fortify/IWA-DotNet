using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using MicroFocus.InsecureWebApp.Models;
using MicroFocus.InsecureWebApp.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net;
using System;
using System.Text;

namespace MicroFocus.InsecureWebApp.Pages.Prescription
{
    public class PresModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        public List<MicroFocus.InsecureWebApp.Models.Prescription> Prescriptions { get; set; }
        public Controllers.PrescriptionController pc;

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }

        public PresModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
            pc = new PrescriptionController(_context);
        }

        public async Task OnGet()
        {
            await Task.Delay(1);
            List<Models.Prescription> p = pc.GetPrescription(Search);
            //Search = pc.GetSearchText(Search);
            Prescriptions = p;
        }

        public string GetSearchText()
        {
            ContentResult cr = pc.GetSearchText(Search);
            ViewData.Add("XSS", cr.Content);
            return cr.Content.ToString();
        }

        public IActionResult OnGetDoctorName(string ID, string Msg)
        {
            int iPresId;
            int.TryParse(ID, out iPresId);
            Models.Prescription pres = pc.GetDoctorNameByPresId(iPresId);
            var result = new ObjectResult(pres.DocName + " " + Msg + " " + ID)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            return result;
        }
    }
}
