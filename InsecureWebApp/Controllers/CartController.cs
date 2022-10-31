using MicroFocus.InsecureWebApp.Data;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
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
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

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
                files.Add(new FileModel
                {
                    Name = Path.GetFileName(filePath),
                    Path = Path.GetFullPath(filePath),
                    DirectoryName = Path.GetDirectoryName(filePath)
                });
            }

            return files;
        }

        [HttpGet("SaveOrder")]
        public string SaveOrder([FromQuery] string jSonOrder)
        {
            //tmpOrder to = new tmpOrder();
            string sRetVal = string.Empty;
            try
            {
                JArray json = JArray.Parse(jSonOrder);

                var to = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tmpOrder>>(json.ToString());
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Order.file");

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (FileStream oStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    binaryFormatter.Serialize(oStream, to);
                }

                OrderController oc = new OrderController(_context);
                List<tmpOrder> to1 = oc.ReadOrderFromFile();

                sRetVal = "function called";
                sRetVal = Newtonsoft.Json.JsonConvert.SerializeObject(to1);
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                System.Environment.StackTrace.Insert(0, jSonOrder);
                System.Environment.StackTrace.Insert(1, ex.ToString());
                sRetVal = System.Environment.StackTrace.ToString();
            }
            return sRetVal;
        }


        [HttpGet("CompareProducts")]
        public Product CompareProduct([FromQuery]Product a, [FromForm]Product b)
        {
            Type contractType = typeof(ICart);
            Type implementedContract = typeof(ICart);
            Uri baseAddress = new Uri("http://localhost:5001/base");
            // Create the ServiceHost and add an endpoint.
            Cart oCart = new Cart();
            return oCart.CompareItem(a, b);
        }
    }
}
