using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface ICustomerDBRepository
    {
        Task AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomers();
        Task<bool> ModifyCustomer(int id, Customer customer);
        Task DeleteCustomer(int id);
    }
}