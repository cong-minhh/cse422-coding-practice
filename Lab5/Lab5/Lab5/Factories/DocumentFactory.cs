using System;
using Lab5.Models;

namespace Lab5.Factories
{
    // Abstract Factory
    public abstract class DocumentFactory
    {
        public abstract Document CreateDocument();
        
        // Factory Method that creates and returns a document
        public Document GetDocument()
        {
            Document document = CreateDocument();
            return document;
        }
    }

    // Concrete Factory for Book
    public class BookFactory : DocumentFactory
    {
        private string _title;
        private string _author;
        private DateTime _publicationDate;
        private string _isbn;
        private int _pages;

        public BookFactory(string title, string author, DateTime publicationDate, string isbn, int pages)
        {
            _title = title;
            _author = author;
            _publicationDate = publicationDate;
            _isbn = isbn;
            _pages = pages;
        }

        public override Document CreateDocument()
        {
            return new Book(_title, _author, _publicationDate, _isbn, _pages);
        }
    }

    // Concrete Factory for Magazine
    public class MagazineFactory : DocumentFactory
    {
        private string _title;
        private string _author;
        private DateTime _publicationDate;
        private string _issueNumber;
        private string _publisher;

        public MagazineFactory(string title, string author, DateTime publicationDate, string issueNumber, string publisher)
        {
            _title = title;
            _author = author;
            _publicationDate = publicationDate;
            _issueNumber = issueNumber;
            _publisher = publisher;
        }

        public override Document CreateDocument()
        {
            return new Magazine(_title, _author, _publicationDate, _issueNumber, _publisher);
        }
    }

    // Concrete Factory for Newspaper
    public class NewspaperFactory : DocumentFactory
    {
        private string _title;
        private string _author;
        private DateTime _publicationDate;
        private string _edition;

        public NewspaperFactory(string title, string author, DateTime publicationDate, string edition)
        {
            _title = title;
            _author = author;
            _publicationDate = publicationDate;
            _edition = edition;
        }

        public override Document CreateDocument()
        {
            return new Newspaper(_title, _author, _publicationDate, _edition);
        }
    }
}