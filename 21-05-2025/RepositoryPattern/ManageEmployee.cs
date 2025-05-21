using RepositoryPattern.Helpers;
using RepositoryPattern.Models;
using RepositoryPattern.Services;
using RepositoryPattern.Repositories;
using RepositoryPattern.Interfaces;

namespace RepositoryPattern.Management
{
    public class ManageEmployee
    {
        private readonly IEmployeeService _employeeService;

        public ManageEmployee(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n=== Employee Management System ===");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Search Employee");
                Console.WriteLine("3. List All Employees");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEmployee();
                        break;

                    case "2":
                        SearchEmployee();
                        break;

                    case "3":
                        ListAllEmployees();
                        break;

                    case "4":
                        Console.WriteLine("Exiting application...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        private void AddEmployee()
        {
            var employee = new Employee();

            employee.Name = InputHelper.ReadString("Enter employee name: ");
            employee.Age = InputHelper.ReadNonNegativeInt("Enter employee age: ");
            employee.Salary = InputHelper.ReadNonNegativeDouble("Enter employee salary: ");

            int id = _employeeService.AddEmployee(employee);
            if (id != -1)
                Console.WriteLine($"Employee added successfully with ID: {id}");
            else
                Console.WriteLine("Failed to add employee.");
        }

        private void SearchEmployee()
        {
            var searchModel = new SearchModel();

            int? id = InputHelper.ReadOptionalNonNegativeInt("Enter Employee ID to search (or press Enter to skip): ");
            if (id.HasValue)
                searchModel.Id = id.Value;

            string? name = InputHelper.ReadOptionalString("Enter Name to search (or press Enter to skip): ");
            if (name != null)
                searchModel.Name = name;

            var ageRange = InputHelper.ReadOptionalIntRange("Enter Min Age (or press Enter to skip): ", "Enter Max Age (or press Enter to skip): ");
            if (ageRange != null)
                searchModel.Age = new Range<int> { MinVal = ageRange.Value.Min, MaxVal = ageRange.Value.Max };

            var salaryRange = InputHelper.ReadOptionalDoubleRange("Enter Min Salary (or press Enter to skip): ", "Enter Max Salary (or press Enter to skip): ");
            if (salaryRange != null)
                searchModel.Salary = new Range<double> { MinVal = salaryRange.Value.Min, MaxVal = salaryRange.Value.Max };

            var results = _employeeService.SearchEmployee(searchModel);

            if (results != null && results.Count > 0)
            {
                Console.WriteLine($"\nFound {results.Count} employee(s):");
                foreach (var emp in results)
                {
                    Console.WriteLine("------------------------------");
                    Console.WriteLine(emp);
                }
            }
            else
            {
                Console.WriteLine("No employees found matching the criteria.");
            }
        }

        private void ListAllEmployees()
        {
            var allEmployees = _employeeService.GetAllEmployees();

         if (_employeeService.GetAllEmployees().Count == 0) 
            {
                Console.WriteLine("No employees available.");
                return;
            }

            Console.WriteLine($"\nListing all {allEmployees.Count} employee(s):");
            foreach (var emp in allEmployees)
            {
                Console.WriteLine("------------------------------");
                Console.WriteLine(emp);
            }
        }
    }
}
