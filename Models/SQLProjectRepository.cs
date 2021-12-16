using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class SQLProjectRepository:IProjectRepository
    {
        private readonly AppDbContext context;

        public SQLProjectRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Project> GetAllProject()
        {
            return context.Projects;
        }

        public Project GetProject(int Id)
        {
            return context.Projects.Find(Id);
            
        }
        //search project by thier ProjectName or by its ProjectCode
        public List<Project> SearchProject(string search)
        {
            int projectCode;
            bool successParse = Int32.TryParse(search, out projectCode);//casting search to string projectCode
            if (!successParse)
                projectCode = 0;
            return context.Projects.Where(p => p.projectName.Contains(search) ||
                                             p.ProjectCode == projectCode).ToList();
        }

        public Project Update(Project projectChanges)
        {
            var project = context.Projects.Attach(projectChanges);
            project.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return projectChanges;
        }
    }
}
