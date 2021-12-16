using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IEmployeeRepository _iEmployeeRepository;

        public EmployeeController(AppDbContext context, IEmployeeRepository iEmployeeRepository)
        {
            _context = context;
            _iEmployeeRepository = iEmployeeRepository;
        }
        public ViewResult Shit()
        {
            var model = _iEmployeeRepository.GetAllEmployee();
            ViewBag.Employee = model;
            ViewBag.PageHeading = "Employee Detail";
            return View(model);
        }
    }
}
