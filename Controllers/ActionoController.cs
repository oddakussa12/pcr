using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ASP.NET_CORE_MVC_EMP_MGMT.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class ActionoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IActionoRepository _iActionoRepository;
        private static int code;//store  ProjectCode passed from ProjectSummary.cshtml
        private static string ProjectName;

        public ActionoController(AppDbContext context, IActionoRepository iActionoRepository)
        {
            _context = context;
            _iActionoRepository = iActionoRepository;
        }

        //when we want to edit an issue this method respond to get method to display a form which has predefined values
        //from the database the specific issue we are editing
        [HttpGet]
        public ViewResult EditProjectActiono(int Id)//Id is the IssueCode
        {
            Actiono actiono = _iActionoRepository.GetActiono(Id);
            EditUpdateActiono editUpdateActiono = new EditUpdateActiono
            {
                Id = actiono.ActionoCode,
                ActionoDescription = actiono.ActionoDescription,
                ActionoOwner = actiono.ActionoOwner,
                ActionoResponsible = actiono.ActionoResponsible,
                ActionoDeadline = actiono.ActionoDeadline,
                ActionnoStatus = actiono.ActionnoStatus,
                ActionoLastUpdate = actiono.ActionoLastUpdate,
                ActionoCode = actiono.ActionoCode
            };
            ViewBag.ProjectCode = code;
            return View(editUpdateActiono);
        }
        //EditProjectIssue(); is used to add new Issue to Issue Table so this method Post the form data to database
        [HttpPost]
        public IActionResult EditProjectActiono(EditUpdateActiono model)
        {
            if (ModelState.IsValid)
            {

                Actiono actiono = _iActionoRepository.GetActiono(model.Id);
                actiono.ActionoDescription = model.ActionoDescription;
                actiono.ActionoOwner = model.ActionoOwner;
                actiono.ActionoResponsible = model.ActionoResponsible;
                actiono.ActionoDeadline = model.ActionoDeadline;
                actiono.ActionnoStatus = model.ActionnoStatus;
                actiono.ActionoLastUpdate = model.ActionoLastUpdate;

                _iActionoRepository.Update(actiono);//Update method is found in IIssueRepository.cs
                //return RedirectToAction("ActionoTable");
                return RedirectToAction("ActionoTable", new { id = code });
            }
            return View();
        }


        //method to delete a project Issue no view is required to delete an issue so this method work on get method
        public async Task<IActionResult> DeleteProjectActiono(int id)
        {
            var actiono = await _context.Actionos.FindAsync(id);
            _context.Actionos.Remove(actiono);
            await _context.SaveChangesAsync();
            //return RedirectToAction("ActionoTable");
            return RedirectToAction("ActionoTable", new { id = code });
        }

        //method to display all Actiions of a project in table format
        public IActionResult ActionoTable(int id)
        {
            var Username = HttpContext.Session.GetString("Username");

            if (Username != null)
            {
                code = id;
                ViewBag.Code = code;
                var model = _iActionoRepository.GetAllProjectActiono(id);//interface in IIssueRepository
                ViewBag.Issue = model;
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
        //method to search project
        [HttpPost]
        public IActionResult ActionoTable(int id,string search = null)
        {
            id = code;
            if (!string.IsNullOrEmpty(search))//if user just press the search button without providing name to be searched
            {
                var foundActions = _iActionoRepository.SearchAction(search);
                return View(foundActions);
            }
            var allAction = _iActionoRepository.GetAllProjectActiono(id);//interface in IProjectRepository
            ViewBag.Project = allAction;

            ////get the name of the project based on the id and pass it to RiskTable.cshtml to use it
            ////to indicate where the user is at
            ProjectName = _context
            .Projects
            .Where(u => u.ProjectCode == id)
            .Select(u => u.projectName)
            .SingleOrDefault();
            ViewBag.ProjectName = ProjectName;

            //pass the global code value to IssueTable.cshtml
            ViewBag.ProjectCode = code;
            return View(allAction);

            //code = id;
            //ViewBag.Code = code;
            //var model = _iActionoRepository.GetAllProjectActiono(id);//interface in IIssueRepository
            //ViewBag.Issue = model;
            ////get the name of the project based on the id and pass it to RiskTable.cshtml to use it
            ////to indicate where the user is at
            //ProjectName = _context
            //.Projects
            //.Where(u => u.ProjectCode == id)
            //.Select(u => u.projectName)
            //.SingleOrDefault();
            //ViewBag.ProjectName = ProjectName;

            ////pass the global code value to IssueTable.cshtml
            //ViewBag.ProjectCode = code;

            //return View(model);
        }
        //AddIssue(); is used to add new Issue to Issue Table
        [HttpGet]
        public IActionResult AddActiono()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddActiono(Actiono actiono)
        {
            if (ModelState.IsValid)
            {

                Actiono act = new Actiono();
                act.ActionoDescription = actiono.ActionoDescription;
                act.ActionoOwner = actiono.ActionoOwner;
                act.ActionoResponsible = actiono.ActionoResponsible;
                act.ActionoDeadline = actiono.ActionoDeadline;
                act.ActionnoStatus = actiono.ActionnoStatus;
                act.ActionoLastUpdate = actiono.ActionoLastUpdate;
                act.ProjectProjectCode = code;

                _context.Actionos.Add(act);
                await _context.SaveChangesAsync();
                return RedirectToAction("ActionoTable", new { id = code });
            }
            return View(actiono);
        }
        // Export ActionTable to file system
        public IActionResult ExportAction()
        {
            var data = _context.Actionos.Where(ri => ri.ProjectProjectCode == code).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Action Log");

                sheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            var fileName = ProjectName + $" Action Log_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }

        //public IActionResult ExportAction()
        //{
        //    var data = _context.Actionos.Where(ri => ri.ProjectProjectCode == code).ToList();

        //    var stream = new MemoryStream();
        //    using (var package = new ExcelPackage(stream))
        //    {
        //        var sheet = package.Workbook.Worksheets.Add("Action Log");
        //        var rowCount = sheet.Dimension.Rows;
        //        for (int row = 2; row <= rowCount; row++)
        //        {

        //            list.Add(new Actiono
        //            {
        //                //ActionoCode = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
        //                ActionoDescription = worksheet.Cells[row, 1].Value.ToString().Trim(),
        //                ActionoOwner = worksheet.Cells[row, 2].Value.ToString().Trim(),
        //                ActionoResponsible = worksheet.Cells[row, 3].Value.ToString().Trim(),
        //                ActionoDeadline = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
        //                ActionnoStatus = worksheet.Cells[row, 5].Value.ToString().Trim(),
        //                ActionoLastUpdate = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
        //                ProjectProjectCode = code,

        //            });
        //        }



        //        sheet.Cells.LoadFromCollection(data, true);
        //        package.Save();
        //    }
        //    stream.Position = 0;
        //    var fileName = ProjectName + $" Action Log_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
        //    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        //}


        public async Task<DemoResponse<List<Actiono>>>Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DemoResponse<List<Actiono>>.GetResult(-1, "Empty File","Unsuccessful","Choose file to import");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                DemoResponse<List<Actiono>>.GetResult(-1, "Unsupported File Format", "Unsuccessful","Please choose .xlsx files ");
            }
            
            var list = new List<Actiono>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);
                
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    
                    for (int row = 2; row <= rowCount; row++)
                    {

                        list.Add(new Actiono
                        {
                            //ActionoCode = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            ActionoDescription = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            ActionoOwner = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            ActionoResponsible = worksheet.Cells[row, 3].Value.ToString().Trim(),
                            ActionoDeadline =DateTime.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
                            //ActionnoStatus = Enum.Parse (worksheet.Cells[row, 5].Value.ToString().Trim()),
                            ActionnoStatus = Enum.Parse<ClosedOpened>(worksheet.Cells[row, 5].Value.ToString().Trim()),
                            ActionoLastUpdate = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString().Trim()),
                            ProjectProjectCode = code,

                        });
                    }
                    
                }
                
            }
            _context.Actionos.AddRange(list);
            await _context.SaveChangesAsync();
            return DemoResponse<List<Actiono>>.GetResult(0, "OK", "Import Was Successful","");
            //return RedirectToAction("ActionoTable", new { id = code });
        }

    }
}
