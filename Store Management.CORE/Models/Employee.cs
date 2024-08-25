using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Store_Management.CORE.Models
{
    public class Employee : User
    {
        [Key]
        public int Id { get; set; }

        public Employee(string firstName, string lastName, string email, string phoneNumber) : base(firstName, lastName, email, phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public Employee() { }
    }
}
