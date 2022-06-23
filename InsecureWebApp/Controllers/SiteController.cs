using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SiteController : ControllerBase
    {

        private readonly ILogger _logger;

        public SiteController(ILogger<SiteController> logger)
        {
            _logger = logger;
        }
        public string Message { get; set; }

        // POST: api/v1/site/subscribe-newsletter-xml
        [HttpPost("subscribe-newsletter-xml")]
        public IActionResult SubscribeNewsletterXML([FromForm] String name, [FromForm] String email)
        {
            Message = $"SubscribeNewsletter request at {DateTime.UtcNow.ToLongTimeString()} with name: {name} and email: {email}";
            _logger.LogInformation(Message);

            FileStream fileStream = new FileStream("email-db.xml", FileMode.Create);
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            XmlWriter writer = XmlWriter.Create(fileStream, settings);

            XmlDocument doc = new XmlDocument();
            XmlElement users = doc.CreateElement("users");
            doc.AppendChild(users);
            XmlElement newUser = doc.CreateElement("user");
            users.AppendChild(newUser);
            newUser.InnerXml = email;
            doc.WriteTo(writer);

            writer.Flush();
            fileStream.Flush();
            return Ok(new { success = true });
        }

        // POST: api/v1/site/subscribe-newsletter-json
        [HttpPost("subscribe-newsletter-json")]
        public IActionResult SubscribeNewsletterJSON([FromForm] String name, [FromForm] String email)
        {
            Message = $"SubscribeNewsletter request at {DateTime.UtcNow.ToLongTimeString()} with name: {name} and email: {email}";
            _logger.LogInformation(Message);

            FileStream fileStream = new FileStream("email-db.json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fileStream);
            JsonWriter writer = new JsonTextWriter(sw)
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            };

            writer.WriteStartObject();

            writer.WritePropertyName("role");
            writer.WriteRawValue("\"default\"");

            writer.WritePropertyName("name");
            writer.WriteRawValue("\"" + name + "\"");

            writer.WritePropertyName("email");
            writer.WriteRawValue("\"" + email + "\"");

            writer.WriteEndObject();
           
            writer.Flush();

            fileStream.Flush();
            fileStream.Close();

            return Ok(new { success = true });
        }

    }
}
