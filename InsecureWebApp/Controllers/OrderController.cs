using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using MicroFocus.InsecureWebApp.Models;
using System.Collections.Generic;
using MicroFocus.InsecureWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("UpdateOrder")]
        public async Task<Order> UpdateOrder([FromBody] Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        [HttpGet("GetOrder")]
        public async Task<List<Order>> GetOrder([FromQuery] Order order)
        {
            dynamic _order;
            if (order.IsAdmin)
            {
                _order = await _context.Order
                    .Include(a => a.User)
                    .Include(a => a.LineItems)
                    .Include("LineItems.Product")
                    .Where(a => a.Id.Equals(order.Id))
                    .ToListAsync();
            }
            else
            {
                _order = await _context.Order
                    .Where(a => a.Id.Equals(order.Id))
                    .ToListAsync();
            }
            
            //if (_order == null) return null;

            return _order;
        }

        [HttpGet("GetOrderDiscount")]
        public async Task<ActionResult<int>> GetOrderDiscount([FromQuery] Order order)
        {
            dynamic _order;
            if (order.IsAdmin)
            {
                _order = await _context.Order
                    .Include(a => a.User)
                    .Include(a => a.LineItems)
                    .Include("LineItems.Product")
                    .Where(a => a.Id.Equals(order.Id))
                    .ToListAsync();
            }
            else
            {
                _order = await _context.Order
                    .Where(a => a.Id.Equals(order.Id))
                    .ToListAsync();
            }

            return _order.Discount;
         }

        [HttpPost("ReadOrderFromFile")]
        public List<tmpOrder> ReadOrderFromFile()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files"+ Path.DirectorySeparatorChar +"Order.file");

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(filePath));
            List<tmpOrder> obj = (List<tmpOrder>)binaryFormatter.Deserialize(memoryStream);
            return obj;
        }
    }
}
