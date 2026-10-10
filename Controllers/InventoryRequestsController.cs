using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryRequestsController : ControllerBase
    {
        private readonly EnvanterContext _context;

        public InventoryRequestsController(EnvanterContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests =await _context.StkInventoryRequests.ToListAsync();
            return Ok(requests);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var requests = await _context.StkInventoryRequests
                .Where(r => r.RequestedByUserId == userId)
                .ToListAsync();
            return Ok(requests);
        }
        public class CreateRequestDto
        {
            public int RequestedByUserId { get; set; }
            public int ProductId { get; set; }
            public int RequestedQuantity { get; set; }
            public string? Note { get; set; }

        }
        public class ApproveRequestDto
        {
            public int ApprovedByUserId { get; set; }
            public int ApprovedQuantity { get; set; } 
        }
        public class RejectRequestDto
        {
            public int ApprovedByUserId { get; set; }
            public string? Note { get; set; }
        }
        public class DeliveredRequestDto
        {
            public int DeliveredByUserId { get; set; }
            public int? DeliveredQuantity { get; set; }
        }
        [HttpPut("{id}/deliver")]
        public async Task<IActionResult> Deliver(int id,    DeliveredRequestDto dto)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_deliver_request({0}, {1}, {2})",
                    id,dto.DeliveredByUserId, (object?)dto.DeliveredQuantity ?? DBNull.Value
                );
                return Ok(new { message = "Ürün Teslim Edildi", requestId = id});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }
        
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id, RejectRequestDto dto)
        {
            try 
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_reject_request({0}, {1}, {2})",
                    id, dto.ApprovedByUserId, dto.Note ??""

                );
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            
        }
           
        
        
        
        [HttpPut("{id}/approve")]
public async Task<IActionResult> Approve(int id, ApproveRequestDto dto)
{
    try
    {
        await _context.Database.ExecuteSqlRawAsync(
            "CALL sp_approve_request({0}, {1}, {2})",
            id, dto.ApprovedByUserId, dto.ApprovedQuantity
        );
        return NoContent();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.InnerException?.Message ?? ex.Message);
    }
}
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_cancel_request({0})",
                    id
                );
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

      
      

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequestDto dto)
        {
            var newRequest = new StkInventoryRequest
            {
                RequestedByUserId = dto.RequestedByUserId,
                ProductId = dto.ProductId,
                RequestedQuantity = dto.RequestedQuantity,
                RemainingQuantity = dto.RequestedQuantity,
                Note = dto.Note,
                StatusCode = 'O',
                RequestDate = DateTime.UtcNow
            
                    
                
            };
            _context.StkInventoryRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            return Ok(new {message = "Talep oluşturuldu", requestId = newRequest.RequestId});
        }
    }
}