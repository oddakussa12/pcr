using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }//the first column in Employees table
        [Required]
        [MaxLength(50, ErrorMessage = "Name can't exced 50 characters")]
        public String EmployeeName { get; set; }// the second column in Employees table
        [Required]
        public String EmployeeGender { get; set; }
        [Required]
        public String EmployeeCity { get; set; }
        
    }
}
