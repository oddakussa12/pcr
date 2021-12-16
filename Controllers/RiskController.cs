using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class RiskController : Controller
    {
        
        private readonly AppDbContext _context;
        private readonly IRisk _iRisk;
        private static int code;//store  ProjectCode passed from ProjectSummary.cshtml
        private static string ProjectName;
        public RiskController(AppDbContext context, IRisk iRisk)
        {
            _context = context;
            _iRisk = iRisk;
            
        }
        
        //method to display all risks of a project in table format
        public IActionResult RiskTable(int id)//id is ProjectCode passed from ProjectSummary.cshtml
        {
            var Username = HttpContext.Session.GetString("Username");

            if (Username != null)
            {
                code = id;//set the Global variable 'code'
                ViewBag.Code = code;
                var model = _iRisk.GetAllProjectRisk(id);//interface in IRisk to display all risks for this projectcode
                ViewBag.Risk = model;
                //get the name of the project based on the id and pass it to RiskTable.cshtml to use it
                //to indicate where the user is at
                ProjectName = _context
                .Projects
                .Where(u => u.ProjectCode == id)
                .Select(u => u.projectName)
                .SingleOrDefault();
                ViewBag.ProjectName = ProjectName;

                //pass the global code value to IssueTable.cshtml
                ViewBag.ProjectCode = code;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }


        }
        
        [HttpPost]
        public IActionResult RiskTable(int id, string search = null)
        {
            id = code;
            if (!string.IsNullOrEmpty(search))
            {
                var foundRisks = _iRisk.SearchRisk(search);
                return View(foundRisks);
            }
            var allRisk = _iRisk.GetAllProjectRisk(id);
            ViewBag.Project = allRisk;

            //pass the global code value to IssueTable.cshtml
            ViewBag.ProjectCode = code;
            return View(allRisk);
        }
        
        // GenerateReport i.e the whole project log file(risk,issue and action log)
        public IActionResult GenerateReport()
        {
            //var data = _context.Risks.ToList();
            var data = _context.Risks.Where(ri=>ri.ProjectProjectCode == code).ToList();
            var data2 = _context.Issues.Where(ri=>ri.ProjectProjectCode == code).ToList();
            var data3 = _context.Actionos.Where(ri => ri.ProjectProjectCode == code).ToList();

            var stream = new MemoryStream();
            using(var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Risk Log");
                var sheet2 = package.Workbook.Worksheets.Add("Issue Log");
                var sheet3 = package.Workbook.Worksheets.Add("Action Log");
                sheet.Cells.LoadFromCollection(data, true);
                sheet2.Cells.LoadFromCollection(data2, true);
                sheet3.Cells.LoadFromCollection(data3, true);
                package.Save();
            }
            stream.Position = 0;
            var fileName = ProjectName+$" Log File_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);

        }
        // Export RiskTable to file
        public IActionResult ExportRisk()
        {
            //var data = _context.Risks.ToList();
            var data = _context.Risks.Where(ri => ri.ProjectProjectCode == code).ToList();
            
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Risk Log");
                sheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            var fileName = ProjectName + $" Risk Log_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
        
        //when we want to edit a risk this Get method display a form with predefined values in its form
        [HttpGet]
        public ViewResult EditProjectRisk(int Id)//Id is the RiskCode of the risk we are editing passed from RiskTable.cshtml
        {
            Risk risk = _iRisk.GetRisk(Id);
            EditUpdateRisk editUpdateRisk = new EditUpdateRisk
            {
                Id = risk.RiskCode,
                RiskAgreedSolution = risk.RiskAgreedSolution,
                RiskCloseDate = risk.RiskCloseDate,
                RiskImpact = risk.RiskImpact,
                RiskOpenDate = risk.RiskOpenDate,
                RiskOwner = risk.RiskOwner,
                RiskStatus = risk.RiskStatus,
                RiskCode = risk.RiskCode,
                RiskDescription = risk.RiskDescription

            };
            ViewBag.ProjectCode = code;
            return View(editUpdateRisk);
        }
        //EditProjectRisk(); is used to add new Risk to Risk Table so this method Post the form data to database
        [HttpPost]
        public IActionResult EditProjectRisk(EditUpdateRisk model)
        {
            if (ModelState.IsValid)
            {
                Risk risk = _iRisk.GetRisk(model.Id);
                risk.RiskDescription = model.RiskDescription;
                risk.RiskImpact = model.RiskImpact;
                risk.RiskOwner = model.RiskOwner;
                risk.RiskStatus = model.RiskStatus;
                risk.RiskOpenDate = model.RiskOpenDate;
                risk.RiskCloseDate = model.RiskCloseDate;
                risk.RiskAgreedSolution = model.RiskAgreedSolution;

                _iRisk.Update(risk);//Update method is found in IRiskRepository.cs
                return RedirectToAction("RiskTable", new { id = code });
            }
            return View();
        }
        //method to delete a project risk no view is required to delete a risk so this method work on get method
        public async Task<IActionResult> DeleteProjectRisk(int id)
        {
            var risk = await _context.Risks.FindAsync(id);
            _context.Risks.Remove(risk);

            await _context.SaveChangesAsync();
            
            return RedirectToAction("RiskTable", new { id = code });
        }
        
        //embi(); is used to add new Risk to Risk Table so this method Get us the form
        [HttpGet]
        public IActionResult embi()
        {
            return View();
        }

        //embi(); is used to add new Risk to Risk Table so this method Post the form data to database
        [HttpPost]
        public async Task<IActionResult> embi(Risk risk)
        {
            
            if (ModelState.IsValid)
            {
                
                Risk ri = new Risk();
                ri.RiskDescription = risk.RiskDescription;
                ri.RiskImpact = risk.RiskImpact;
                ri.RiskOwner = risk.RiskOwner;
                ri.RiskOpenDate = risk.RiskOpenDate;
                ri.RiskCloseDate = risk.RiskCloseDate;
                ri.RiskStatus = risk.RiskStatus;
                ri.RiskAgreedSolution = risk.RiskAgreedSolution;
                ri.ProjectProjectCode = code;
                
                _context.Risks.Add(ri);
                await _context.SaveChangesAsync();
                return RedirectToAction("RiskTable", new { id = code });
            }
            return View(risk);
        }

        

    }
}
