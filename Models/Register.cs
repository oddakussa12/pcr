using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
//THIS CLASS IS USED TO REGISTER NEW PROJECT MANAGER
namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Register
    {
        [Required]
        [EmailAddress]
        [Key]
        public String Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Confirm password")]
        [Compare("Password",
            ErrorMessage ="Password do not match.")]
        public String ConfirmPassword { get; set; }
    }
}
