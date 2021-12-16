using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IActionoRepository
    {
        IEnumerable<Actiono> GetAllProjectActiono(int id);//show all project action in table format
        Actiono GetActiono(int Id);//to edit projectrisk 
        Actiono Update(Actiono actionoChanges);
        List<Actiono> SearchAction(string search);
        //Actiono Delete(int Id);
        //Actiono Add(Actiono actiono);
    }
}
