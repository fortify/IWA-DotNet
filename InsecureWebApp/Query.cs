
using HotChocolate;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MicroFocus.InsecureWebApp.Models;
using MicroFocus.InsecureWebApp.Data;

namespace MicroFocus.InsecureWebApp
{
    public class Query
    {
        public async Task<List<Order>> GetOrders([Service] ApplicationDbContext _context, int? iOrderId = 0, bool? isAdmin=false)
        {
            List<Order> orders = new List<Order>();
            Controllers.OrderController oc = new Controllers.OrderController(_context);
            Order tmpOrd = new Order();
            tmpOrd.Id = iOrderId.Value;
            tmpOrd.IsAdmin = isAdmin.Value;

            orders = await oc.GetOrder(tmpOrd);

            return orders;
        }

        public async Task<List<Product>> GetProducts([Service] ApplicationDbContext _context, string? keyword = "", int? limit = 50) {
            List<Product> products = new List<Product>();
            Controllers.ProductsController pc = new Controllers.ProductsController(_context);
            var colProducts = pc.GetProducts(keyword, limit.Value);

            List<Product> objPrd = (List<Product>)colProducts.Value;
            // SQLi payload = "a%'); Update Product set price=12.95 where ID=1; --'"
            return objPrd;
        }


    }
}
