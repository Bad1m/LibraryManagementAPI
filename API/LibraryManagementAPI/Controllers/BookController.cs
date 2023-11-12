using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementAPI.Filters;

namespace LibraryManagementAPI.Controllers
{
    [TypeFilter(typeof(CancellationTokenFilter))]
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService booksService)
        {
            _bookService = booksService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks(CancellationToken cancellationToken, int page = 1, int pageSize = 10)
        {
            var books = await _bookService.GetAllBooksAsync(cancellationToken);

            var totalBooks = books.Count;
            var totalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
            var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var pagedResult = new PagedResult<BookDto>
            {
                Items = paginatedBooks,
                TotalItems = totalBooks,
                Page = page,
                TotalPages = totalPages
            };

            return Ok(pagedResult);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookAsync(id, cancellationToken);
            return Ok(book);
        }

        [AllowAnonymous]
        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetBookByISBN([FromRoute] string isbn, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn, cancellationToken);
            return Ok(book);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookDto book, CancellationToken cancellationToken)
        {
            var createdBook = await _bookService.CreateBookAsync(book, cancellationToken);
            return Ok(createdBook);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto book, CancellationToken cancellationToken)
        {
            var updatedBook = await _bookService.UpdateBookAsync(id, book, cancellationToken);
            return Ok(updatedBook);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id, CancellationToken cancellationToken)
        {
            await _bookService.DeleteBookAsync(id, cancellationToken);
            return Ok();
        }
    }
}