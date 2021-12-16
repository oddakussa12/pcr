using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class IdentityUserCopy
    {
        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
    }
}
