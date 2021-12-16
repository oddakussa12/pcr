using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class IssueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IIssueRepository _iIssueRepository;
        private static string ProjectName;
        private static int code;//store  ProjectCode passed from ProjectSummary.cshtml
        public IssueController(AppDbContext context, IIssueRepository iIssueRepository)
        {
            _context = context;
            _iIssueRepository = iIssueRepository;
        }
        //when we want to edit an issue this method respond to get method to display a form which has predefined values
        //from the database the specific issue we are editing
        [HttpGet]
        public ViewResult EditProjectIssue(int Id)//Id is the IssueCode
        {
            Issue issue = _iIssueRepository.GetIssue(Id);
            EditUpdateIssue editUpdateIssue = new EditUpdateIssue
            {
                Id = issue.IssueCode,
                IssueDescription = issue.IssueDescription,
                IssueImpact = issue.IssueImpact,
                IssueOpenDate = issue.IssueOpenDate,
                IssueCloseDate = issue.IssueCloseDate,
                IssueOwner = issue.IssueOwner,
                IssueStatus = issue.IssueStatus,
                IssueAgreedSolution = issue.IssueAgreedSolution,
                IssueCode = issue.IssueCode
            };
            ViewBag.ProjectCode = code;
            return View(editUpdateIssue);
        }
        //EditProjectIssue(); is used to add new Issue to Issue Table so this method Post the form data to database
        [HttpPost]
        public IActionResult EditProjectIssue(EditUpdateIssue model)
        {
            if (ModelState.IsValid)
            {

                Issue issue = _iIssueRepository.GetIssue(model.Id);
                issue.IssueDescription = model.IssueDescription;
                issue.IssueOwner = model.IssueOwner;
                issue.IssueImpact = model.IssueImpact;
                issue.IssueOpenDate = model.IssueOpenDate;
                issue.IssueCloseDate = model.IssueCloseDate;
                issue.IssueStatus = model.IssueStatus;
                issue.IssueAgreedSolution = model.IssueAgreedSolution;

                _iIssueRepository.Update(issue);//Update method is found in IIssueRepository.cs
                //return RedirectToAction("IssueTable");
                return RedirectToAction("IssueTable", new { id = code });
            }
            return View();
        }


        //method to delete a project Issue no view is required to delete an issue so this method work on get method
        public async Task<IActionResult> DeleteProjectIssue(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            //return RedirectToAction("IssueTable");
            return RedirectToAction("IssueTable", new { id = code });
        }

        //method to display all Issue of a project in table format
        public IActionResult IssueTable(int id)//id is ProjectCode passed from RiskTable.cshtml
        {
            var Username = HttpContext.Session.GetString("Username");

            if (Username != null)
            {
                code = id;
                ViewBag.Code = code;
                var model = _iIssueRepository.GetAllProjectIssue(id);//interface in IIssueRepository
                ViewBag.Issue = model;
                //get the name of the project based on the id and pass it to IssueTable.cshtml to use it
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
        public IActionResult IssueTable(int id, string search = null)
        {
            id = code;
            if (!string.IsNullOrEmpty(search))
            {
                var foundIssues = _iIssueRepository.SearchIssue(search);
                return View(foundIssues);
            }
            var allIssue = _iIssueRepository.GetAllProjectIssue(id);
            ViewBag.Project = allIssue;

            //pass the global code value to IssueTable.cshtml
            ViewBag.ProjectCode = code;
            return View(allIssue);
        }
        //AddIssue(); is used to add new Issue to Issue Table
        [HttpGet]
        public IActionResult AddIssue()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddIssue(Issue issue)
        {
            if (ModelState.IsValid)
            {

                Issue iss = new Issue();
                iss.IssueDescription = issue.IssueDescription;
                iss.IssueImpact = issue.IssueImpact;
                iss.IssueOwner = issue.IssueOwner;
                iss.IssueOpenDate = issue.IssueOpenDate;
                iss.IssueCloseDate = issue.IssueCloseDate;
                iss.IssueStatus = issue.IssueStatus;
                iss.IssueAgreedSolution = issue.IssueAgreedSolution;
                iss.ProjectProjectCode = code;

                _context.Issues.Add(iss);
                await _context.SaveChangesAsync();
                return RedirectToAction("IssueTable", new { id = code });
            }
            return View(issue);
        }
        // Export IssueTable 
        public IActionResult ExportIssue()
        {
            //var data = _context.Risks.ToList();
            var data = _context.Issues.Where(ri => ri.ProjectProjectCode == code).ToList();
            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Issue Log");
                sheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            var fileName = ProjectName + $" Issue Log_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

    }
}
