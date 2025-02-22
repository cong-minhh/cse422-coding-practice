public interface IBook
{
    string Id { get; }
    string Title { get; }
    string Author { get; }
    string Category { get; }
    int AvailableQuantity { get; }
    void UpdateQuantity(int change);
} 