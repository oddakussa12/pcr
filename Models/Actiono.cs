using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public class Actiono
    {
        [Key]
        public int ActionoCode { get; set; }

        [Required]
        [Display(Name = "Action Description: ")]
        public String ActionoDescription { get; set; }

        [Required]
        [Display(Name="Action Owner")]
        public String ActionoOwner { get; set; }

        [Required]
        [Display(Name ="Responsible")]
        public String ActionoResponsible { get; set; }

        [Required]
        [Display(Name ="Deadline")]
        [DataType(DataType.Date)]
        public DateTime ActionoDeadline { get; set; }

        [Required]
        [Display(Name ="Status")]
        public ClosedOpened ActionnoStatus { get; set; }

        [Required]
        [Display(Name = "Last Update")]
        [DataType(DataType.Date)]
        public DateTime ActionoLastUpdate { get; set; }

        //foreign key
        public int? ProjectProjectCode { get; set; }
        [ForeignKey("ProjectProjectCode")]
        public virtual Project Project { get; set; }

    }
}
