using Api1_Library.Models;
using Api1_Library.DTOs;
using Api1_Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Api1_Library.Controllers
{
    [ApiController]
    [Route("api/loans")]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _lcxt;
        public LoansController(LibraryContext lcxt)
        {
            _lcxt = lcxt;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var loans = await _lcxt.Loans
                .Join(_lcxt.Members, l => l.MemberId, m => m.Id, (l, m) => new { l, m })
                .Join(_lcxt.Books, lm => lm.l.BookId, b => b.Id, (lm, b) => new
                {
                    lm.l.Id,
                    MemberId = lm.m.Id,
                    Member = lm.m.Name,
                    BookId = b.Id,
                    Book = b.Title,
                    lm.l.LoanDate,
                    lm.l.DueDate,
                    lm.l.ReturnDate,
                })
                .OrderByDescending(l => l.Id)
                .ToListAsync();
            return Ok(loans);
        }
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(BorrowLoanDto dto)
        {
            bool memberExists = await _lcxt.Members.AnyAsync(m => m.Id == dto.MemberId);
            if (!memberExists) return NotFound("Member not found");
            var book = await _lcxt.Books.FirstOrDefaultAsync(b => b.Id == dto.BookId);
            if (book == null) return NotFound("Book not found");
            int activeLoans = await _lcxt.Loans.CountAsync(
                l => l.BookId == dto.BookId && l.ReturnDate == null);
            if (activeLoans >= book.Copies)
            {
                return Conflict("No copies available");
            }
            var now = DateTime.UtcNow;
            var loan = new Loan
            {
                MemberId = dto.MemberId,
                BookId = dto.BookId,
                LoanDate = now,
                DueDate = now.AddDays(dto.Days)
            };
            _lcxt.Loans.Add(loan);
            await _lcxt.SaveChangesAsync();
            return Created(string.Empty, loan);
        }
        [HttpPost("return")]
        public async Task<IActionResult> Return([FromQuery] int loanId)
        {
            var loan = await _lcxt.Loans.FindAsync(loanId);
            if (loan == null) return NotFound("Loan not found");
            if (loan.ReturnDate != null)
            {
                return Conflict("Loan already returned");
            }
            loan.ReturnDate = DateTime.UtcNow;
            await _lcxt.SaveChangesAsync();
            return Ok();
        }
    }

}
