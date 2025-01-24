using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Interfaces;

namespace Lab3
{
    internal class Book : IPrintable
    {
        private string _isbn = string.Empty;
        private string _title = string.Empty;
        private string _author = string.Empty;
        private int _year;
        private int _copiesAvailable;

        public string ISBN
        {
            get { return _isbn; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ISBN cannot be empty or null");
                _isbn = value;
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be empty or null");
                _title = value;
            }
        }

        public string Author
        {
            get { return _author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Author cannot be empty or null");
                _author = value;
            }
        }

        public int Year
        {
            get { return _year; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Year cannot be less than 0");
                _year = value;
            }
        }

        public int CopiesAvailable
        {
            get { return _copiesAvailable; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("CopiesAvailable cannot be less than 0");
                _copiesAvailable = value;
            }
        }

        public Book(string isbn, string title, string author, int year, int copiesAvailable)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
            Year = year;
            CopiesAvailable = copiesAvailable;
        }

        public void DisplayInfo()
        {
            PrintDetails();
        }

        public void PrintDetails()
        {
            Console.WriteLine("Book Information:");
            Console.WriteLine($"ISBN: {ISBN}");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Author: {Author}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Copies Available: {CopiesAvailable}");
        }
    }
}
