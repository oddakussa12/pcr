using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class LoginController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;
        public LoginController(UserManager<IdentityUser> userManager,
                               SignInManager<IdentityUser> signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }//end ctor

        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }//end Login action
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password,
                                    model.RememberMe, true);
                if (result.Succeeded)
                {
                    //if model.Email = odda@ienetworks.co the User_name = odda
                    var user_name = model.Email.Split('@')[0];//get substring of model.Email until the '@' character
                    //Console.WriteLine(s.Split('-')[0]);
                    //set session
                    HttpContext.Session.SetString("Username", user_name );
                    
                    return RedirectToAction("index", "home");
                }
                
                    ModelState.AddModelError(string.Empty,"Invalid Login Attempt");
                 
            }//end if
            return View(model); 
        }//end login action
    }
}
