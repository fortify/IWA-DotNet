using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MicroFocus.InsecureWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using MicroFocus.InsecureWebApp.Controllers;

namespace MicroFocus.InsecureWebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class AddressModel : PageModel
    {
        private const string HASHKEY = "{12345678901234567890123456 key}";
        private const string HASHIV = "{1234567890 key}";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public byte[] enckey = Encoding.UTF8.GetBytes(HASHKEY);
        public byte[] enciv = Encoding.UTF8.GetBytes(HASHIV);

        public AddressModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Address")]
            public string Address { get; set; }

            [CreditCard]
            [Display(Name ="Credit Card")]
            public string CreditCard { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var address = user.Address;
            string creditcard = string.Empty;
            if (!string.IsNullOrEmpty(user.CreditCardNo)){
                using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                {
                    byte[] bytes = Convert.FromBase64String(user.CreditCardNo);
                    creditcard = CryptoController.DecryptStringFromBytes_Aes(bytes, enckey, enciv).Replace("\0","");
                }
            }

            Username = userName;

            Input = new InputModel
            {
                Address = address,
                CreditCard = creditcard
            };
        }


        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            var address = user.Address;
            var creditcard = user.CreditCardNo;

            if (Input.Address != address)
            {
                user.Address = Input.Address;
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    StatusMessage = "Unexpected error when trying to update address: " + ex.Message;
                    return RedirectToPage();
                }
            }

            if (Input.CreditCard != creditcard)
            {
                if (!string.IsNullOrEmpty(Input.CreditCard))
                {
                    using (AesCryptoServiceProvider myAes = new AesCryptoServiceProvider())
                    {
                        var enCreditCard = CryptoController.EncryptStringToBytes_Aes(Input.CreditCard, enckey, enciv);
                        user.CreditCardNo = Convert.ToBase64String(enCreditCard);
                    }
                }
                else
                { 
                    user.CreditCardNo = string.Empty; 
                }

                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    StatusMessage = "Unexpected error when trying to update credit card: " + ex.Message;
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public static string encryptCardNo(string sCreditCardNo)
        {
            var enCreditCard = sCreditCardNo;
            var dsa1 = new DSACryptoServiceProvider(); // Noncompliant - default key size is 1024
            dsa1.KeySize = 2048; // Noncompliant - the setter does not update the underlying key size for the DSACryptoServiceProvider class
            
            var dsa2 = new DSACryptoServiceProvider(2048); // Noncompliant - cannot create DSACryptoServiceProvider with a key size bigger than 1024

            var rsa1 = new RSACryptoServiceProvider(); // Noncompliant - default key size is 1024
            rsa1.KeySize = 2048; // Noncompliant - the setter does not update the underlying key size for the RSACryptoServiceProvider class

            var rsa2 = new RSACng(1024); // Noncompliant

            return enCreditCard;
        }

        private static string CalculateSha(string text, Encoding enc)
        {
            var buffer = enc.GetBytes(text);
            using var cryptoTransformSha1 = new SHA1CryptoServiceProvider(); // <=

            var hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer))
                                   .Replace("-", string.Empty);

            return hash.ToLower();
        }

        
    }
}
