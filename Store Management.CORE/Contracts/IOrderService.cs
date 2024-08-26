using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IOrderService
    {
        Task<bool> AddOrder(int userId, int productId, int quantity);
        Task<List<Order>> GetAllOrders();
        Task<bool> ModifyOrder(int id, Order order);
        Task<bool> DeleteOrder(int id);
        Task<bool> UserExists(int userId);
        Task<bool> ProductExists(int productId);
    }
}