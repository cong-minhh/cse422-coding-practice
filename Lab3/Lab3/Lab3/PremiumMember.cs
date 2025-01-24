using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Lab3.Interfaces;


namespace Lab3
{
    internal class PremiumMember : Member, IMemberActions
    {
        private DateTime _membershipExpiry;
        private int _maxBooksAllowed;

        public PremiumMember(string memberId, string name, string email) : base(memberId, name, email)
        {
            _membershipExpiry = DateTime.Now.AddYears(1);
            _maxBooksAllowed = 10; 
        }

        public DateTime MembershipExpiry
        {
            get { return _membershipExpiry; }
            set
            {
                if (value < DateTime.Now)
                {
                    throw new ArgumentException("Membership expiry date cannot be in the past");
                }
                _membershipExpiry = value;
            }
        }

        public int MaxBooksAllowed
        {
            get { return _maxBooksAllowed; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("MaxBooksAllowed cannot be negative");
                }
                if (value > 50) 
                {
                    throw new ArgumentException("MaxBooksAllowed cannot exceed 50");
                }
                _maxBooksAllowed = value;
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("Premium Member Information:");
            Console.WriteLine($"MemberId: {MemberId}");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"MembershipExpiry: {MembershipExpiry:d}"); 
            Console.WriteLine($"MaxBooksAllowed: {MaxBooksAllowed}");
        }

        public override void BorrowBook(Book book)
        {
            if (MembershipExpiry < DateTime.Now)
            {
                throw new InvalidOperationException("Cannot borrow books: Premium membership has expired");
            }

            base.BorrowBook(book);
        }

        public override void ReturnBook(Book book)
        {
            base.ReturnBook(book);
        }

        public override void PrintDetails()
        {
            base.PrintDetails();
            Console.WriteLine($"Membership Expiry: {MembershipExpiry:d}");
            Console.WriteLine($"Max Books Allowed: {MaxBooksAllowed}");
        }
    }
}
