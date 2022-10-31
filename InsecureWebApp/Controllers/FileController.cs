using Microsoft.AspNetCore.Mvc;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        private const string PRESCRIPTION_LOCATION = "Files\\Prescriptions\\";

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file, string zipFileName, string targetDir = "")
        {
            string sPres_Location = "Files" + Path.DirectorySeparatorChar + "Prescriptions" + Path.DirectorySeparatorChar;
            string sDir = Path.Combine(Directory.GetCurrentDirectory(), sPres_Location);
            string sPath = Path.Combine(Directory.GetCurrentDirectory(), targetDir) + Path.DirectorySeparatorChar +  zipFileName;
            FastZip fastZip = new FastZip();
            string fileFilter = null;
            string sFinalDir = string.Empty;


            using (var stream = new FileStream(sDir + zipFileName, FileMode.Create))
            {
                await file.CopyToAsync(stream);

                sFinalDir = stream.Name;
            }
            if (string.IsNullOrEmpty(targetDir))
            {
                targetDir = sPres_Location;
            }
            fastZip.ExtractZip(sDir + zipFileName, targetDir, fileFilter);

            sFinalDir = Directory.GetParent(Path.GetFullPath(sPath)).FullName;
            
            return Ok("File extracted at : " + sFinalDir);

            //Response.Headers.Add("Content-Disposition", "attachment; filename="+ zipFileName +".zip");
            //return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(zipFileName), "application/json");
        }
    }
}
