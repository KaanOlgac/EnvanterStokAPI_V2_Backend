using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI.Models;

namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly EnvanterContext _context;

        public StocksController(EnvanterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.StkStocks
                                       .OrderBy(s => s.StockId)
                                       .ToListAsync();
            return Ok(stocks);
        }

        [HttpGet("critical")]
        public async Task<IActionResult> GetCritical()
        {
            var criticalStocks = await (
                from stock in _context.StkStocks
                join product in _context.StkProducts
                    on stock.ProductId equals product.ProductId
                where stock.NetQuantity <= product.MinStockLevel
                select new
                {
                    stock.StockId,
                    stock.ProductId,
                    product.ProductName,
                    product.ProductCode,
                    stock.CurrentQuantity,
                    stock.NetQuantity,
                    product.MinStockLevel
                }
            ).ToListAsync();

            return Ok(criticalStocks);
        }

        public class UpdateStockDto
        {
            public int CurrentQuantity { get; set; }
        }

        [HttpPut("product/{productId}")]
        public async Task<IActionResult> UpdateByProduct(int productId, UpdateStockDto dto)
        {
            var stock = await _context.StkStocks
                .FirstOrDefaultAsync(s => s.ProductId == productId);
            if (stock == null)
                return NotFound("Bu ürüne ait stok kaydı bulunamadı.");

            stock.CurrentQuantity = dto.CurrentQuantity;
            stock.NetQuantity = dto.CurrentQuantity - stock.ReservedQuantity;
            stock.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        public class CreateStockDto
        {
            public int ProductId { get; set; }
            public int CurrentQuantity { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateStockDto dto)
        {
            var newStock = new StkStock
            {
                ProductId = dto.ProductId,
                CurrentQuantity = dto.CurrentQuantity,
                ReservedQuantity = 0,
                DeliveredQuantity = 0,
                NetQuantity = dto.CurrentQuantity,
                UpdatedAt = DateTime.UtcNow
            };

            _context.StkStocks.Add(newStock);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Stok kaydı oluşturuldu", stockId = newStock.StockId });
        }
    }
}