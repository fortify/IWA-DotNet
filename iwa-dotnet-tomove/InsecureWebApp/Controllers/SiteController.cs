using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            string returnVal = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.XmlResolver = new XmlUrlResolver();
                xdoc.LoadXml(name);
                returnVal += xdoc.InnerText;
            }

            if (!string.IsNullOrEmpty(email))
            {
                XmlDocument xdoc1 = new XmlDocument();
                xdoc1.XmlResolver = new XmlUrlResolver();
                xdoc1.LoadXml(email);
                returnVal += xdoc1.InnerText;
            }

            return Content(returnVal);

            //Commenting for Generating XXE Vulnerabilty
            //string result;
            //Message = $"SubscribeNewsletter request at {DateTime.UtcNow.ToLongTimeString()} with name: {name} and email: {email}";
            //_logger.LogInformation(Message);
            //XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };

            //using (FileStream fileStream = new FileStream("email-db.xml", FileMode.Create))
            //using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
            //{
            //    XmlDocument doc = new XmlDocument();
            //    doc.XmlResolver = new XmlUrlResolver();
            //    XmlElement users = doc.CreateElement("users");
            //    doc.AppendChild(users);
            //    XmlElement newUser = doc.CreateElement("user");
            //    users.AppendChild(newUser);
            //    newUser.InnerXml = email;
            //    doc.WriteTo(writer);

            //    writer.Flush();
            //    fileStream.Flush();
            //    result = doc.InnerText;
            //}
            //return Ok(result);
            //return Ok(new { success = true });
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

        [HttpGet("IsEmailFromTrustedDomain")]
        public bool IsEmailFromTrustedDomain(string sEmail)
        {
            bool sRetVal = false;
            IPAddress[] hostIPsAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            var emailsplit = sEmail.Split(".");
            foreach (IPAddress ip in hostIPsAddress)
            {
                IPAddress hostIPAddress = IPAddress.Parse(ip.ToString());
                IPHostEntry hostInfo = Dns.GetHostByAddress(hostIPAddress);
                if (hostInfo.HostName.EndsWith(emailsplit[1]))
                {
                    sRetVal = true;
                    break;
                }
            }

            return sRetVal;
        }
    }
}
