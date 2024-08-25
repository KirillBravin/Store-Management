using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Store_Management.CORE.Services
{
    public class UserService : IUserService
    {
        private readonly ICustomerDBRepository _customerDBRepository;
        private readonly IEmployeeDBRepository _employeeDBRepository;
        private readonly IMongoDBCache _mongoDBCache;
        private readonly ILogger<UserService> _logger;

        public UserService(ICustomerDBRepository customerDBRepository, IEmployeeDBRepository employeeDBRepository, IMongoDBCache mongoDBCache, ILogger<UserService> logger)
        {
            _customerDBRepository = customerDBRepository;
            _employeeDBRepository = employeeDBRepository;
            _mongoDBCache = mongoDBCache;
            _logger = logger;
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                if (user is Customer customer)
                {
                    var customerId = await _customerDBRepository.AddCustomer(customer);
                    if (customerId > 0)
                    {
                        customer.Id = customerId;
                        await _mongoDBCache.AddCustomer(customer);
                        _logger.LogInformation("Customer added successfully with ID: {CustomerId}", customerId);
                        return true;
                    }
                }
                else if (user is Employee employee)
                {
                    var employeeId = await _employeeDBRepository.AddEmployee(employee);
                    if (employeeId > 0)
                    {
                        employee.Id = employeeId;
                        await _mongoDBCache.AddEmployee(employee);
                        _logger.LogInformation("Employee added successfully with ID: {EmployeeId}", employeeId);
                        return true;
                    }
                }
                else
                {
                    _logger.LogWarning("Unsupported user type: {UserType}", user.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding user.");
            }
            return false;
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                var employees = await _employeeDBRepository.GetAllEmployees();

                if (employees == null || employees.Count == 0)
                {
                    _logger.LogWarning("No employees found.");
                }
                else
                {
                    _logger.LogInformation("Retrieved {EmployeeCount} employees successfully.", employees.Count);
                }
                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while showing all employees.");
                return null;
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            try
            {
                var customers = await _customerDBRepository.GetAllCustomers();

                if (customers == null || customers.Count == 0)
                {
                    _logger.LogWarning("No customers found.");
                }
                else
                {
                    _logger.LogInformation("Retrieved {CustomersCount} customers successfully.", customers.Count);
                }
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while showing all customers.");
                return null;
            }
        }

        public async Task<bool> ModifyUsers(int id, User user)
        {
            try
            {
                bool result = false;

                if (user is Customer customer)
                {
                    result = await _customerDBRepository.ModifyCustomer(id, customer);

                    if (result)
                    {
                        _logger.LogInformation("Customer with ID {CustomerId} successfully modified in the repository.", customer.Id);
                        try
                        {
                            await _mongoDBCache.ModifyCustomer(id, customer);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while updating the cache for customer with ID {CustomerId}.", customer.Id);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to modify customer with ID {CustomerId} in the repository.", customer.Id);
                    }
                }
                else if (user is Employee employee)
                {
                    result = await _employeeDBRepository.ModifyEmployee(id, employee);

                    if (result)
                    {
                        _logger.LogInformation("Employee with ID {EmployeeId} successfully modified in the repository.", employee.Id);
                        try
                        {
                            await _mongoDBCache.ModifyEmployee(id, employee);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while updating the cache for employee with ID {EmployeeId}.", employee.Id);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Failed to modify employee with ID {EmployeeId} in the repository.", employee.Id);
                    }
                }
                else
                {
                    _logger.LogWarning("Unsupported user type: {UserType}", user.GetType().Name);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while modifying the user with ID {UserId}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                bool result = false;
                result = await _customerDBRepository.DeleteCustomer(id);

                if (result)
                {
                    _logger.LogInformation("Customer with id: {Id} successfully deleted.", id);
                    try
                    {
                        await _mongoDBCache.DeleteCustomer(id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while deleting the cache for customer with id: {Id}.", id);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to delete customer with id: {Id}. Customer may not exist.", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while trying to delete a customer with id: {Id}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            try
            {
                bool result = false;
                result = await _employeeDBRepository.DeleteEmployee(id);

                if (result)
                {
                    _logger.LogInformation("Employee with id: {Id} successfully deleted.", id);
                    try
                    {
                        await _mongoDBCache.DeleteEmployee(id);
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while deleting the cache for employee with ID {Id}.", id);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to delete employee with id: {Id}. Employee may not exist.", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while trying to delete an employee with id: {Id}.", id);
                return false;
            }
        }
    }
}
