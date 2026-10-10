using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnvanterStokAPI.Models;

namespace EnvanterStokAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EnvanterContext _context;

        public UsersController(EnvanterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _context.StkUsers.ToListAsync();
            return Ok(users);
        }

        public class UpdateUserDto
        {
            public int RoleId { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public bool IsActive { get; set; }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var user = await _context.StkUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            user.RoleId = dto.RoleId;
            user.FullName = dto.FullName;
            user.EmailAddress = dto.EmailAddress;
            user.IsActive = dto.IsActive;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}