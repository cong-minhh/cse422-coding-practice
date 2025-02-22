using System;
using System.Collections.Generic;
using System.Linq;

public class BookRepository : IBookRepository
{
    private readonly Dictionary<string, IBook> _books;

    public BookRepository()
    {
        _books = new Dictionary<string, IBook>();
    }

    public void AddBook(IBook book)
    {
        if (!_books.ContainsKey(book.Id))
        {
            _books.Add(book.Id, book);
        }
    }

    public IBook GetBookById(string id)
    {
        if (!_books.ContainsKey(id))
            throw new KeyNotFoundException($"Book with ID {id} not found");
            
        return _books[id];
    }

    public IEnumerable<IBook> SearchByTitle(string title)
    {
        return _books.Values
            .Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IEnumerable<IBook> SearchByCategory(string category)
    {
        return _books.Values
            .Where(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IEnumerable<IBook> GetAllBooks()
    {
        return _books.Values.ToList();
    }
} 