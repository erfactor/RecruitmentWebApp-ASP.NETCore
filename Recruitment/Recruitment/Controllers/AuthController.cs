using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Recruitment.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [Route("signup")]
        [HttpGet]
        public IActionResult SignUp()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "B2C_1_sign_up");
        }
        
        [Route("signout")]
        [HttpPost]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var scheme = User.FindFirst("tfp").Value;
            await HttpContext.SignOutAsync(scheme);
        }
        
        [HttpGet]
        [Route("signin")]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties {RedirectUri = "/"}, "B2C_1_sign_in");
        }

        [HttpGet]
        [Route("editprofile")]
        public IActionResult EditProfile()
        {
            return Challenge(new AuthenticationProperties {RedirectUri = "/"}, "B2C_1_edit_profile");
        }
    }
}