using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store_Management.CORE.Models;
using Store_Management.CORE.Contracts;
using MongoDB.Driver;
using Castle.Core.Resource;

namespace Store_Management.CORE.Repositories.MongoDb
{
    public class MongoDBCache : IMongoDBCache
    {
        private readonly IMongoCollection<Customer> _customerCache;
        private readonly IMongoCollection<Employee> _employeeCache;
        private readonly IMongoCollection<Order> _orderCache;
        private readonly IMongoCollection<Product> _productCache;

        public MongoDBCache (IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("StoreManagement");

            _customerCache = database.GetCollection<Customer>("customers_cache");
            _employeeCache = database.GetCollection<Employee>("employees_cache");
            _orderCache = database.GetCollection<Order>("orders_cache");
            _productCache = database.GetCollection<Product>("products_cache");
        }

        // Customers
        public async Task AddCustomer(Customer customer)
        {
            await _customerCache.InsertOneAsync(customer);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _customerCache.Find(_ => true).ToListAsync();
        }

        public async Task<bool> ModifyCustomer(int id, Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);

            var update = Builders<Customer>.Update
                .Set(x => x.FirstName, customer.FirstName)
                .Set(x => x.LastName, customer.LastName)
                .Set(x => x.Email, customer.Email)
                .Set(x => x.PhoneNumber, customer.PhoneNumber);

            var result = await _customerCache.UpdateOneAsync(filter, update);

            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var result = await _customerCache.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        // Employee

        public async Task AddEmployee(Employee employee)
        {
            await _employeeCache.InsertOneAsync(employee);
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _employeeCache.Find(_ => true).ToListAsync();
        }

        public async Task<bool> ModifyEmployee(int id, Employee employee)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);

            var update = Builders<Employee>.Update
                .Set(x => x.FirstName, employee.FirstName)
                .Set(x => x.LastName, employee.LastName)
                .Set(x => x.Email, employee.Email)
                .Set(x => x.PhoneNumber, employee.PhoneNumber);

            var result = await _employeeCache.UpdateOneAsync(filter, update);

            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteEmployee(int id)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);
            var result = await _employeeCache.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        // Product

        public async Task AddProduct(Product product)
        {
            await _productCache.InsertOneAsync(product);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _productCache.Find(_ => true).ToListAsync();
        }

        public async Task<bool> ModifyProduct(int id, Product product)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);

            var update = Builders<Product>.Update
                .Set(x => x.Name, product.Name)
                .Set(x => x.Price, product.Price)
                .Set(x => x.Quantity, product.Quantity)
                .Set(x => x.Category, product.Category);

            var result = await _productCache.UpdateOneAsync(filter, update);

            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var result = await _productCache.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }

        // Orders

        public async Task AddOrder(Order order)
        {
            await _orderCache.InsertOneAsync(order);
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _orderCache.Find(_ => true).ToListAsync();
        }

        public async Task<bool> ModifyOrder(int id, Order order)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);

            var update = Builders<Order>.Update
                .Set(x => x.UserId, order.UserId)
                .Set(x => x.ProductId, order.ProductId)
                .Set(x => x.Quantity, order.Quantity)
                .Set(x => x.Date, order.Date);

            var result = await _orderCache.UpdateOneAsync(filter, update);

            return result.MatchedCount > 0 && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var result = await _orderCache.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}
