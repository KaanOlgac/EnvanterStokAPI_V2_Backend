using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly EnvanterContext _context;

        public ProductsController(EnvanterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.StkProducts
                                         .OrderBy(p=> p.ProductId)
                                         .ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.StkProducts.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(StkProduct newProduct)
        {
            _context.StkProducts.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Update(int id, StkProduct updatedProduct)
        {
            var product = await _context.StkProducts.FindAsync(id);
            if (product == null)
                return NotFound();

            product.CategoryId = updatedProduct.CategoryId;
            product.ProductCode = updatedProduct.ProductCode;
            product.ProductName = updatedProduct.ProductName;
            product.UnitName = updatedProduct.UnitName;
            product.MinStockLevel = updatedProduct.MinStockLevel;
            product.ClearanceLevel = updatedProduct.ClearanceLevel;
            product.IsActive = updatedProduct.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.StkProducts.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.StkProducts.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}