using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Store_Management.CORE.Enums;

namespace Store_Management.CORE.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ProductCategory Category { get; set; }
        public int Quantity { get; set; }

        public Product (string name, double price, ProductCategory category, int quantity)
        {
            Name = name;
            Price = price;
            Category = category;
            Quantity = quantity;
        }

        public Product() { }
    }
}
