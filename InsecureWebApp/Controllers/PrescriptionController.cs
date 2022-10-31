using MicroFocus.InsecureWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private const string PRESCRIPTION_LOCATION = "Files\\Prescriptions\\";

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

        [HttpPost("UploadXml")]
        public IActionResult UploadXml(string sPath)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.XmlResolver = new XmlUrlResolver();
            xdoc.LoadXml(sPath);

            return Content(xdoc.InnerText);
        }

        [HttpPost("UploadFile")]
        public async Task<bool> UploadFile(IFormFile file, string sPath)
        {
            bool blnResult = false;
            try
            {
                using (var stream = new FileStream(sPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                blnResult = true;
            }catch (Exception ex)
            {
                throw new FileLoadException(ex.Message);
            }
            return blnResult;
        }

        [HttpPost("UpdateXml")]
        public async Task<bool> UpdateXml(string sFileName, string xmlContent)
        {
            bool retFunc = false;
            if (!string.IsNullOrEmpty(xmlContent))
            {
                await Task.Delay(100);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Prescriptions" + Path.DirectorySeparatorChar) + sFileName;

                XmlDocument document = new XmlDocument();
                document.Load(path);

                document.InnerXml = xmlContent;
                document.Save(path);
                retFunc = true;
            }
            return retFunc;
        }

        [HttpGet("DownloadFile")]
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Prescriptions" + Path.DirectorySeparatorChar) + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

    }
}
