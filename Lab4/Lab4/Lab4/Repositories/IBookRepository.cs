public interface IBookRepository
{
    void AddBook(IBook book);
    IBook GetBookById(string id);
    IEnumerable<IBook> SearchByTitle(string title);
    IEnumerable<IBook> SearchByCategory(string category);
    IEnumerable<IBook> GetAllBooks();
} 