using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IProductService
    {
        Task<bool> AddProduct(Product product);
        Task<List<Product>> GetAllProducts();
        Task<bool> ModifyProduct(int id, Product product);
        Task<bool> DeleteProduct(int id);
    }
}