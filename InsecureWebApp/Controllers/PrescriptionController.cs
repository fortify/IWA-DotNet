using MicroFocus.InsecureWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Controllers
{
    public class PrescriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PrescriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ContentResult GetSearchText(string sSearchText)
        {

            return Content("Prescription Search By : " + sSearchText, "text/html");
        }

        [HttpGet("GetPrescription")]
        public List<Models.Prescription> GetPrescription(string sSearchText)
        {
            List<Models.Prescription> pres;
            if (string.IsNullOrEmpty(sSearchText))
            {
                pres = _context.Prescription.ToList();
            }
            else
            {
                pres = _context.Prescription.Where(m => m.DocName.Contains(sSearchText)).ToList();
            }
            return pres; //Ok(pres);
        }

        [HttpGet("GetDoctorName")]
        public Models.Prescription GetDoctorNameByPresId(int iPresId)
        {
            var pres = _context.Prescription.Where(m => m.ID.Equals(iPresId)).FirstOrDefault();
            return pres;
        }
    }
}
