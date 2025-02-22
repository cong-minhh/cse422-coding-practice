using System;

public class Book : IBook
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public string Category { get; private set; }
    public int AvailableQuantity { get; private set; }

    public Book(string title, string author, string category, int quantity)
    {
        Id = Guid.NewGuid().ToString();
        Title = title;
        Author = author;
        Category = category;
        AvailableQuantity = quantity;
    }

    public void UpdateQuantity(int amount)
    {
        AvailableQuantity += amount;
    }
} 