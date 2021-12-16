using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInmanager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<IdentityUser>userManager,
                                 SignInManager<IdentityUser>signInManager,
                                 ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInmanager = signInManager;
            this.logger = logger;
        }
        //logout functionality
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInmanager.SignOutAsync();
            HttpContext.Session.Remove("Username");
            
            return RedirectToAction("Login", "Login");//after logout the user redirect to login page
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                //if(user != null && await userManager.IsEmailConfirmedAsync(user))
                if (user != null)  
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new { email = model.Email, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, passwordResetLink);
                    return View("ForgotPasswordConfirmation");
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet] 
        public IActionResult Register()//Register new PM 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register model)//Register new PM 
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result= await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                   await signInmanager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "home");
                    ViewBag.Success = "PM Registered";
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
    }
}
