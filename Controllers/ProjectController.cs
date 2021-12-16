using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProjectRepository _iprojectRepository;
        //this controller request IprojectRepository interface
        public ProjectController(AppDbContext context, IProjectRepository iprojectRepository)
        {
            _context = context;
            _iprojectRepository = iprojectRepository;
        }
        






        //when we want to edit a risk this Get method display a form with predefined values in its form
        [HttpGet]
        public ViewResult EditUpdateProject(int Id)//Id is the RiskCode of the risk we are editing passed from RiskTable.cshtml
        {
            Project project = _iprojectRepository.GetProject(Id);
            ViewBag.Users = _context.Users.Select(u => u.UserName).ToList();
            EditUpdateProject editUpdateProject = new EditUpdateProject
            {

                Id = project.ProjectCode,
                projectName = project.projectName,
                //ProjectPM_Name = project.ProjectPM_Name,
                //ProjectPM_Name = ViewBag.Users,
                ClientName = project.ClientName,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Progress = project.Progress,
                ProjectCode = project.ProjectCode,
               
            };
            return View(editUpdateProject);
        }
        //EditProjectRisk(); is used to add new Risk to Risk Table so this method Post the form data to database
        [HttpPost]
        public IActionResult EditUpdateProject(EditUpdateProject model)
        {
            if (ModelState.IsValid)
            {

                Project project = _iprojectRepository.GetProject(model.Id);
                project.projectName = model.projectName;
                project.ProjectPM_Name = model.ProjectPM_Name;
                project.ClientName = model.ClientName;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.Progress = model.Progress;
                _iprojectRepository.Update(project);//Update method is found in IRiskRepository.cs
                return RedirectToAction("ProjectSummary");
            }
            return View();
        }


        //method to delete a project risk no view is required to delete a risk so this method work on get method
        [HttpGet]
        public async Task<IActionResult> DeleteProject(int id)//id is ProjectCode
        {
            var project = await _context.Projects.FindAsync(id);//id is ProjectCode passed from ProjectSummary
            _context.Projects.Remove(project);
            //deleting a project also deletes its associated log files
            //RiskCode(primary key) IN Projects table is the same with ProjectProjectCode(foriegn key) in Risks table
            var ProjectProjectCodeRisk = _context.Risks.SingleOrDefault(x => x.ProjectProjectCode == id);//delete risks by their foreign key
            var ProjectProjectCodeIssue = _context.Issues.SingleOrDefault(x => x.ProjectProjectCode == id);//delete issues by thir foreign key
            var ProjectProjectCodeAction = _context.Actionos.SingleOrDefault(x => x.ProjectProjectCode == id);//delete actions by thier foreign key
            if (ProjectProjectCodeRisk != null || ProjectProjectCodeIssue != null
                || ProjectProjectCodeAction != null)
            {
                _context.Risks.Remove(ProjectProjectCodeRisk);
                _context.Issues.Remove(ProjectProjectCodeIssue);
                _context.Actionos.Remove(ProjectProjectCodeAction);
                _context.SaveChanges();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ProjectSummary");
        }
        //method to display all projects in table form
        [HttpGet]
        public ViewResult ProjectSummary()
        {
            var model = _iprojectRepository.GetAllProject();//interface in IProjectRepository
            ViewBag.Project = model;
            return View(model);
        }
        [HttpPost]//when we search a project
        public IActionResult ProjectSummary(string search = null)
        {
            if (!string.IsNullOrEmpty(search))//if user just press the search button without providing name to be searched
            {
                var foundProject = _iprojectRepository.SearchProject(search);
                return View(foundProject);
            }
            var allProject = _iprojectRepository.GetAllProject();//interface in IProjectRepository
            ViewBag.Project = allProject;
            return View(allProject);
        }
        private IEnumerable<SelectListItem> GetAllUserId()
        {

            return _context.UserClaims.ToList().Select(users => new SelectListItem
            {
                Text = users.UserId.ToString(),
                Value = users.UserId.ToString()
            });
        }
        //public IEnumerable<IdentityUserCopy> DisplayPMs { get; set; }

        [HttpGet]
        public IActionResult CreateNewProject()
        {
            
            ViewBag.Users = _context.Users.Select(u => u.UserName).ToList();
            return View();
        }
        //method to insert Project detail to db
        [HttpPost]
        public async Task<IActionResult>CreateNewProject(Project project)
        {
            if (ModelState.IsValid)
            {
              
                Project pro = new Project
                {
                    projectName = project.projectName,
                    ProjectPM_Name = project.ProjectPM_Name,
                    ClientName = project.ClientName,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Progress = project.Progress
                };
               
                _context.Projects.Add(pro);
                await _context.SaveChangesAsync();
                return RedirectToAction("ProjectSummary");
            }
            
            return View(project);
        }
        

    }
}
