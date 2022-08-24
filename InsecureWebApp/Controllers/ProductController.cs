using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureWebApp.Models;
using MicroFocus.InsecureWebApp.Data;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MicroFocus.InsecureWebApp.Controllers
{
    #region ProductsController
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion
        // POST: api/v1/products
        [SwaggerOperation(
            OperationId = "InsertSearchText",
            Description = "Requires user id",
            Summary = "Save Keyword search user wise"
        )]
        [HttpPost("InsertSearchText")]
        public async Task<IActionResult> InsertSearchText(String keywords, string UserId)
        {
            String query = string.Empty;
            if (!String.IsNullOrEmpty(keywords) && (!String.IsNullOrEmpty(UserId)))
            {
                query = "DELETE FROM ProductSearch where UserId='" + UserId + "'; INSERT INTO ProductSearch (SearchText,UserId) VALUES ('" + keywords + "', '" + UserId + "')";
                _context.Database.ExecuteSqlCommand(query);
            }
            return Ok(query);
        }

        // GET: api/v1/products
        [SwaggerOperation("GetProducts")]
        [HttpGet("GetProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts(String keywords, long limit = 50)
        {
            /* FvB */
            var query = "SELECT TOP " + Convert.ToInt32(limit) + " * FROM dbo.Product WHERE (" +
                " Name LIKE '%" + keywords + "%' OR " +
                " Summary LIKE '%" + keywords + "%' OR " +
                " Description LIKE '%" + keywords + "%')";

            var products1 = _context.Database.ExecuteSqlCommand(query);
            //return products;
            //var products1 = _context.Product.FromSqlRaw(query);
            /*var products = from p in _context.Product
                           select p;

            if (!String.IsNullOrEmpty(keywords))
            {
                products = products.Where(p => p.Name.Contains(keywords)
                              || p.Summary.Contains(keywords)
                              || p.Description.Contains(keywords));
            }

            if (limit > 0)
            {
                products = products.Take(Convert.ToInt32(limit));
            }
            return await products.AsNoTracking().ToListAsync();*/
            var products = _context.Product.FromSqlRaw("SELECT TOP " + Convert.ToInt32(limit) + " *, Description as HtmlContent FROM dbo.Product WHERE (" +
                " Name LIKE '%" + keywords + "%' OR " +
                " Summary LIKE '%" + keywords + "%' OR " +
                " Description LIKE '%" + keywords + "%')").ToList();
            return products;
        }

        [SwaggerOperation("GetTestResponse")]
        [HttpGet("GetTestResponse")]
        public async Task<IActionResult> GetTestResponse()
        {
            //for testing of XSS purpose
            var response = Content("Test Success !!");
            return Ok(response.Content.ToString());

            //await HttpResponseWritingExtensions.WriteAsync(HttpContext.Response, "Hello World");
            //return Ok(HttpContext.Response);
        }

        #region snippet_GetByID
        // GET: api/v1/products/5
        [SwaggerOperation(OperationId = "GetProductById")]
        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Product = await _context.Product.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Product;
        }
        #endregion

        #region snippet_Update
        // PUT: api/v1/products/5
        [SwaggerOperation(OperationId = "PutProduct")]
        [HttpPut("PutProductById/{id}")]
#pragma warning disable SF8FC171238E9435BBC5E53DF24BB279A // Mass Assignment : Insecure Binder Configuration
        public async Task<IActionResult> PutProduct(int id, Product Product)
#pragma warning restore SF8FC171238E9435BBC5E53DF24BB279A // Mass Assignment : Insecure Binder Configuration
        {
            if (id != Product.ID)
            {
                return BadRequest();
            }

            _context.Entry(Product).State = EntityState.Modified;

            if (await TryUpdateModelAsync<Product>(Product,"", c => c.Code, 
                c => c.Price, 
                c => c.Name, 
                c => c.Description ))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }
        #endregion

        #region snippet_Create
        // POST: api/v1/products
        [SwaggerOperation(OperationId = "PostProduct")]
        [HttpPost("PostProduct")]
#pragma warning disable SF8FC171238E9435BBC5E53DF24BB279A // Mass Assignment : Insecure Binder Configuration
        public async Task<ActionResult<Product>> PostProduct(Product Product)
#pragma warning restore SF8FC171238E9435BBC5E53DF24BB279A // Mass Assignment : Insecure Binder Configuration
        {
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProduct", new { id = Product.ID }, Product);
            return CreatedAtAction(nameof(GetProduct), new { id = Product.ID }, Product);
        }
        #endregion

        #region snippet_Delete
        // DELETE: api/v1/products/5
        //[Route("DeleteProduct", Name = "DeleteProduct")]
        [SwaggerOperation(OperationId = "DeleteProduct")]
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var Product = await _context.Product.FindAsync(id);
            if (Product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(Product);
            await _context.SaveChangesAsync();

            return Product;
        }
        #endregion

        private bool ProductExists(long id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}