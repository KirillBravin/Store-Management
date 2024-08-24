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
    public class OrderDBRepository : IOrderDBRepository
    {
        public async Task AddOrder(int customerId, int productId, int quantity)
        {
            using (var context = new DBContext())
            {
                DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
                Order newOrder = new Order(customerId, productId, currentDate, quantity);

                await context.Orders.AddAsync(newOrder);
                await context.SaveChangesAsync();
                Console.WriteLine("Order successfully created.");
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            using (var context = new DBContext())
            {
                return await context.Orders.ToListAsync();
            }
        }

        public async Task<bool> ModifyOrder(int id, Order order)
        {
            using (var context = new DBContext())
            {
                var existingOrder = await context.Orders.FindAsync(id);

                if (existingOrder == null)
                {
                    Console.WriteLine("Order not found.");
                    return false;
                }

                existingOrder.UserId = order.UserId;
                existingOrder.ProductId = order.ProductId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.Date = order.Date;

                await context.SaveChangesAsync();

                Console.WriteLine("Order updated successfully.");
                return true;
            }
        }

        public async Task DeleteOrder(int id)
        {
            using (var context = new DBContext())
            {
                var existingOrder = await context.Orders.FindAsync(id);

                if (existingOrder == null)
                {
                    Console.WriteLine($"Order with id: {id} not found.");
                    return;
                }

                context.Orders.Remove(existingOrder);
                await context.SaveChangesAsync();
                Console.WriteLine("Order successfully deleted.");
            }
        }
    }
}
