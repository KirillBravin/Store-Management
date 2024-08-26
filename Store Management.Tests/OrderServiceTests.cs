using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using Store_Management.CORE.Services;
using Microsoft.Extensions.Logging;

namespace Store_Management.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderDBRepository> _mockOrderRepo;
        private readonly Mock<IMongoDBCache> _mockCache;
        private readonly Mock<ICustomerDBRepository> _mockCustomerRepo;
        private readonly Mock<IEmployeeDBRepository> _mockEmployeeRepo;
        private readonly Mock<IProductDBRepository> _mockProductRepo;
        private readonly Mock<ILogger<IOrderService>> _mockLogger;
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _mockOrderRepo = new Mock<IOrderDBRepository>();
            _mockCache = new Mock<IMongoDBCache>();
            _mockCustomerRepo = new Mock<ICustomerDBRepository>();
            _mockEmployeeRepo = new Mock<IEmployeeDBRepository>();
            _mockProductRepo = new Mock<IProductDBRepository>();
            _mockLogger = new Mock<ILogger<IOrderService>>();

            _orderService = new OrderService(
                _mockOrderRepo.Object,
                _mockCache.Object,
                _mockLogger.Object,
                _mockCustomerRepo.Object,
                _mockEmployeeRepo.Object,
                _mockProductRepo.Object
            );
        }

        [Fact]
        public async Task TestAddSuccessfulOrder()
        {
            // Arrange
            int userId = 1;
            int productId = 2;
            int quantity = 3;
            var order = new Order
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                Date = DateOnly.FromDateTime(DateTime.Now)
            };

            _mockCustomerRepo.Setup(cr => cr.GetCustomerById(userId)).ReturnsAsync(new Customer());
            _mockEmployeeRepo.Setup(er => er.GetEmployeeById(userId)).ReturnsAsync((Employee)null);
            _mockProductRepo.Setup(pr => pr.GetProductById(productId)).ReturnsAsync(new Product());
            _mockOrderRepo.Setup(repo => repo.AddOrder(It.Is<Order>(o => o.UserId == userId && o.ProductId == productId && o.Quantity == quantity && o.Date == order.Date))).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.AddOrder(It.Is<Order>(o => o.UserId == userId && o.ProductId == productId && o.Quantity == quantity && o.Date == order.Date))).Returns(Task.CompletedTask);

            // Act
            var result = await _orderService.AddOrder(userId, productId, quantity);

            // Assert
            Assert.True(result);
            _mockOrderRepo.Verify(repo => repo.AddOrder(It.Is<Order>(o => o.UserId == userId && o.ProductId == productId && o.Quantity == quantity && o.Date == order.Date)), Times.Once);
            _mockCache.Verify(cache => cache.AddOrder(It.Is<Order>(o => o.UserId == userId && o.ProductId == productId && o.Quantity == quantity && o.Date == order.Date)), Times.Once);
        }

        // Tests if all orders are found
        [Fact]
        public async Task TestGetAllOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { UserId = 1, ProductId = 2, Quantity = 3, Date = DateOnly.FromDateTime(DateTime.Now) },
                new Order { UserId = 4, ProductId = 5, Quantity = 6, Date = DateOnly.FromDateTime(DateTime.Now) }
            };

            _mockOrderRepo.Setup(repo => repo.GetAllOrders()).ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllOrders();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, o => o.UserId == 1 && o.ProductId == 2);
            Assert.Contains(result, o => o.UserId == 4 && o.ProductId == 5);
        }

        // Test if list of orders return an empty list, if there are no orders found
        [Fact]
        public async Task TestGetEmptyListOfOrders()
        {
            // Arrange
            _mockOrderRepo.Setup(repo => repo.GetAllOrders()).ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderService.GetAllOrders();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestModifyOrderSuccessfull()
        {
            // Arrange
            int id = 1;
            var order = new Order { UserId = 1, ProductId = 2, Quantity = 3 };
            _mockCustomerRepo.Setup(repo => repo.GetCustomerById(order.UserId)).ReturnsAsync(new Customer());
            _mockEmployeeRepo.Setup(repo => repo.GetEmployeeById(order.UserId)).ReturnsAsync((Employee)null);
            _mockProductRepo.Setup(repo => repo.GetProductById(order.ProductId)).ReturnsAsync(new Product());
            _mockOrderRepo.Setup(repo => repo.ModifyOrder(id, order)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.ModifyOrder(id, order)).Returns(Task.FromResult(true));

            // Act
            var result = await _orderService.ModifyOrder(id, order);

            // Assert
            Assert.True(result);
            _mockOrderRepo.Verify(repo => repo.ModifyOrder(id, order), Times.Once);
            _mockCache.Verify(cache => cache.ModifyOrder(id, order), Times.Once);
        }

        [Fact]
        public async Task TestDeleteOrderSuccessful()
        {
            // Arrange
            int id = 1;
            _mockOrderRepo.Setup(repo => repo.DeleteOrder(id)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.DeleteOrder(id)).Returns(Task.FromResult(true));

            // Act
            var result = await _orderService.DeleteOrder(id);

            // Assert
            Assert.True(result);
            _mockOrderRepo.Verify(repo => repo.DeleteOrder(id), Times.Once);
            _mockCache.Verify(cache => cache.DeleteOrder(id), Times.Once);
        }
    }
}
