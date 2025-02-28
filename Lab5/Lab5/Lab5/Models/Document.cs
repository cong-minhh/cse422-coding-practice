using System;

namespace Lab5.Models
{
    // Base Document class
    public abstract class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsAvailable { get; set; } = true;

        public Document(string title, string author, DateTime publicationDate)
        {
            Title = title;
            Author = author;
            PublicationDate = publicationDate;
        }

        public abstract string GetDocumentType();
    }

    // Concrete Document types
    public class Book : Document
    {
        public string ISBN { get; set; }
        public int Pages { get; set; }

        public Book(string title, string author, DateTime publicationDate, string isbn, int pages) 
            : base(title, author, publicationDate)
        {
            ISBN = isbn;
            Pages = pages;
        }

        public override string GetDocumentType() => "Book";
    }

    public class Magazine : Document
    {
        public string IssueNumber { get; set; }
        public string Publisher { get; set; }

        public Magazine(string title, string author, DateTime publicationDate, string issueNumber, string publisher) 
            : base(title, author, publicationDate)
        {
            IssueNumber = issueNumber;
            Publisher = publisher;
        }

        public override string GetDocumentType() => "Magazine";
    }

    public class Newspaper : Document
    {
        public string Edition { get; set; }

        public Newspaper(string title, string author, DateTime publicationDate, string edition) 
            : base(title, author, publicationDate)
        {
            Edition = edition;
        }

        public override string GetDocumentType() => "Newspaper";
    }
}