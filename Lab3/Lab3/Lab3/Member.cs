using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab3.Interfaces;

namespace Lab3
{
    internal class Member : IPrintable, IMemberActions
    {
        private string _memberId;
        private string _name;
        private string _email;

        public string MemberId
        {
            get { return _memberId; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("MemberId cannot be empty or null");
                }
                _memberId = value;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be empty or null");
                }
                _name = value;
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Email cannot be empty or null");
                }
                if (!value.Contains("@") || !value.Contains("."))
                {
                    throw new ArgumentException("Invalid email format");
                }
                _email = value;
            }
        }
        public Member(string memberId, string name, string email)
        {
            MemberId = memberId;
            Name = name;
            Email = email;
        }

        public virtual void DisplayInfo()
        {
            PrintDetails();
        }

        public virtual void PrintDetails()
        {
            Console.WriteLine("Member Information:");
            Console.WriteLine($"MemberId: {MemberId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {Email}");
        }

        public virtual void BorrowBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            if (book.CopiesAvailable <= 0)
            {
                throw new InvalidOperationException($"No copies of '{book.Title}' are available for borrowing");
            }

            book.CopiesAvailable--;
            Console.WriteLine($"{Name} has borrowed '{book.Title}'");
        }

        public virtual void ReturnBook(Book book)
        {
            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            book.CopiesAvailable++;
            Console.WriteLine($"{Name} has returned '{book.Title}'");
        }
    }
}
