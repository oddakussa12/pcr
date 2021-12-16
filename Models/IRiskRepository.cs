using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IRisk
    {
        IEnumerable<Risk> GetAllProjectRisk(int id);//show all project risk in table format
        Risk GetRisk(int Id);//to edit projectrisk 
        Risk Update(Risk riskChanges);
        List<Risk> SearchRisk(string search);
        //Risk Delete(int Id);
        //Risk Add(Risk risk);

    }
}
