using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Store_Management.CORE.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductDBRepository _productRepository;
        private readonly IMongoDBCache _mongoDBCache;
        private readonly ILogger<IProductService> _logger;

        public ProductService (IProductDBRepository dbRepository, IMongoDBCache cache, ILogger<IProductService> logger)
        {
            _productRepository = dbRepository;
            _mongoDBCache = cache;
            _logger = logger;
        }

        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                var productId = await _productRepository.AddProduct(product);
                if (productId > 0)
                {
                    product.Id = productId;
                    await _mongoDBCache.AddProduct(product);
                    _logger.LogInformation("Product successfully added with id: {ProductId}", productId);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Failed to add product. The repository returned an invalid product ID.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding product.");
            }
            return false;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProducts();

                if (products == null || products.Count == 0)
                {
                    _logger.LogWarning("No product found.");
                }
                else
                {
                    _logger.LogInformation("Retrieved {ProductCount} products successfully.", products.Count);
                }
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while showing all products.");
                return null;
            }
        }

        public async Task<bool> ModifyProduct(int id, Product product)
        {
            try
            {
                bool result = false;
                result = await _productRepository.ModifyProduct(id, product);

                if (result)
                {
                    _logger.LogInformation("Product with Id: {ProductId} successfully modified in the repository.", product.Id);
                    try
                    {
                        await _mongoDBCache.ModifyProduct(id, product);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred while updating the cache for product with id {ProductId}.", product.Id);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to modify product with id: {ProductId} in the repository.", product.Id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while modifying the user with ID {ProductId}");
                return false;
            }
        }

        public async Task<bool> DeleteProduct(int id)
        {
            try
            {
                bool result = false;
                result = await _productRepository.DeleteProduct(id);

                if(result)
                {
                    _logger.LogInformation("Product with id: {Id} successfully deleted.", id);
                    try
                    {
                        await _mongoDBCache.DeleteProduct(id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error has occurred while deleting the cache with id: {Id}.", id);
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to delete product with id: {Id}. Product may not exist.", id);
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error has occurred while trying to delete a product with id: {Id}.", id);
                return false;
            }
        }
    }
}
