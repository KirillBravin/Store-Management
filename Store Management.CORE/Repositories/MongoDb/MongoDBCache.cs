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

        public async Task ModifyCustomer(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, customer.Id);
            var result = await _customerCache.ReplaceOneAsync(filter, customer);

            if (result.MatchedCount == 0)
            {
                throw new Exception($"Customer not found with id: {customer.Id}");
            }
        }

        public async Task DeleteCustomer(int id)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Id, id);
            var result = await _customerCache.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception($"Customer not found with id: {id}");
            }
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

        public async Task ModifyEmployee(Employee employee)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, employee.Id);
            var result = await _employeeCache.ReplaceOneAsync(filter, employee);

            if (result.MatchedCount == 0)
            {
                throw new Exception($"Employee not found with id: {employee.Id}");
            }
        }

        public async Task DeleteEmployee(int id)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);
            var result = await _employeeCache.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception($"Employee not found with id: {id}");
            }
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

        public async Task ModifyProduct(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
            var result = await _productCache.ReplaceOneAsync(filter, product);

            if (result.MatchedCount == 0)
            {
                throw new Exception($"Product not found with id: {product.Id}");
            }
        }

        public async Task DeleteProduct(int id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var result = await _productCache.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception($"Product not found with id: {id}");
            }
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

        public async Task ModifyOrder(Order order)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
            var result = await _orderCache.ReplaceOneAsync(filter, order);

            if (result.MatchedCount == 0)
            {
                throw new Exception($"Order not found with id: {order.Id}");
            }
        }

        public async Task DeleteOrder(int id)
        {
            var filter = Builders<Order>.Filter.Eq(x => x.Id, id);
            var result = await _orderCache.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception($"Order not found with id: {id}");
            }
        }
    }
}
