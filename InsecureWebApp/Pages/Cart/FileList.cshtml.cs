using MicroFocus.InsecureWebApp.Controllers;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MicroFocus.InsecureWebApp.Pages.Cart
{
    public class FileListModel : PageModel
    {
        private readonly MicroFocus.InsecureWebApp.Data.ApplicationDbContext _context;

        public FileListModel(MicroFocus.InsecureWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FileModel> Files { get; set; }
        public void OnGet()
        {
            CartController cc = new CartController(_context);
            this.Files = cc.ViewDirectoryContent();
        }
    }
}
