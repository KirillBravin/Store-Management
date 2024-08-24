using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IEmployeeDBRepository
    {
        Task AddEmployee(Employee employee);
        Task<List<Employee>> GetAllEmployees();
        Task<bool> ModifyEmployee(int id, Employee employee);
        Task DeleteEmployee(int id);
    }
}