using AutoMapper;
using LibraryManagementAPI.Data.Entities;
using LibraryManagementAPI.Data.Interfaces;
using LibraryManagementAPI.Services.Constants;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBookValidator _bookDtoValidator;

        public BookService(IBookRepository repository, IMapper mapper, IBookValidator bookDtoValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _bookDtoValidator = bookDtoValidator;
        }

        public async Task<BookDto> CreateBookAsync(BookDto book)
        {
            _bookDtoValidator.ValidateBook(book);

            if (!await IsISBNUniqueAsync(book.ISBN))
            {
                throw new DbUpdateException(BookErrors.ISBNAlreadyExists);
            }

            var entity = _mapper.Map<Book>(book);
            var createdBookEntity = await _repository.InsertAsync(entity);
            return _mapper.Map<BookDto>(createdBookEntity);
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var books = await _repository.GetAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookAsync(int id)
        {
            var book = await _repository.GetByIdAsync(id);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> GetBookByISBNAsync(string isbn)
        {
            var book = await _repository.GetByISBNAsync(isbn);

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(int id, BookDto book)
        {
            _bookDtoValidator.ValidateBook(book);

            var existingBook = await _repository.GetByIdAsync(id);

            if (existingBook == null)
            {
                throw new InvalidOperationException(BookErrors.BookNotFound);
            }

            if (existingBook.ISBN != book.ISBN && !await IsISBNUniqueAsync(book.ISBN, id))
            {
                throw new DbUpdateException(BookErrors.ISBNAlreadyExists);
            }

            book.Id = id;

            _mapper.Map(book, existingBook);

            await _repository.UpdateAsync(existingBook);

            return _mapper.Map<BookDto>(existingBook);
        }

        public Task<bool> DeleteBookAsync(int id)
        {
            return _repository.DeleteAsync(new Book { Id = id });
        }

        private async Task<bool> IsISBNUniqueAsync(string isbn, int? bookId = null)
        {
            var existingBook = await _repository.GetByISBNAsync(isbn);

            if (existingBook == null)
            {
                return true;
            }

            if (bookId.HasValue && existingBook.Id == bookId.Value)
            {
                return true;
            }

            return false;
        }
    }
}
