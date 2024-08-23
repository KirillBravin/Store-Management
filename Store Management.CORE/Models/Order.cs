using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store_Management.CORE.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public DateOnly Date { get; set; }
        public int Quantity { get; set; }

        public Order(int userId, int productId, DateOnly date, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            Date = date;
            Quantity = quantity;
        }

        public Order() { }
    }
}
