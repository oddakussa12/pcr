using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email: ")]
        public String Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password: ")]
        public String Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
