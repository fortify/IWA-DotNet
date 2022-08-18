using MicroFocus.InsecureWebApp.Controllers;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace MicroFocus.InsecureWebApp.Pages.Cart
{
    public class FileListModel : PageModel
    {
        public List<FileModel> Files { get; set; }
        public void OnGet()
        {
            CartController cc = new CartController();
            this.Files = cc.ViewDirectoryContent();
        }
    }
}
