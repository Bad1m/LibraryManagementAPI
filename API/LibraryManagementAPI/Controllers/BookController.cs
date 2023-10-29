using AutoMapper;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService booksService, IMapper mapper)
        {
            _bookService = booksService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetBooksAsync();
            var bookViews = _mapper.Map<List<BookViewModel>>(books);
            return Ok(bookViews);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook([FromRoute] int id)
        {
            var book = await _bookService.GetBookAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var bookView = _mapper.Map<BookViewModel>(book);
            return bookView is not null ? Ok(book) : NoContent();
        }

        [AllowAnonymous]
        [HttpGet("isbn/{isbn}")]
        public async Task<IActionResult> GetBookByISBN([FromRoute] string isbn)
        {
            var book = await _bookService.GetBookByISBNAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }

            var bookView = _mapper.Map<BookViewModel>(book);
            return bookView is not null ? Ok(book) : NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookViewModel bookView)
        {
            var book = _mapper.Map<BookDto>(bookView);
            var createdBook = await _bookService.CreateBookAsync(book);
            var createdBookView = _mapper.Map<BookDto>(createdBook);
            return Ok(createdBookView);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookViewModel bookView)
        {
            var book = _mapper.Map<BookDto>(bookView);
            var updatedBook = await _bookService.UpdateBookAsync(id, book);
            if (updatedBook == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookDto>(updatedBook));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var isDelete = await _bookService.DeleteBookAsync(id);
            return isDelete ? Ok() : BadRequest();
        }
    }
}