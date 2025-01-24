using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class BorrowTransaction : Transaction
    {
        private Book _bookBorrowed;
        public Book BookBorrowed
        {
            get { return _bookBorrowed; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "BookReturned cannot be null");
                }
                _bookBorrowed = value;
            }
        }
        public override void Execute()
        {
            if (BookBorrowed == null)
            {
                throw new InvalidOperationException("Cannot execute borrow transaction: No book specified");
            }

            if (Member == null)
            {
                throw new InvalidOperationException("Cannot execute borrow transaction: No member specified");
            }

            if (BookBorrowed.CopiesAvailable <= 0)
            {
                throw new InvalidOperationException($"Cannot borrow book: {BookBorrowed.Title} is not available");
            }

            if (Member is PremiumMember premiumMember)
            {
                if (premiumMember.MembershipExpiry < DateTime.Now)
                {
                    throw new InvalidOperationException("Premium membership has expired");
                }
            }

            BookBorrowed.CopiesAvailable--;

            Console.WriteLine($"Borrow Transaction Executed Successfully:");
            Console.WriteLine($"Transaction ID: {TransactionId}");
            Console.WriteLine($"Date: {TransactionDate}");
            Console.WriteLine($"Member: {Member.Name} (ID: {Member.MemberId})");
            Console.WriteLine($"Book Borrowed: {BookBorrowed.Title} (ISBN: {BookBorrowed.ISBN})");
            Console.WriteLine($"Remaining Copies: {BookBorrowed.CopiesAvailable}");
        }
    }
}
