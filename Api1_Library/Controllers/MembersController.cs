using Api1_Library.Models;
using Api1_Library.DTOs;
using Api1_Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api1_Library.Controllers
{
    [ApiController]
    [Route("api/members")]
    public class MembersController : ControllerBase
    {
        private readonly LibraryContext _lcxt;
        public MembersController(LibraryContext lcxt)
        {
            _lcxt = lcxt;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var members = await _lcxt.Members.ToListAsync();
            return Ok(members);
        }
        [HttpPost]
        public async Task <IActionResult> Create(CreateMemberDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
            { 
                return BadRequest("Email is required");
            }
            bool emailExists = await _lcxt.Members
                .AnyAsync(m=>m.Email== dto.Email);
            if (emailExists)
            {
                return Conflict("Email already exists");
            }
            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _lcxt.Members.Add(member);
            await _lcxt.SaveChangesAsync();
            return CreatedAtAction(
                nameof(GetAll),
                new { id = member.Id },
                member);

        }

    }
}
