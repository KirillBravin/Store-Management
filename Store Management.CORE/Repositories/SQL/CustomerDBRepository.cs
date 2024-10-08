﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store_Management.CORE.Models;
using Store_Management.CORE.Contracts;

namespace Store_Management.CORE.Repositories.SQL
{
    public class CustomerDBRepository : ICustomerDBRepository
    {
        public async Task<int> AddCustomer(Customer customer)
        {
            using (var context = new DBContext())
            {
                // Checking if customer with this first name and last name exists
                bool customerExists = await context.Customers.AnyAsync(c => c.FirstName == customer.FirstName && c.LastName == customer.LastName);

                if (customerExists)
                {
                    Console.WriteLine("Customer with this first name and last name already exists.");
                    return -1;
                }

                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                Console.WriteLine("Customer added successfully.");
                return customer.Id;
            }
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            using (var context = new DBContext())
            {
                return await context.Customers.ToListAsync();
            }
        }

        public async Task<bool> ModifyCustomer(int id, Customer customer)
        {
            using (var context = new DBContext())
            {
                var existingCustomer = await context.Customers.FindAsync(id);

                if (existingCustomer == null)
                {
                    Console.WriteLine("Customer not found");
                    return false;
                }

                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                existingCustomer.PhoneNumber = customer.PhoneNumber;

                await context.SaveChangesAsync();

                Console.WriteLine("Customer updated successfully.");
                return true;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            using (var context = new DBContext())
            {
                var existingCustomer = await context.Customers.FindAsync(id);

                if (existingCustomer == null)
                {
                    Console.WriteLine($"Customer with id: {id} not found.");
                    return false;
                }

                context.Customers.Remove(existingCustomer);

                await context.SaveChangesAsync();
                Console.WriteLine("Customer successfully deleted.");
                return true;
            }
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            try
            {
                using (var context = new DBContext())
                {
                    var customer = await context.Customers.FindAsync(id);

                    if (customer == null)
                    {
                        Console.WriteLine($"Warning: Customer with id: {id} not found.");
                    }

                    return customer;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving customer with id: {id}", ex);
                throw;
            }
        }
    }
}