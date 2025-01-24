using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Interfaces;

namespace Lab3
{
    internal class Library : IPrintable
    {
        private string _libraryName;
        private List<Book> _books;
        private List<Member> _members;

        public string LibraryName
        {
            get { return _libraryName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Library name cannot be empty or null");
                }
                _libraryName = value;
            }
        }

        public List<Book> Books
        {
            get { return _books; }
            private set { _books = value ?? new List<Book>(); }
        }

        public List<Member> Members
        {
            get { return _members; }
            private set { _members = value ?? new List<Member>(); }
        }

        // Define the event using Action<Book, Member> delegate
        public event Action<Book, Member> OnBookBorrowed;

        // parameterless Constructor
        public Library()
        {
            LibraryName = "Default Library";
            Books = new List<Book>();
            Members = new List<Member>();
        }

        // parameterized Constructor
        public Library(string libraryName, List<Book> initialBooks)
        {
            LibraryName = libraryName;
            Books = new List<Book>(initialBooks ?? new List<Book>());
            Members = new List<Member>();
        }

        // copy constructor
        public Library(Library other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            LibraryName = other.LibraryName;
            Books = new List<Book>(other.Books);
            Members = new List<Member>(other.Members);
        }

        public void DisplayLibraryInfo()
        {
            PrintDetails();
        }

        public void PrintDetails()
        {
            Console.WriteLine($"Library Information for: {LibraryName}");
            Console.WriteLine($"Number of Books: {Books.Count}");
            Console.WriteLine($"Number of Members: {Members.Count}");
            
            Console.WriteLine("\nBook Collection:");
            foreach (var book in Books)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} ({book.CopiesAvailable} copies available)");
            }

            Console.WriteLine("\nMember List:");
            foreach (var member in Members)
            {
                if (member is PremiumMember premiumMember)
                {
                    Console.WriteLine($"- {member.Name} (Premium Member)");
                }
                else
                {
                    Console.WriteLine($"- {member.Name} (Regular Member)");
                }
            }
        }
        public void AddBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }
            Books.Add(book);
        }

        public void AddMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }
            Members.Add(member);
        }

        // Method to handle book borrowing and trigger the event
        public void BorrowBook(Book book, Member member)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            if (!Books.Contains(book))
                throw new InvalidOperationException("Book is not available in the library");
            if (!Members.Contains(member))
                throw new InvalidOperationException("Member is not registered in the library");
            if (book.CopiesAvailable <= 0)
                throw new InvalidOperationException("No copies available for borrowing");

            // Update book status
            book.CopiesAvailable--;

            // Trigger the event if there are subscribers
            OnBookBorrowed?.Invoke(book, member);
        }

        public void DisplayAllBooks()
        {
            Console.WriteLine($"\nBooks in {LibraryName}:");
            foreach (var book in Books)
            {
                Console.WriteLine($"- {book.Title} by {book.Author} ({book.CopiesAvailable} copies available)");
            }
        }

        public void DisplayAllMembers()
        {
            Console.WriteLine($"\nMembers in {LibraryName}:");
            foreach (var member in Members)
            {
                Console.WriteLine($"- {member.Name} (ID: {member.MemberId})");
            }
        }
    }
}
