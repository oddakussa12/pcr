using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class SQLIssueRepository : IIssueRepository
    {
        private readonly AppDbContext context;

        public SQLIssueRepository(AppDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Issue> GetAllProjectIssue(int id)
        {
            //return context.Issues;
            return context.Issues.Where(ri => ri.ProjectProjectCode == id).ToList();
        }

        public Issue GetIssue(int Id)
        {
            return context.Issues.Find(Id);
        }

        public Issue Update(Issue issueChanges)
        {
            var issue = context.Issues.Attach(issueChanges);
            issue.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return issueChanges;
        }
        public List<Issue> SearchIssue(string search)
        {
            int IssueCode;
            bool successParse = Int32.TryParse(search, out IssueCode);//casting search to string projectCode
            if (!successParse)
                IssueCode = 0;
            return context.Issues.Where(p => p.IssueDescription.Contains(search) ||
                                             
                                             p.IssueCode == IssueCode).ToList();
        }
        //public Issue Delete(int id)
        //{
        //    Issue issue = context.Issues.Find(id);
        //    if (issue != null)
        //    {
        //        context.Issues.Remove(issue);
        //        context.SaveChanges();
        //    }
        //    return issue;
        //}//end
    }
}
