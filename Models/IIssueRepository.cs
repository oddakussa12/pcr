using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IIssueRepository
    {
        IEnumerable<Issue> GetAllProjectIssue(int id);//show all project Issue in table format
        Issue GetIssue(int Id);//to edit project Issue
        Issue Update(Issue issueChanges);
        List<Issue> SearchIssue(string search);
        //Risk Delete(int Id);
        //Risk Add(Risk risk);
    }
}
