using Moq;
using Xunit;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Store_Management.CORE.Models;
using Store_Management.CORE.Enums;

namespace Store_Management.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductDBRepository> _mockProductRepo;
        private readonly Mock<IMongoDBCache> _mockCache;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepo = new Mock<IProductDBRepository>();
            _mockCache = new Mock<IMongoDBCache>();
            _mockLogger = new Mock<ILogger<ProductService>>();

            _productService = new ProductService(
                _mockProductRepo.Object,
                _mockCache.Object,
                _mockLogger.Object
                );
        }

        [Fact]
        public async Task TestAddingProductSuccessfully()
        {
            // Arrange
            var product = new Product("Test product", 10.0, ProductCategory.Flute, 5);
            _mockProductRepo.Setup(repo => repo.AddProduct(product)).ReturnsAsync(1);

            // Act
            var result = await _productService.AddProduct(product);

            // Assert
            Assert.True(result);
            _mockCache.Verify(cache => cache.AddProduct(product), Times.Once);
        }

        // Test for invalid product id
        [Fact]
        public async Task TestInvalidProductId()
        {
            // Arrange
            var product = new Product("Test product", 10.0, ProductCategory.Flute, 5);
            _mockProductRepo.Setup(repo => repo.AddProduct(product)).ReturnsAsync(0);

            // Act
            var result = await _productService.AddProduct(product);

            // Assert
            Assert.False(result);
            _mockCache.Verify(cache => cache.AddProduct(It.IsAny<Product>()), Times.Never);
        }

        // Test for thrown exception to ensure cache isnt updated
        [Fact]
        public async Task TestAddProductException()
        {
            // Arrange
            var product = new Product("Test product", 10.0, ProductCategory.Flute, 5);
            _mockProductRepo.Setup(repo => repo.AddProduct(product)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _productService.AddProduct(product);

            // Assert
            Assert.False(result);
            _mockCache.Verify(cache => cache.AddProduct(It.IsAny<Product>()), Times.Never);
        }

        // Tests if products are successfully returned
        [Fact]
        public async Task TestGetAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product("Guitar_1", 10.0, ProductCategory.Guitar, 5),
                new Product("Flute-Finita", 13.5, ProductCategory.Flute, 28)
            };

            _mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Guitar_1", result[0].Name);
            Assert.Equal("Flute-Finita", result[1].Name);
        }

        // Tests if no products exist
        [Fact]
        public async Task TestNoProductExists()
        {
            // Arrange
            var products = new List<Product>();
            _mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Tests for handling exception if getting all products
        [Fact]
        public async Task TestGetAllProductsException()
        {
            // Arrange
            _mockProductRepo.Setup(repo => repo.GetAllProducts()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.Null(result);
        }

        // Tests to successfully modify product
        [Fact]
        public async Task TestModifyingProduct()
        {
            // Arrange
            var product = new Product("Guitar_1", 10.0, ProductCategory.Guitar, 5);
            _mockProductRepo.Setup(repo => repo.ModifyProduct(It.IsAny<int>(), It.IsAny<Product>())).ReturnsAsync(true);

            // Act
            var result = await _productService.ModifyProduct(1, product);

            // Assert
            Assert.True(result);
            _mockCache.Verify(cache => cache.ModifyProduct(1, product), Times.Once);
        }

        // Tests to successfully delete product
        [Fact]
        public async Task TestDeleteProduct()
        {
            // Arrange
            int productId = 1;
            _mockProductRepo.Setup(repo => repo.DeleteProduct(productId)).ReturnsAsync(true);

            // Act
            var result = await _productService.DeleteProduct(productId);

            // Assert
            Assert.True(result);
            _mockCache.Verify(cache => cache.DeleteProduct(productId), Times.Once);
        }
    }
}
