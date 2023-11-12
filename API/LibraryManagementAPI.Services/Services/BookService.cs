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
        public async Task<BookDto> CreateBookAsync(BookDto book, CancellationToken cancellationToken)
        {
            _bookDtoValidator.ValidateBook(book);
            if (!await IsISBNUniqueAsync(cancellationToken,book.ISBN))
            {
                throw new DbUpdateException(BookErrors.ISBNAlreadyExists);
            }

            var entity = _mapper.Map<Book>(book);
            var createdBookEntity = _repository.Insert(entity);
            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BookDto>(createdBookEntity);
        }

        public async Task<List<BookDto>> GetAllBooksAsync(CancellationToken cancellationToken)
        {
            var books = await _repository.GetAsync(cancellationToken);
            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<BookDto> GetBookAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureBookExists(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> GetBookByISBNAsync(string isbn, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByISBNAsync(isbn, cancellationToken);
            EnsureBookExists(book);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBookAsync(int id, BookDto book, CancellationToken cancellationToken)
        {
            _bookDtoValidator.ValidateBook(book);
            var existingBook = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureBookExists(existingBook);
            if (existingBook.ISBN != book.ISBN && !await IsISBNUniqueAsync(cancellationToken, book.ISBN, id))
            {
                throw new DbUpdateException(BookErrors.ISBNAlreadyExists);
            }

            _mapper.Map(book, existingBook);
            existingBook.Id = id;
            _repository.Update(existingBook);
            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BookDto>(existingBook);
        }

        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _repository.GetByIdAsync(id, cancellationToken);
            EnsureBookExists(book);
            _repository.Delete(new Book { Id = id });
            await _repository.SaveChangesAsync(cancellationToken);
        }

        private void EnsureBookExists(Book? existingBook)
        {
            if (existingBook is null)
            {
                throw new InvalidOperationException(BookErrors.BookNotFound);
            }
        }

        private async Task<bool> IsISBNUniqueAsync(CancellationToken cancellationToken, string isbn, int? bookId = null)
        {
            var existingBook = await _repository.GetByISBNAsync(isbn, cancellationToken);
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