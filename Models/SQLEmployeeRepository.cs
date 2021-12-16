using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    //this class provide implementation for IEmployeeRepository interface
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;

        //since we need to store and retrive objects from sql server we need DbContext class so 
        //we have to inject our AppDbContext class...in belwo ctor
        public SQLEmployeeRepository(AppDbContext context)//context is the name of parameter we give as a choice
        {
            this.context = context;
        }// end ctor


        public Employee add(Employee employee)
        {
            context.Employees.Add(employee);//Employees is field in AppDbContext class
            context.SaveChanges();
            return employee;
        }//end method add

        public Employee Delete(int id)
        {
            Employee employee = context.Employees.Find(id);
            if(employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }//end method Delete

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Employees;
        }//end method GetAllEmployee

        public Employee GetEmployee(int id)
        {
            return context.Employees.Find(id);
        }//end GtEmployee method

        public Employee Update(Employee employeeChanges)
        {
            var employee = context.Employees.Attach(employeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeeChanges;
        }//end Update method
    }
}
