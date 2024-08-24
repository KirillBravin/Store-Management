using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IEmployeeDBRepository
    {
        Task<int> AddEmployee(Employee employee);
        Task<List<Employee>> GetAllEmployees();
        Task<bool> ModifyEmployee(int id, Employee employee);
        Task<bool> DeleteEmployee(int id);
    }
}