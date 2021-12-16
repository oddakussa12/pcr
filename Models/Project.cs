using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Project
    {
        [Key]
        public int ProjectCode { get; set; }
        [Required]
        [Display(Name = "Project Name: ")]
        public String projectName { get; set; }// the second column in Employees table
        [Required]
        [Display(Name = "PM Name: ")]
        public String ProjectPM_Name { get; set; }
        [Required]
        [Display(Name = "Client Name: ")]
        public String ClientName { get; set; }
        [Required]
        [Display(Name = "Start Date: ")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Due Date: ")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Progress: ")]
        public int Progress { get; set; }

        public virtual ICollection<Risk> Risks { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
        public virtual ICollection<Actiono> Actionos { get; set; }
        //public IEnumerable<IdentityUser> AspNetUsers { get; set; }
        //public IEnumerable<SelectListItem> AspNetUsers { get; set; }


    }
}
