public interface ILibraryService
{
    void AddBook(string title, string author, string category, int quantity);
    IEnumerable<IBook> SearchBooks(string searchTerm, SearchType searchType);
    void LendBook(string readerId, string bookId);
    void ReturnBook(string readerId, string bookId);
}

public enum SearchType
{
    Title,
    Category
} 