using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    //our Controller class should inherit from Controller class if we want to retun json data or view(cshtml)
    public class HomeController : Controller 
    {
        private readonly IProjectRepository _iprojectRepository;
        private readonly AppDbContext _context;     
        //constractor injection
        public HomeController(AppDbContext context, IProjectRepository iprojectRepository)
        {
            _context = context;
            _iprojectRepository = iprojectRepository;
        }//end ctor
        public IActionResult Home()
        {
            return View();
        }

        //method to display the home page
        public IActionResult Index()
        {

            
            var Username = HttpContext.Session.GetString("Username");
            
            if (Username != null)
            {
                var model = _iprojectRepository.GetAllProject();//interface in IProjectRepository
                ViewBag.Project = model;
                return View(model);
                //return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

        }//end Index() action or method
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }//end Error action or method
    }//end class HomeController
}
