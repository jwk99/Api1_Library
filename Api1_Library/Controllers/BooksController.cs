using Api1_Library.Models;
using Api1_Library.DTOs;
using Api1_Library.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Api1_Library.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _lcxt;
        public BooksController(LibraryContext lcxt)
        {
            _lcxt = lcxt;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _lcxt.Books
                .Select(b=>new
                {
                    b.Id,
                    b.Title,
                    b.Author,
                    b.Copies,
                    Available = b.Copies - _lcxt.Loans.Count(l=>l.BookId == b.Id && l.ReturnDate==null)
                })
                .ToListAsync();
            return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookDto dto)
        {
            if (dto.Copies < 0)
                return BadRequest("Copies must be >= 0");
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Copies = dto.Copies,
            };
            _lcxt.Books.Add(book);
            await _lcxt.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAll),
                new { id = book.Id },
                book);
        }
    }
}
