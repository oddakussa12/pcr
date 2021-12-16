using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Risk
    {
        [Key]
        public int RiskCode { get; set; }
        [Required]
        [Display(Name = "Risk Description: ")]
        public String RiskDescription { get; set; }// the second column in Employees table
        [Required]
        [Display(Name = "Risk Owner: ")]
        public String RiskOwner { get; set; }
        [Required]
        [Display(Name = "Risk Open Date: ")]
        [DataType(DataType.Date)]
        public DateTime RiskOpenDate { get; set; }
        [Required]
        [Display(Name = "Risk Close Date: ")]
        [DataType(DataType.Date)]
        public DateTime RiskCloseDate { get; set; }
        [Required]
        [Display(Name = "Risk Status: ")]
        public ClosedOpened RiskStatus { get; set; }
        [Required]
        [Display(Name = "Risk Impact: ")]
        public LowMediumHigh RiskImpact { get; set; }
        [Required]
        [Display(Name = "Agreed Solution: ")]
        public String RiskAgreedSolution { get; set; }

        //foreign key
        public int? ProjectProjectCode { get; set; }
        [ForeignKey("ProjectProjectCode")]
        public virtual Project Project { get; set; }
        
    }
}
