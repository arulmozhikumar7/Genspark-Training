using System;

namespace EmployeeApp
{
    public class Employee : IComparable<Employee>
    {
        private int id, age;
        private string name = string.Empty;
        private double salary;

        public Employee() { }

        public Employee(int id, int age, string name, double salary)
        {
            this.id = id;
            this.age = age;
            this.name = name ?? string.Empty;
            this.salary = salary;
        }

        public void TakeEmployeeDetailsFromUser()
        {
            Console.WriteLine("Please enter the employee ID:");
            Id = Convert.ToInt32(Console.ReadLine() ?? "0");

            Console.WriteLine("Please enter the employee name:");
            Name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Please enter the employee age:");
            Age = Convert.ToInt32(Console.ReadLine() ?? "0");

            Console.WriteLine("Please enter the employee salary:");
            Salary = Convert.ToDouble(Console.ReadLine() ?? "0");
        }

        public override string ToString()
        {
            return $"Employee ID : {id}\nName : {name}\nAge : {age}\nSalary : {salary}";
        }

        // Enables sorting by Salary
        public int CompareTo(Employee? other)
        {
            if (other == null) return 1;
            return this.salary.CompareTo(other.salary);
        }

        // Enables comparison in Dictionary & HashSet
        public override bool Equals(object? obj)
        {
            return obj is Employee other && this.id == other.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        // Properties
        public int Id { get => id; set => id = value; }
        public int Age { get => age; set => age = value; }
        public string Name { get => name; set => name = value ?? string.Empty; }
        public double Salary { get => salary; set => salary = value; }
    }
}
