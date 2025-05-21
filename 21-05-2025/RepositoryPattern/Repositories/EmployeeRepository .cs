using RepositoryPattern.Models;
using RepositoryPattern.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace RepositoryPattern.Repositories
{
    public class EmployeeRepository : Repository<int, Employee>
    {
        public override ICollection<Employee> GetAll()
        {
            if (_items.Count == 0)
                throw new KeyNotFoundException("No employees found");
            return _items.ToList();
        }

        public override Employee GetById(int id)
        {
            var employee = _items.FirstOrDefault(e => e.Id == id);
    if (employee == null)
        throw new KeyNotFoundException("Employee not found with ID " + id);
    return employee;
        }

       protected override int GenerateID()
        {
            return _items.Count == 0 ? 101 : _items.Max(e => e.Id) + 1;
        }

    }
}
