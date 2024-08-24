using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IOrderDBRepository
    {
        Task AddOrder(int customerId, int productId, int quantity);
        Task<List<Order>> GetAllOrders();
        Task<bool> ModifyOrder(int id, Order order);
        Task DeleteOrder(int id);
    }
}