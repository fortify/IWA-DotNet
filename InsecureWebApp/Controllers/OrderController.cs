using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using MicroFocus.InsecureWebApp.Models;
using System.Collections.Generic;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("ReadOrderFromFile")]
        public List<tmpOrder> ReadOrderFromFile()
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath = filePath + "\\Files\\Order.file";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
            List<tmpOrder> obj = (List<tmpOrder>)binaryFormatter.Deserialize(memoryStream);
            return obj;
        }
    }
}
