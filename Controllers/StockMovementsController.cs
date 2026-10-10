using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI;
using EnvanterStokAPI.Models;

namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementsController : ControllerBase
    {
        private readonly EnvanterContext _context;
        public StockMovementsController(EnvanterContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movements = await _context.StkStockMovements
                .OrderByDescending(m => m.MovementDate)
                .ToListAsync();
            return Ok(movements);
        }
    }
}