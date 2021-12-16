using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleCode { get; set; }

        [Required]
        [Display(Name = "Task Name: ")]
        public string ScheduleTaskName { get; set; }


        [Required]
        [Display(Name = "Duration(in days):")]
        public string ScheduleDuration { get; set; }

        [Required]
        [Display(Name = "Start Date:")]
        [DataType(DataType.Date)]
        public DateTime ScheduleStartDate { get; set; }

        [Required]
        [Display(Name = "Due Date:")]
        [DataType(DataType.Date)]
        public DateTime ScheduleDueDate { get; set; }

        [Required]
        [Display(Name = "Resource Name:")]
        public string ScheduleResourceName { get; set; }

        //foreign key
        public int? ProjectProjectCode { get; set; }
        [ForeignKey("ProjectProjectCode")]
        public virtual Project Project { get; set; }
    }
}
