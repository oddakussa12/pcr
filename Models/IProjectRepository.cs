using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetAllProject();//show all project details
        Project GetProject(int Id);//to edit project
        Project Update(Project projectChanges);
        List<Project> SearchProject(string search);




        //Risk Delete(int Id);
        //Risk Add(Risk risk);
    }
}
