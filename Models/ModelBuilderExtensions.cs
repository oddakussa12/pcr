using Microsoft.EntityFrameworkCore;//since we use ModelBuilder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    //this class is useed to seed/insert data to Employees Table
    //for extensioin method this class must be static
    public static class ModelBuilderExtensions
    {
        //seed these data to Employees table N.B Employees in field/table for Employee class
        public static void Seed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    EmployeeName = "Beniyam",
                    EmployeeCity = "Pawi",
                    EmployeeGender = "Male"

                }
                //new Employee
                //{

                //    EmployeeName = "Beniyam",
                //    EmployeeCity = "Pawi",
                //    EmployeeGender = "Male"

                //}
                );
        }//end Seed method
    }//end ModelBuilderExtensions
}
