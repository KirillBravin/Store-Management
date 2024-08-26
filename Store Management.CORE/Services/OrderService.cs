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
        private readonly ICustomerDBRepository _customerRepository;
        private readonly IEmployeeDBRepository _employeeRepository;
        private readonly IProductDBRepository _productRepository;
        private readonly IMongoDBCache _mongoDBCache;
        private readonly ILogger<IOrderService> _logger;

        public OrderService (IOrderDBRepository orderRepository, IMongoDBCache mongoDBCache, ILogger<IOrderService> logger, ICustomerDBRepository customerRepository, IEmployeeDBRepository employeeRepository, IProductDBRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mongoDBCache = mongoDBCache;
            _logger = logger;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _productRepository = productRepository;
        }

        public async Task<bool> AddOrder(int userId, int productId, int quantity)
        {
            if (!await UserExists(userId))
            {
                _logger.LogWarning("User with id: {UserId} does not exist", userId);
                return false;
            }

            if (!await ProductExists(productId))
            {
                _logger.LogWarning("Product with id: {ProductId} does not exist.", productId);
                return false;
            }

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
                    try
                    {
                        await _mongoDBCache.AddOrder(order);
                        _logger.LogInformation("Order successfully added with UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while adding the order to the MongoDB cache with UserId: {UserId}, ProductId: {ProductId}, Quantity: {Quantity}", userId, productId, quantity);
                    }
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
                bool userExists = await UserExists(order.UserId);
                if (!userExists)
                {
                    _logger.LogWarning("Modification failed. User with id: {UserId} does not exist.", order.UserId);
                    return false;
                }

                var product = await _productRepository.GetProductById(order.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Modification failed. Product with id: {ProductId} does not exist.", order.ProductId);
                    return false;
                }

                bool result = false;
                result = await _orderRepository.ModifyOrder(id, order);

                if(result)
                {
                    try
                    {
                        await _mongoDBCache.ModifyOrder(id, order);
                        _logger.LogInformation("Order with id: {Id} successfully modified.", id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred while updating the cache for order with id: {OrderId}.", order.Id);
                    }
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
                    try
                    {
                        await _mongoDBCache.DeleteOrder(id);
                        _logger.LogInformation("Order with id: {Id} successfully deleted.", id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred while deleting the cache with id: {Id}.", id);
                    }
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

        public async Task<bool> UserExists(int userId)
        {
            var customerExists = await _customerRepository.GetCustomerById(userId) != null;
            var employeeExists = await _employeeRepository.GetEmployeeById(userId) != null;
            return customerExists || employeeExists;
        }

        public async Task<bool> ProductExists(int productId)
        {
            return await _productRepository.GetProductById(productId) != null;
        }
    }
}
