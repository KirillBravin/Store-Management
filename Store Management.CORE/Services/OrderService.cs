using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using Serilog;
using Microsoft.Extensions.Logging;
using Castle.Core.Resource;

namespace Store_Management.CORE.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDBRepository _orderRepository;
        private readonly IMongoDBCache _mongoDBCache;
        private readonly ILogger<IOrderService> _logger;

        public OrderService (IOrderDBRepository orderRepository, IMongoDBCache mongoDBCache, ILogger<IOrderService> logger)
        {
            _orderRepository = orderRepository;
            _mongoDBCache = mongoDBCache;
            _logger = logger;
        }

        public async Task<bool> AddOrder(int userId, int productId, int quantity)
        {
            try
            {
                var order = new Order
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };

                bool result = await _orderRepository.AddOrder(order);

                if (result)
                {
                    _logger.LogInformation("Order successfully added with UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
                }
                else
                {
                    _logger.LogWarning("Failed to add order with UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while adding the order with UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
                return false;
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllOrders();

                if (orders == null || orders.Count == 0)
                {
                    _logger.LogWarning("No orders found.");
                }
                else
                {
                    _logger.LogInformation("Retrieved {OrderCount} orders successfully.", orders.Count);
                }
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while showing all orders.");
                return new List<Order>();
            }
        }

        public async Task<bool> ModifyOrder(int id, Order order)
        {
            try
            {
                bool result = false;
                result = await _orderRepository.ModifyOrder(id, order);

                if(result)
                {
                    _logger.LogInformation("Order with id: {Id} successfully modified.", id);
                }
                else
                {
                    _logger.LogWarning("Failed to modify order with id: {Id} in the repository.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while modifying order with id: {Id}.", id);
                return false;
            }
        }

        public async Task<bool> DeleteOrder(int id)
        {
            try
            {
                bool result = false;
                result = await _orderRepository.DeleteOrder(id);

                if (result)
                {
                    _logger.LogInformation("Order with id: {Id} successfully deleted.", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete order with id: {Id}.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while trying to delete an order with id: {Id}.", id);
                return false;
            }
        }
    }
}
