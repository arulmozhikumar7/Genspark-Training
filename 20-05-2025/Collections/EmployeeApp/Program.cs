using System;
using System.Collections.Generic;

namespace EmployeeApp
{
    class Program
    {
        // list of employees in the system
        static List<Employee> employees = new List<Employee>();
        static Dictionary<int, Employee> employeeDict = new Dictionary<int, Employee>();
        static void Main(string[] args)
        { 
            EmployeePromotion promotion = new EmployeePromotion();

            while (true)
            {
                Console.WriteLine("\nSelect Task:");
                Console.WriteLine(" 1 - Add New Employee to System");
                Console.WriteLine(" 2 - Add Employee to Promotion List");
                Console.WriteLine(" 3 - Find Promotion Position by Name");
                Console.WriteLine(" 4 - Display Current List Capacity");
                Console.WriteLine(" 5 - Trim Excess Capacity");
                Console.WriteLine(" 6 - Display Sorted Promoted Employee List");
                Console.WriteLine(" 7 - Display  Employee List");
                Console.WriteLine(" 8 - Sort Employees");
                Console.WriteLine(" 9 - Find Employee by Id");
                Console.WriteLine("10 - Find Employees by Name");
                Console.WriteLine("11 - Find Employees Older Than Given");
                Console.WriteLine("12 - Update Employee by Id");
                Console.WriteLine("13 - Delete Employee by Id");
                Console.WriteLine("14 - Exit");

                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewEmployee();
                        break;

                    case "2":
                        AddEmployeeToPromotionList(promotion);
                        break;

                    case "3":
                        FindPosition(promotion);
                        break;

                    case "4":
                        Console.WriteLine($"Current Capacity: {promotion.GetCapacity()}");
                        Console.WriteLine($"Number of Employees in Promotion List: {promotion.Count()}");
                        break;

                    case "5":
                        Console.WriteLine($"Capacity before trimming: {promotion.GetCapacity()}");
                        promotion.TrimList();
                        Console.WriteLine($"Capacity after trimming: {promotion.GetCapacity()}");
                        break;

                    case "6":
                        DisplaySortedList(promotion);
                        break;

                    case "7":
                        DisplayAllEmployees();
                        break;

                    case "8":
                        SortEmployeesMenu();
                        break;

                    case "9":
                        DisplayEmployeeById();
                        break;

                    case "10":
                        FindEmployeesByName();
                        break;

                    case "11":
                        FindEmployeesOlderThanGiven();
                        break;

                    case "12":
                        UpdateEmployeeById();
                        break;

                    case "13":
                        DeleteEmployeeById();
                        break;

                    case "14":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please select again.");
                        break;
                }
            }
        }

      static void AddNewEmployee()
    {
    Console.Write("How many employees do you want to add? ");
    if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
    {
        Console.WriteLine("Invalid number. Returning to main menu.");
        return;
    }

    int added = 0;

    while (added < count)
    {
        Console.WriteLine($"\nAdding employee {added + 1} of {count}:");

        Employee emp = new Employee();

        // Get unique Employee ID first
        while (true)
        {
            Console.Write("Enter Employee ID (integer): ");
            string? inputId = Console.ReadLine();

            if (!int.TryParse(inputId, out int id))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer ID.");
                continue;
            }

            if (employees.Exists(e => e.Id == id))
            {
                Console.WriteLine($"Employee with ID \"{id}\" already exists. Please enter a different ID.");
            }
            else
            {
                emp.Id = id;
                break;
            }
        }

        // Get Employee Name (non-empty)
        while (true)
        {
            Console.Write("Enter Employee Name: ");
            string? inputName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(inputName))
            {
                Console.WriteLine("Name cannot be empty. Please enter a valid name.");
                continue;
            }
            emp.Name = inputName.Trim();
            break;
        }

        // Get Age (positive integer)
        while (true)
        {
            Console.Write("Enter Employee Age: ");
            string? inputAge = Console.ReadLine();
            if (!int.TryParse(inputAge, out int age) || age <= 0)
            {
                Console.WriteLine("Invalid age. Please enter a positive integer.");
                continue;
            }
            emp.Age = age;
            break;
        }

        // Get Salary (non-negative double)
        while (true)
        {
            Console.Write("Enter Employee Salary: ");
            string? inputSalary = Console.ReadLine();
            if (!double.TryParse(inputSalary, out double salary) || salary < 0)
            {
                Console.WriteLine("Invalid salary. Please enter a non-negative number.");
                continue;
            }
            emp.Salary = salary;
            break;
        }

        // Add to list after all validations
        employees.Add(emp);

        // Add to dictionary after adding to the list
        employeeDict[emp.Id] = emp;

        Console.WriteLine($"Employee \"{emp.Name}\" added successfully!");

        added++;
    }
}
 
     static void UpdateEmployeeById()
        {
            int id;
            while (true)
            {
                Console.Write("Enter the Employee ID to update: ");
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out id) || id <= 0)
                {
                    Console.WriteLine("Invalid ID. Please enter a positive integer.");
                    continue;
                }
                break;
            }

            if (!employeeDict.ContainsKey(id))
            {
                Console.WriteLine($"No employee found with ID {id}.");
                return;
            }

            Employee emp = employeeDict[id];
            Console.WriteLine($"\nCurrent details of Employee ID {id}:");
            Console.WriteLine($"Name: {emp.Name}, Age: {emp.Age}, Salary: {emp.Salary}");

            // Update Name
            while (true)
            {
                Console.Write("Enter new name (leave blank to keep unchanged): ");
                string? newName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newName))
                    break;
                emp.Name = newName.Trim();
                break;
            }

            // Update Age
            while (true)
            {
                Console.Write("Enter new age (leave blank to keep unchanged): ");
                string? newAgeInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newAgeInput))
                    break;

                if (!int.TryParse(newAgeInput, out int newAge) || newAge <= 0)
                {
                    Console.WriteLine("Invalid age. Please enter a positive integer.");
                    continue;
                }
                emp.Age = newAge;
                break;
            }

            // Update Salary
            while (true)
            {
                Console.Write("Enter new salary (leave blank to keep unchanged): ");
                string? newSalaryInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(newSalaryInput))
                    break;

                if (!double.TryParse(newSalaryInput, out double newSalary) || newSalary < 0)
                {
                    Console.WriteLine("Invalid salary. Please enter a non-negative number.");
                    continue;
                }
                emp.Salary = newSalary;
                break;
            }

            Console.WriteLine("Employee details updated successfully.");
        }

    static void DeleteEmployeeById()
    {
        int id;
        while (true)
        {
            Console.Write("Enter the Employee ID to delete: ");
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out id) || id <= 0)
            {
                Console.WriteLine("Invalid ID. Please enter a positive integer.");
                continue;
            }
            break;
        }

        if (!employeeDict.ContainsKey(id))
        {
            Console.WriteLine($"No employee found with ID {id}.");
            return;
        }

        // Remove from dictionary
        Employee emp = employeeDict[id];
        employeeDict.Remove(id);

        // Remove from list
        employees.Remove(emp);

        Console.WriteLine($"Employee with ID {id} ({emp.Name}) has been successfully deleted.");
    }

    static void FindEmployeesOlderThanGiven()
        {
            int id;
            while (true)
            {
                Console.Write("Enter Employee ID to compare age: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out id) || id <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer ID.");
                    continue;
                }
                break;
            }

            if (!employeeDict.TryGetValue(id, out Employee? givenEmp))
            {
                Console.WriteLine($"Employee with ID {id} not found.");
                return;
            }

            var olderEmployees = employees.Where(e => e.Age > givenEmp.Age).ToList();

            if (olderEmployees.Count == 0)
            {
                Console.WriteLine($"No employees are older than {givenEmp.Name} (Age: {givenEmp.Age}).");
                return;
            }

            Console.WriteLine($"\nEmployees older than {givenEmp.Name} (Age: {givenEmp.Age}):");

            foreach (var emp in olderEmployees)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine($"ID: {emp.Id}");
                Console.WriteLine($"Name: {emp.Name}");
                Console.WriteLine($"Age: {emp.Age}");
                Console.WriteLine($"Salary: {emp.Salary}");
            }
        }

     static void DisplayAllEmployees()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees available.");
                return;
            }
            Console.WriteLine("\nAll Employees:");
            Console.WriteLine($"{"ID"} {"Name"} {"Age"} {"Salary"}");
            Console.WriteLine(new string('-', 40));
            foreach (var emp in employees)
            {
                Console.WriteLine(emp);
            }
        }

        static void DisplayEmployeeById()
        {
            int id;
            while (true)
            {
                Console.Write("Enter Employee ID to search (positive integer): ");
                string? input = Console.ReadLine();

                if (int.TryParse(input, out id) && id > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid positive integer ID.");
                }
            }

            if (employeeDict.TryGetValue(id, out Employee? emp))
            {
                Console.WriteLine("\nEmployee Details:");
                Console.WriteLine($"ID: {emp.Id}");
                Console.WriteLine($"Name: {emp.Name}");
                Console.WriteLine($"Age: {emp.Age}");
                Console.WriteLine($"Salary: {emp.Salary}");
            }
            else
            {
                Console.WriteLine($"Employee with ID {id} not found.");
            }
    // LINQ
    //     Employee? emp = employees.FirstOrDefault(e => e.Id == id);

    // if (emp != null)
    // {
    //     Console.WriteLine("Employee found:");
    //     Console.WriteLine($"ID: {emp.Id}, Name: {emp.Name}, Age: {emp.Age}, Salary: {emp.Salary}");
    // }
    // else
    // {
    //     Console.WriteLine($"No employee found with ID {id}.");
    // }
    }

    static void FindEmployeesByName()
    {
    Console.Write("Enter employee name to search: ");
    string? inputName = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(inputName))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    string searchName = inputName.Trim().ToLower();

    // Find all matching employees by name (case-insensitive)
    var matchedEmployees = employees
        .Where(e => e.Name.ToLower().Contains(searchName))
        .ToList();

    if (matchedEmployees.Count == 0)
    {
        Console.WriteLine($"No employees found matching \"{inputName}\".");
        return;
    }

    Console.WriteLine($"\nFound {matchedEmployees.Count} employee(s) matching \"{inputName}\":");

    foreach (var emp in matchedEmployees)
    {
        Console.WriteLine("------------------------------");
        Console.WriteLine($"ID: {emp.Id}");
        Console.WriteLine($"Name: {emp.Name}");
        Console.WriteLine($"Age: {emp.Age}");
        Console.WriteLine($"Salary: {emp.Salary}");
    }
}

    static void SortEmployeesMenu()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees to sort.");
                return;
            }

            Console.WriteLine("\nSort Employees By:");
            Console.WriteLine("1. Name");
            Console.WriteLine("2. Salary");
            Console.WriteLine("3. Age");
            Console.WriteLine("4. ID");
            int choice = Helper.ReadInt("Enter choice: ");

            switch (choice)
            {
                case 1:
                    employees = employees.OrderBy(e => e.Name).ToList();
                    Console.WriteLine("Employees sorted by Name.");
                    break;
                case 2:
                    employees = employees.OrderByDescending(e => e.Salary).ToList();
                    Console.WriteLine("Employees sorted by Salary (descending).");
                    break;
                case 3:
                    employees = employees.OrderBy(e => e.Age).ToList();
                    Console.WriteLine("Employees sorted by Age.");
                    break;
                case 4:
                    employees = employees.OrderBy(e => e.Id).ToList();
                    Console.WriteLine("Employees sorted by ID.");
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    return;
            }
            DisplayAllEmployees();
        }
        static void AddEmployeeToPromotionList(EmployeePromotion promotion)
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees exist in the system. Please add employees first.");
                return;
            }

            Console.WriteLine("Enter employee names to add to promotion list (blank to stop):");

            while (true)
            {
                string? name = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(name))
                    break;


                Employee? emp = employees.Find(e => e.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (emp == null)
                {
                    Console.WriteLine($"Employee \"{name}\" does not exist in the system. Please add them first.");
                }
                else
                {
                    promotion.AddToPromotionList(emp.Name);
                    Console.WriteLine($"Employee \"{emp.Name}\" added to promotion list.");
                }
            }
        }

        static void FindPosition(EmployeePromotion promotion)
        {
            string? name = Helper.ReadNonEmptyString("Please enter the name of the employee to check promotion position");
            int position = promotion.GetPromotionPosition(name!);
            if (position == -1)
            {
                Console.WriteLine($"Employee '{name}' is not in the promotion list.");
            }
            else
            {
                Console.WriteLine($"\"{name}\" is at position {position} for promotion.");
            }
        }

        static void DisplaySortedList(EmployeePromotion promotion)
        {
            var sortedList = promotion.GetSortedPromotionList();
            Console.WriteLine("Promoted employee list:");
            foreach (var emp in sortedList)
            {
                Console.WriteLine(emp);
            }
        }
    }
}
