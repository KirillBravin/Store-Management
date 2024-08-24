using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store_Management.CORE.Models;
using Store_Management.CORE.Contracts;

namespace Store_Management.CORE.Repositories.SQL
{
    public class ProductDBRepository : IProductDBRepository
    {
        public async Task<int> AddProduct(Product product)
        {
            using (var context = new DBContext())
            {
                bool productExists = await context.Products.AnyAsync(p =>  p.Category == product.Category && p.Name == product.Name);

                if (productExists)
                {
                    Console.WriteLine("Product with this name and category already exists.");
                    return -1;
                }

                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                Console.WriteLine("Product added successfully.");
                return product.Id;
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
            using (var context = new DBContext())
            {
                return await context.Products.ToListAsync();
            }
        }

        public async Task<bool> ModifyProduct(int id, Product product)
        {
            using (var context = new DBContext())
            {
                var existingProduct = await context.Products.FindAsync(id);

                if (existingProduct == null)
                {
                    Console.WriteLine("Product not found.");
                    return false;
                }

                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.Category = product.Category;
                existingProduct.Quantity = product.Quantity;

                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task DeleteProduct(int id)
        {
            using (var context = new DBContext())
            {
                var existingProduct = await context.Products.FindAsync(id);

                if (existingProduct == null)
                {
                    Console.WriteLine($"Product with id: {id} not found.");
                }

                context.Products.Remove(existingProduct);

                await context.SaveChangesAsync();
                Console.WriteLine("Product successfully deleted.");
            }
        }
    }
}
