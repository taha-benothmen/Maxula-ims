using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IMS.Shared.Data;
using IMS.Shared.Models;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddProdect")]
        public async Task<IActionResult> PostProduct(Product product)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == product.Name);

            if (existingProduct != null)
            {
                existingProduct.Quantity += product.Quantity;
                _context.Products.Update(existingProduct);
            }
            else
            {
                _context.Products.Add(product);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product updatedProduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            product.Name = updatedProduct.Name ?? product.Name;
            product.Quantity = updatedProduct.Quantity != 0 ? updatedProduct.Quantity : product.Quantity; 
            product.Price = updatedProduct.Price != 0 ? updatedProduct.Price : product.Price; 


            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product updated successfully", Product = product });
        }

        [HttpGet("GetProduct")]
        public async Task<ActionResult<int>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product deleted successfully" });
        }

    }
}
