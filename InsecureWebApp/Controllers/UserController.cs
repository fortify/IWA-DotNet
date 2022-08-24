using MicroFocus.InsecureWebApp.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MicroFocus.InsecureWebApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;
        private readonly HttpContext _httpContext;

        public UserController(ILogger<LoginModel> logger, SignInManager<IdentityUser> signInManager, HttpContext httpContext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _httpContext = httpContext;
        }

        [HttpPost("AuthenticateUser")]
        public async Task<Microsoft.AspNetCore.Identity.SignInResult> AuthUser([FromForm] string email, [FromForm] string pwd, [FromForm] bool RememberMe)
        {
            string sUserName = WebUtility.UrlDecode(email);
            sUserName = sUserName.Trim();

            string Message = $"Login attempt at {DateTime.UtcNow.ToLongTimeString()} with name: {sUserName}";
            _logger.LogInformation(Message);
            _logger.LogInformation("Trying Authentication for User: " + WebUtility.UrlDecode(sUserName));
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(email, pwd, RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("{0} logged in.", sUserName);
                _httpContext.Session.SetString("AuthToken", sUserName);
                _httpContext.Response.Cookies.Append("AuthToken", sUserName);
                _httpContext.Session.SetString("SessID", _httpContext.Session.Id);
                _httpContext.Response.Cookies.Append("SessID", _httpContext.Session.Id);
            }

            return result;
        }
    }
}
