using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store_Management.CORE.Models
{
    public class Customer : User
    {
        public int Id { get; set; }

        public Customer(string firstName, string lastName, string email, string phoneNumber) : base(firstName, lastName, email, phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Customer() { }
    }
}
