using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroFocus.InsecureProductService.Models;
using MicroFocus.InsecureProductService.Data;

namespace MicroFocus.InsecureProductService.Controllers
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

        // GET: api/v1/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(String keywords, long limit = 50)
        {
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
            var products = _context.Product.FromSqlRaw("SELECT TOP " + Convert.ToInt32(limit) + " * FROM dbo.Product WHERE (" +
                " Name LIKE '%" + keywords + "%' OR " +
                " Summary LIKE '%" + keywords + "%' OR " +
                " Description LIKE '%" + keywords + "%')").ToList();
            return products;
        }

        #region snippet_GetByID
        // GET: api/v1/products/5
        [HttpGet("{id}")]
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product Product)
        {
            if (id != Product.ID)
            {
                return BadRequest();
            }

            _context.Entry(Product).State = EntityState.Modified;

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

            return NoContent();
        }
        #endregion

        #region snippet_Create
        // POST: api/v1/products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product Product)
        {
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetProduct", new { id = Product.ID }, Product);
            return CreatedAtAction(nameof(GetProduct), new { id = Product.ID }, Product);
        }
        #endregion

        #region snippet_Delete
        // DELETE: api/v1/products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
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

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }

        [HttpGet("DelProduct/{id}")]
        public async Task<ActionResult<Product>> DelProduct(int id)
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
    }
}