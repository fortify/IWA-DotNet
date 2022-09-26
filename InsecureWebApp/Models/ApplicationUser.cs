using Microsoft.AspNetCore.Identity;

namespace MicroFocus.InsecureWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string CreditCardNo { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
    }
}
