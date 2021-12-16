using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//this class is the same with Risk class
//when we want to update a given risk we want the data for the risk to be displayed on EditUpdateRisk.cshtml
//so EditUpdateRisk.cshtml use this class
//to not duplicate code meke this class inherit from Risk class 

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class EditUpdateRisk:Risk
    {
        //this field is used to store the RiskCode of a risk which we are editing/updating
        public int Id { get; set; }  

    }
}
