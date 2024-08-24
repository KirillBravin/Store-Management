using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IMongoDBCache
    {
        Task AddCustomer(Customer customer);
        Task<List<Customer>> GetAllCustomers();
        Task ModifyCustomer(Customer customer);
        Task DeleteCustomer(int id);
        Task AddEmployee(Employee employee);
        Task<List<Employee>> GetAllEmployees();
        Task ModifyEmployee(Employee employee);
        Task DeleteEmployee(int id);
        Task AddProduct(Product product);
        Task<List<Product>> GetAllProducts();
        Task ModifyProduct(Product product);
        Task DeleteProduct(int id);
        Task AddOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task ModifyOrder(Order order);
        Task DeleteOrder(int id);
    }
}