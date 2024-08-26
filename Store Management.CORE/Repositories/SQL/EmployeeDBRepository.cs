using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store_Management.CORE.Models;
using Store_Management.CORE.Contracts;


namespace Store_Management.CORE.Repositories.SQL
{
    public class EmployeeDBRepository : IEmployeeDBRepository
    {
        public async Task<int> AddEmployee(Employee employee)
        {
            using (var context = new DBContext())
            {
                // Checking if employee with this first name and last name exists
                bool employeeExists = await context.Employees.AnyAsync(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName);

                if (employeeExists)
                {
                    Console.WriteLine("Employee with this first name and last name already exists.");
                    return -1;
                }

                await context.Employees.AddAsync(employee);
                await context.SaveChangesAsync();
                Console.WriteLine("Employee added successfully.");
                return employee.Id;
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            using (var context = new DBContext())
            {
                return await context.Employees.ToListAsync();
            }
        }

        public async Task<bool> ModifyEmployee(int id, Employee employee)
        {
            using (var context = new DBContext())
            {
                var existingEmployee = await context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    Console.WriteLine("Employee not found");
                    return false;
                }

                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Email = employee.Email;
                existingEmployee.PhoneNumber = employee.PhoneNumber;

                await context.SaveChangesAsync();

                Console.WriteLine("Employee updated successfully.");
                return true;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            using (var context = new DBContext())
            {
                var existingEmployee = await context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    Console.WriteLine($"Employee with id: {id} not found.");
                    return false;
                }

                context.Employees.Remove(existingEmployee);

                await context.SaveChangesAsync();
                Console.WriteLine("Employee successfully deleted.");
                return true;
            }
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            try
            {
                using (var context = new DBContext())
                {
                    var employee = await context.Employees.FindAsync(id);
                    
                    if(employee == null)
                    {
                        Console.WriteLine($"Warning: Employee with id: {id} not found.");
                    }

                    return employee;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving employee with id: {id}", ex);
                throw;
            }
        }
    }
}
