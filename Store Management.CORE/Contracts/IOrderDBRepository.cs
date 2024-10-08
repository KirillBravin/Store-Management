﻿using Store_Management.CORE.Models;

namespace Store_Management.CORE.Contracts
{
    public interface IOrderDBRepository
    {
        Task<bool> AddOrder(Order order);
        Task<List<Order>> GetAllOrders();
        Task<bool> ModifyOrder(int id, Order order);
        Task<bool> DeleteOrder(int id);
    }
}