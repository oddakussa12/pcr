using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_CORE_MVC_EMP_MGMT.Models
{
    //this class provide implementation for interface IEmployeeRepository
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;
        public MockEmployeeRepository()
        {
            //intialize private field but these data shouldn't be hard coded instead from database
            _employeeList = new List<Employee>()
            {
                new Employee(){ EmployeeName = "Odda Kussa",EmployeeGender="Male" , EmployeeCity = "Addis Ababa"},
                
            };
        }//end ctor

        public Employee add(Employee employee)
        {
            throw new NotImplementedException();
        }//end add method

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.EmployeeId == id);
            if(employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }//end Delete method

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _employeeList;
        }//end method GetAllEmployee

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.EmployeeId == id);
        }//end method GetEmployee

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.EmployeeId == employeeChanges.EmployeeId);
            if (employee != null)
            {
                employee.EmployeeName = employeeChanges.EmployeeName;
            }
            return employee;
        }//end update method
    }//end class MockEmployeeRepository
}//end namespace
