using System;
using System.Collections.Generic;

public class Reader
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    private List<IBook> BorrowedBooks { get; set; }

    public Reader(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        BorrowedBooks = new List<IBook>();
    }

    public IReadOnlyList<IBook> GetBorrowedBooks() => BorrowedBooks.AsReadOnly();
    
    public bool CanBorrowBooks() => BorrowedBooks.Count < 3;
    
    public void BorrowBook(IBook book)
    {
        if (!CanBorrowBooks())
            throw new InvalidOperationException("Reader has reached maximum borrowed books limit");
            
        BorrowedBooks.Add(book);
    }

    public void ReturnBook(IBook book)
    {
        BorrowedBooks.Remove(book);
    }
} 