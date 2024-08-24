using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IUserService
    {
        Task<bool> AddUser(User user);
        Task<List<Employee>> GetAllEmployees();
        Task<List<Customer>> GetAllCustomers();
        Task<bool> ModifyUsers(int id, User user);
        Task<bool> DeleteCustomer(int id);
        Task<bool> DeleteEmployee(int id);
    }
}