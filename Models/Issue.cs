using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Issue
    {
        [Key]
        public int IssueCode { get; set; }
        [Required]
        [Display(Name = "Issue Description: ")]
        public String IssueDescription { get; set; }// the second column in Employees table
        [Required]
        [Display(Name = "Issue Owner: ")]
        public String IssueOwner { get; set; }
        [Required]
        [Display(Name = "Issue Open Date: ")]
        [DataType(DataType.Date)]
        public DateTime IssueOpenDate { get; set; }
        [Required]
        [Display(Name = "Issue Close Date: ")]
        [DataType(DataType.Date)]
        public DateTime IssueCloseDate { get; set; }
        [Required]
        [Display(Name = "Issue Status: ")]
        public ClosedOpened IssueStatus { get; set; }
        
        [Display(Name = "Issue Impact: ")]
        [Required]
        public LowMediumHigh IssueImpact { get; set; }
        [Required]
        [Display(Name = "Agreed Solution: ")]
        public String IssueAgreedSolution { get; set; }

        //foreign key
        public int? ProjectProjectCode { get; set; }
        [ForeignKey("ProjectProjectCode")]
        public virtual Project Project { get; set; }
    }
}
