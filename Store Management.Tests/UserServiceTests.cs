using Moq;
using Xunit;
using Store_Management.CORE;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Store_Management.CORE.Models;

namespace Store_Management.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<ICustomerDBRepository> _mockCustomerRepo;
        private readonly Mock<IEmployeeDBRepository> _mockEmployeeRepo;
        private readonly Mock<IMongoDBCache> _mockCache;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockCustomerRepo = new Mock<ICustomerDBRepository>();
            _mockEmployeeRepo = new Mock<IEmployeeDBRepository>();
            _mockCache = new Mock<IMongoDBCache>();
            _mockLogger = new Mock<ILogger<UserService>>();

            _userService = new UserService(
                _mockCustomerRepo.Object,
                _mockEmployeeRepo.Object,
                _mockCache.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task TestAddingCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "7483920332"
            };
            _mockCustomerRepo.Setup(repo => repo.AddCustomer(It.IsAny<Customer>())).ReturnsAsync(1);

            // Act
            var result = await _userService.AddUser(customer);

            // Assert
            Assert.True(result);
            _mockCustomerRepo.Verify(repo => repo.AddCustomer(It.Is<Customer>(c => c == customer)), Times.Once);
            _mockCache.Verify(cache => cache.AddCustomer(It.Is<Customer>(c => c == customer)), Times.Once);
        }

        [Fact]
        public async Task TestAddingEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "7483920332"
            };
            _mockEmployeeRepo.Setup(repo => repo.AddEmployee(It.IsAny<Employee>())).ReturnsAsync(1);

            // Act
            var result = await _userService.AddUser(employee);

            // Assert
            Assert.True(result);
            _mockEmployeeRepo.Verify(repo => repo.AddEmployee(It.Is<Employee>(e => e== employee)), Times.Once);
            _mockCache.Verify(cache => cache.AddEmployee(It.Is<Employee>(e => e == employee)), Times.Once);
        }

        // Returns list of employees
        [Fact]
        public async Task TestGettingAllEmployees_ShouldReturnListOfEmployees()
        {
            //Arrange
            var employees = new List<Employee>
            { 
                new Employee {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "7483920332"
                    }
            };
            _mockEmployeeRepo.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(employees);

            // Act
            var result = await _userService.GetAllEmployees();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        // Testing if there are no employees and it should return empty list
        [Fact]
        public async Task TestGettingAllEmployees_EmptyList()
        {
            // Arrange
            _mockEmployeeRepo.Setup(repo => repo.GetAllEmployees()).ReturnsAsync(new List<Employee>());

            // Act
            var result = await _userService.GetAllEmployees();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Test if there is an exception while retrieving employees
        [Fact]
        public async Task TestGettingEmployeesException()
        {
            // Arrange
            _mockEmployeeRepo.Setup(repo => repo.GetAllEmployees()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userService.GetAllEmployees();

            // Assert
            Assert.Null(result);
        }

        // Returns list of customers
        [Fact]
        public async Task TestGettingAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "7483920332"
                }
            };

            _mockCustomerRepo.Setup(repo => repo.GetAllCustomers()).ReturnsAsync(customers);

            // Act
            var result = await _userService.GetAllCustomers();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
        }

        // Tests if no customers are found, return empty list
        [Fact]
        public async Task TestGettingAllCustomersEmptyList()
        {
            // Arrange
            _mockCustomerRepo.Setup(repo => repo.GetAllCustomers()).ReturnsAsync(new List<Customer>());

            // Act
            var result = await _userService.GetAllCustomers();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        // Test if there is an exception while retrieving customers
        [Fact]
        public async Task TestGettingCustomersException()
        {
            // Arrange
            _mockCustomerRepo.Setup(repo => repo.GetAllCustomers()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userService.GetAllCustomers();

            // Assert
            Assert.Null(result);
        }

        // Test to modify a customer
        [Fact]
        public async Task TestToModifyCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "7483920332"
            };
            int id = 1;

            _mockCustomerRepo.Setup(repo => repo.ModifyCustomer(id, customer)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.ModifyCustomer(id, customer)).Returns(Task.FromResult(true));

            // Act
            var result = await _userService.ModifyUsers(id, customer);

            // Assert
            Assert.True(result);
        }

        // Test to modify an employee
        [Fact]
        public async Task TestToModifyEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "7483920332"
            };
            int id = 1;

            _mockEmployeeRepo.Setup(repo => repo.ModifyEmployee(id, employee)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.ModifyEmployee(id, employee)).Returns(Task.FromResult(true));

            // Act
            var result = await _userService.ModifyUsers(id, employee);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TestToDeleteCustomer()
        {
            // Arrange
            int id = 1;
            _mockCustomerRepo.Setup(repo => repo.DeleteCustomer(id)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.DeleteCustomer(id)).Returns(Task.FromResult(true));

            // Act
            var result = await _userService.DeleteCustomer(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TestToDeleteEmployee()
        {
            // Arrange
            int id = 1;
            _mockEmployeeRepo.Setup(repo => repo.DeleteEmployee(id)).ReturnsAsync(true);
            _mockCache.Setup(cache => cache.DeleteEmployee(id)).Returns(Task.FromResult(true));

            // Act
            var result = await _userService.DeleteEmployee(id);

            // Assert
            Assert.True(result);
        }
    }
}