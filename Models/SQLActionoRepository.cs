using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class SQLActionoRepository:IActionoRepository
    {
        private readonly AppDbContext context;

        public SQLActionoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Actiono GetActiono(int Id)
        {
            return context.Actionos.Find(Id);
        }

        public IEnumerable<Actiono> GetAllProjectActiono(int id)
        {
            //return context.Actionos;
            return context.Actionos.Where(ri => ri.ProjectProjectCode == id).ToList();
        }

        public Actiono Update(Actiono actionoChanges)
        {
            var actiono = context.Actionos.Attach(actionoChanges);
            actiono.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return actionoChanges;
        }
        //search project by thier ProjectName or by its ProjectCode
        public List<Actiono> SearchAction(string search)
        {
            int ActionCode;
            bool successParse = Int32.TryParse(search, out ActionCode);//casting search to string projectCode
            if (!successParse)
                ActionCode = 0;
            return context.Actionos.Where(p => p.ActionoDescription.Contains(search) ||
                                             
                                             p.ActionoCode == ActionCode).ToList();
        }

       
    }
}
