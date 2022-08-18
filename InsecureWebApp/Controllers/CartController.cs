using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        [Authorize]
        [HttpGet("ViewDirectoryContent")]
        public List<FileModel> ViewDirectoryContent()
        {
            //Fetch all files in the Folder (Directory).
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory());

            //Copy File names to Model collection.
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel { 
                    Name = Path.GetFileName(filePath), 
                    Path = Path.GetFullPath(filePath), 
                    DirectoryName=Path.GetDirectoryName(filePath)  
                });
            }

            return files;
        }

        [HttpGet("SaveOrder")]
        public string SaveOrder(string jSonOrder)
        {
            //tmpOrder to = new tmpOrder();
            string sRetVal = string.Empty;
            JArray json = JArray.Parse(jSonOrder);

            var to = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tmpOrder>>(json.ToString());
            string filePath = Directory.GetCurrentDirectory();
            filePath = filePath + "\\Files\\Order.file";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream oStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(oStream, to);
            }

            OrderController oc = new OrderController();
            List<tmpOrder> to1 = oc.ReadOrderFromFile();

            sRetVal = "function called";
            sRetVal = Newtonsoft.Json.JsonConvert.SerializeObject(to1);
            return sRetVal;
        }
    }
}
