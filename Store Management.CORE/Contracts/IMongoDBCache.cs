using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IMongoDBCache
    {
        Task AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomers();
        Task<bool> ModifyCustomer(int id, Customer customer);
        Task<bool> DeleteCustomer(int id);
        Task AddEmployee(Employee employee);
        Task<List<Employee>> GetAllEmployees();
        Task<bool> ModifyEmployee(int id, Employee employee);
        Task<bool> DeleteEmployee(int id);
        Task AddProduct(Product product);
        Task<List<Product>> GetAllProducts();
        Task<bool> ModifyProduct(int id, Product product);
        Task<bool> DeleteProduct(int id);
        Task AddOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task<bool> ModifyOrder(int id, Order order);
        Task<bool> DeleteOrder(int id);
    }
}