using Microsoft.AspNetCore.Mvc;
using Store_Management.CORE.Contracts;
using Store_Management.CORE.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Store_Management.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            var result = await _productService.AddProduct(product);

            if (result)
            {
                return Ok("Product added successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding product.");
            }
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            if (products == null || products.Count == 0)
            {
                return NotFound("No product found.");
            }

            return Ok(products);
        }

        [HttpPut("ModifyProduct")]
        public async Task<IActionResult> ModifyProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            var result = await _productService.ModifyProduct(id, product);

            if(result)
            {
                return Ok("Product modified successfully.");
            }
            else
            {
                return StatusCode(500, "An error has occurred while modifying the product.");
            }
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result)
            {
                return Ok("Product successfully deleted.");
            }
            else
            {
                return NotFound("Product not found or could not be deleted.");
            }
        }
    }
}
