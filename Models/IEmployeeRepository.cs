using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);//show single project
        IEnumerable<Employee> GetAllEmployee();
        Employee add(Employee employee);//add project
        Employee Update(Employee employeeChanges);//to edit project details
        Employee Delete(int Id);//delete a project by its id


    }
}
