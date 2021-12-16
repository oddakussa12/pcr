using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class SQLRiskRepository : IRisk
    {
        private readonly AppDbContext context;

        public SQLRiskRepository(AppDbContext context)
        {
            this.context = context;
        }
        //this Method/GetAllProjectRisk is used for RiskTable.cshtml to display all riks of a project
        public IEnumerable<Risk> GetAllProjectRisk(int id)
        {
            //return context.Risks.Where(ri=>ri.Project.ProjectCode==id).ToList();
            //context.Risks.AsEnumerable();
            return context.Risks.Where(ri => ri.ProjectProjectCode == id).ToList();
        }
        //this method/GetRisk is used for EditUpdateRisk.cshtml to find a risk which will be updated
        public Risk GetRisk(int id)
        {
            return context.Risks.Find(id);
        }//end GtEmployee method

        public Risk Update(Risk riskChanges)
        {
            var risk = context.Risks.Attach(riskChanges);
            risk.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return riskChanges;
        }
        //search project by thier ProjectName or by its ProjectCode
        public List<Risk> SearchRisk(string search)
        {
            int RiskCode;
            bool successParse = Int32.TryParse(search, out RiskCode);//casting search to string projectCode
            if (!successParse)
                RiskCode = 0;
            return context.Risks.Where(p => p.RiskDescription.Contains(search) ||
                                             
                                             p.RiskCode == RiskCode).ToList();
        }

        



        //public Risk Delete(int id)
        //{
        //    Risk risk = context.Risks.Find(id);
        //    if (risk != null)
        //    {
        //        context.Risks.Remove(risk);
        //        context.SaveChanges();
        //    }
        //    return risk;
        //}//end
    }
}
