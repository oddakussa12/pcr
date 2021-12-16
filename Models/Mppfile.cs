using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    [NotMapped]
    public class Mppfile
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Under { get; set; }
        public string KR { get; set; }
    }
}
