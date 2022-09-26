using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MicroFocus.InsecureWebApp.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MicroFocus.InsecureWebApp.Areas.Identity.Pages.Account.Manage
{
    public class OrderDataModel : PageModel
    {
        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrderDataModel> _logger;

        public OrderDataModel(UserManager<ApplicationUser> userManager,
            ILogger<OrderDataModel> logger, MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Models.Order _order { get; set; }

        public List<Models.Order> Orders { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            List<Models.Order> lstOrder = new List<Order>();

            if(User.IsInRole("Admin"))
            {
                lstOrder = await _context.Order
                    .Include(a => a.User)
                    .Include(a => a.LineItems)
                    .Include("LineItems.Product")
                    .ToListAsync();
            }
            else
            {
                lstOrder = await _context.Order
                    .Include(a => a.User)
                    .Include(a => a.LineItems)
                    .Include("LineItems.Product")
                    .Where(a => a.User.Email.Equals(user.Email))
                    .ToListAsync();
            }



            Orders = lstOrder;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User with ID '{UserId}' asked to generte sample Order data.", _userManager.GetUserId(User));

            Product p = await _context.Product.FirstOrDefaultAsync(m => m.ID == 5);
            Product p1 = await _context.Product.FirstOrDefaultAsync(m => m.ID == 4);
            var order = new Models.Order();

            order.Date = System.DateTime.Today;
            order.Discount = 3;
            order.User = user;
            order.LineItems = new System.Collections.Generic.List<OrderDetail>()
            {
                new OrderDetail() {
                    OrderId = order.Id,
                    Qty = 2,
                    Product = p
                },
                new OrderDetail()
                {
                    OrderId = order.Id,
                    Qty = 5,
                    Product = p1
                }
            };

            OrderController oc = new OrderController(_context);
            _order = await oc.UpdateOrder(order);

            return RedirectToPage();
        }
    }
}
