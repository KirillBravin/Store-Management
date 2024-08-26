using Microsoft.AspNetCore.Mvc;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Store_Management.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null");
            }

            var result = await _userService.AddUser(employee);

            if (result)
            {
                return Ok("Employee added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding an employee.");
            }
        }

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("User is null");
            }

            var result = await _userService.AddUser(customer);

            if (result)
            {
                return Ok("Customer added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding a customer.");
            }
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _userService.GetAllEmployees();

            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found.");
            }

            return Ok(employees);
        }

        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _userService.GetAllCustomers();

            if (customers == null || customers.Count == 0)
            {
                return NotFound("No customers found.");
            }

            return Ok(customers);
        }

        [HttpPut("ModifyCustomer")]
        public async Task<IActionResult> ModifyCustomer(int id, [FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer is null.");
            }

            var result = await _userService.ModifyUsers(id, customer);

            if (result)
            {
                return Ok("Customer modified successfully.");
            }
            else
            {
                return StatusCode(500, "An error has occurred while modifying a customer");
            }
        }

        [HttpPut("ModifyEmployee")]
        public async Task<IActionResult> ModifyEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null.");
            }

            var result = await _userService.ModifyUsers(id, employee);

            if (result)
            {
                return Ok("Employee modified successfully.");
            }
            else
            {
                return StatusCode(500, "An error has occurred while modifying an employee");
            }
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _userService.DeleteEmployee(id);

            if (result)
            {
                return Ok("Employee deleted successfully.");
            }
            else
            {
                return NotFound("Employee not found or could not be deleted.");
            }
        }

        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _userService.DeleteCustomer(id);

            if (result)
            {
                return Ok("Customer deleted successfully.");
            }
            else
            {
                return NotFound("Customer not found or could not be deleted.");
            }
        }
    }
}
