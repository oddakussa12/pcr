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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.NET_CORE_MVC_EMP_MGMT.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IScheduleRepository _iScheduleRepository;
        private static int code;//store  ProjectCode passed from ProjectSummary.cshtml
        private static string ProjectName;

        public ScheduleController(AppDbContext context, IScheduleRepository iScheduleRepository)
        {
            _context = context;
            _iScheduleRepository = iScheduleRepository;
        }
        //method to display all schedules of a project in table format
        public IActionResult ScheduleTable(int id)
        {
            var Username = HttpContext.Session.GetString("Username");

            if (Username != null)
            {
                code = id;
                ViewBag.Code = code;
                var model = _iScheduleRepository.GetAllProjectSchedule(id);//interface in IScheduleRepository
                ViewBag.Schedule = model;
                //get the name of the project based on the id and pass it to ScheduleTable.cshtml to use it
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
        //method to delete all project schedules 
        [HttpGet]
        public IActionResult ClearSchedule(int id)//id is ProjectCode
        {
            //using (DbContext db = new DbContext(ConfigHelper.Instance().ConnectionString))
            //{
                try
                {
                    foreach (var entity in _context.Schedules.Where(o => o.ProjectProjectCode == id))
                        _context.Schedules.RemoveRange(entity);

                    _context.SaveChanges();

                    //var project = _context.Schedules.SingleOrDefault(o => o.ProjectProjectCode == id);
                    //_context.Schedules.RemoveRange(project);

                    //_context.SaveChanges();
                return RedirectToAction("ScheduleTable", new { id = id });
                }
                catch (Exception e)
                {
                    //ErrorLoggingService.Log(this, e);
                return RedirectToAction("ScheduleTable", new { id = id });
                }
            //}





            //var ProjectProjectCodeSchedule = _context.Schedules.AddRange(x => x.ProjectProjectCode == id);//delete Schedules by thier foreign key
            //if (ProjectProjectCodeSchedule != null)
            //{
            //    _context.Schedules.RemoveRange(ProjectProjectCodeSchedule);
                
            //    _context.SaveChanges();
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction("ScheduleTable", new { id = id });
            
        }





        // Export ActionTable to file system
        public IActionResult ExportSchedule()
        {
            var data = _context.Schedules.Where(ri => ri.ProjectProjectCode == code).ToList();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Who does what?");

                sheet.Cells.LoadFromCollection(data, true);
                package.Save();
            }
            stream.Position = 0;
            var fileName = ProjectName + $" Schedule_{DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }


        public async Task<DemoResponse<List<Schedule>>> Import(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return DemoResponse<List<Schedule>>.GetResult(-1, "Empty File", "Unsuccessful", "Choose file to import");
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                DemoResponse<List<Actiono>>.GetResult(-1, "Unsupported File Format", "Unsuccessful", "Please choose .xlsx files ");
            }
            
            var list = new List<Schedule>();

            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {

                        list.Add(new Schedule
                        {
                            //ActionoCode = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            ScheduleTaskName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            ScheduleDuration = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            ScheduleStartDate = DateTime.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                            ScheduleDueDate = DateTime.Parse(worksheet.Cells[row, 4].Value.ToString().Trim()),
                            ScheduleResourceName = worksheet.Cells[row, 5].Value.ToString().Trim(),
                            
                            ProjectProjectCode = code,

                        });
                    }

                }

            }
            _context.Schedules.AddRange(list);
            await _context.SaveChangesAsync();
            return DemoResponse<List<Schedule>>.GetResult(0, "OK", "Import Was Successful", "");
            //return RedirectToAction("ActionoTable", new { id = code });
        }



    }
}
