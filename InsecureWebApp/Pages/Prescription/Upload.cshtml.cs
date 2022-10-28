using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using MicroFocus.InsecureWebApp.Models;
using System.Collections.Generic;
using System.Xml;

namespace MicroFocus.InsecureWebApp.Pages.Prescription
{
    public class UploadModel : PageModel
    {
        private readonly Data.ApplicationDbContext _context;
        public Controllers.PrescriptionController pc;
        public List<PrescriptionFileModel> files { get; set; }

        public UploadModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
            pc = new Controllers.PrescriptionController(_context);
        }

        public void OnGet()
        {
            //Copy File names to Model collection.
            this.files = new List<PrescriptionFileModel>();
            try
            {
                //Fetch all files in the Folder (Directory).
                string[] filePaths = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Prescriptions"), "*.xml");


                foreach (string filePath in filePaths)
                {
                    //string fullPath = filePath + "\\" + Path.GetFileName(filePath);
                    files.Add(new PrescriptionFileModel { FileName = Path.GetFileName(filePath), FileDesc = getXmlFileContent(filePath), xml = getXml(filePath) });
                }
            }
            catch (System.Exception ex)
            { }
        }

        public string getXml(string sFilePath)
        {
            string xmlFileContent = string.Empty;
            XmlReaderSettings settings = new XmlReaderSettings();
            //settings.ProhibitDtd = false;
            
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            settings.XmlResolver = new XmlUrlResolver();
            //Method 2
            XmlDocument document = new XmlDocument();
            document.XmlResolver = new XmlUrlResolver();
            document.Load(sFilePath);
            using (var fileStream = System.IO.File.OpenText(sFilePath))
            using (XmlReader reader = XmlReader.Create(sFilePath,settings))
            {
                while (reader.Read())
                {
                    string result = reader.NodeType switch
                    {
                        XmlNodeType.Element when reader.Name == "product" => $"{reader.Name}\n",
                        XmlNodeType.Element => $"{reader.Name}: ",
                        XmlNodeType.Text => $"{reader.Value}\n",
                        XmlNodeType.EndElement when reader.Name == "product" => "-------------------\n",
                        _ => ""
                    };
                    xmlFileContent += result;

                }
            }
            return xmlFileContent;
            //return document.InnerText;
        }

        public string getXmlFileContent(string sFilePath)
        {
            string xmlFileContent = string.Empty;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ProhibitDtd = false;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            //settings.XmlResolver =  = null;

            //Method 1
            XmlDocument document = new XmlDocument();
            document.Load(sFilePath);
            XmlNamespaceManager m = new XmlNamespaceManager(document.NameTable);
            using (StreamReader fileStream = System.IO.File.OpenText(sFilePath))
            using (XmlReader reader = XmlReader.Create(fileStream, settings))
            {
                while (reader.Read())
                {
                    string result = reader.NodeType switch
                    {
                        XmlNodeType.Element when reader.Name == "product" => $"{reader.Name}\n",
                        XmlNodeType.Element => $"{reader.Name}: ",
                        XmlNodeType.Text => $"{reader.Value}\n",
                        XmlNodeType.EndElement when reader.Name == "product" => "-------------------\n",
                        _ => ""
                    };
                    xmlFileContent += result;
                }
            }
            //return xmlFileContent; 
            return document.InnerXml.ToString();

            
        }

        //public FileResult DownloadFile(string fileName)
        //{
        //    //Build the File Path.
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "Files\\Prescriptions") + fileName;

        //    //Read the File data into Byte Array.
        //    byte[] bytes = System.IO.File.ReadAllBytes(path);

        //    //Send the File to Download.
        //    return File(bytes, "application/octet-stream", fileName);
        //}

        public async Task<IActionResult> OnPost(IFormFile PresFile)
        {
            if (PresFile == null || PresFile.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Prescriptions", PresFile.FileName);
            var task = await pc.UploadFile(PresFile, path);
            
            if (!task)
            {
                throw new FileLoadException();
            }
            TempData["FileUpload"] = "File Uploaded Successfully";
            return RedirectToPage("Upload");
        }

        public async Task<IActionResult> OnPostUpdateXML(string sFileName, string xmlContent)
        {
            if (!string.IsNullOrEmpty(xmlContent))
            {
                var task = await pc.UpdateXml(sFileName, xmlContent);

                if (!task)
                {
                    throw new FileLoadException();
                }
            }
            return RedirectToPage("Upload");
        }
    }
}
