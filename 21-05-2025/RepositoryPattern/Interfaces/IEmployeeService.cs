using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryPattern.Models;

namespace RepositoryPattern.Interfaces
{
    public interface IEmployeeService
    {
        int AddEmployee(Employee employee);
        List<Employee>? SearchEmployee(SearchModel searchModel);
         List<Employee> GetAllEmployees();
    }
}