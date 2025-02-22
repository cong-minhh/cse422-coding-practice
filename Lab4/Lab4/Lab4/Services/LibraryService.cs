using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IReaderRepository _readerRepository;

        public LibraryService(IBookRepository bookRepository, IReaderRepository readerRepository)
        {
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
        }

        public void AddBook(string title, string author, string category, int quantity)
        {
            var book = new Book(title, author, category, quantity);
            _bookRepository.AddBook(book);
        }

        public IEnumerable<IBook> SearchBooks(string searchTerm, SearchType searchType)
        {
            return searchType switch
            {
                SearchType.Title => _bookRepository.SearchByTitle(searchTerm),
                SearchType.Category => _bookRepository.SearchByCategory(searchTerm),
                _ => throw new ArgumentException("Invalid search type")
            };
        }

        public void LendBook(string readerId, string bookId)
        {
            var reader = _readerRepository.GetReaderById(readerId);
            var book = _bookRepository.GetBookById(bookId);

            if (book.AvailableQuantity <= 0)
                throw new InvalidOperationException("Book is not available");

            if (!reader.CanBorrowBooks())
                throw new InvalidOperationException("Reader has reached maximum borrowed books limit");

            book.UpdateQuantity(-1);
            reader.BorrowBook(book);
        }

        public void ReturnBook(string readerId, string bookId)
        {
            var reader = _readerRepository.GetReaderById(readerId);
            var book = _bookRepository.GetBookById(bookId);

            reader.ReturnBook(book);
            book.UpdateQuantity(1);
        }
    }
} 