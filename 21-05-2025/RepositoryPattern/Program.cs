using RepositoryPattern.Management;
using RepositoryPattern.Interfaces;
using RepositoryPattern.Services;
using RepositoryPattern.Models;
using RepositoryPattern.Repositories;

class Program
{
    static void Main(string[] args)
    {
        IRepositor<int, Employee> employeeRepository = new EmployeeRepository();
        IEmployeeService employeeService = new EmployeeService(employeeRepository);
        var manager = new ManageEmployee(employeeService);
        manager.Run();
    }
}
