using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class ReturnTransaction : Transaction
    {
        private Book _bookReturned;
        public Book BookReturned
        {
            get { return _bookReturned; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "BookReturned cannot be null");
                }
                _bookReturned = value;
            }
        }
        public override void Execute()
        {
            if (BookReturned == null)
            {
                throw new InvalidOperationException("Cannot execute return transaction: No book specified");
            }

            if (Member == null)
            {
                throw new InvalidOperationException("Cannot execute return transaction: No member specified");
            }

            BookReturned.CopiesAvailable++;

            Console.WriteLine($"Return Transaction Executed Successfully:");
            Console.WriteLine($"Transaction ID: {TransactionId}");
            Console.WriteLine($"Date: {TransactionDate}");
            Console.WriteLine($"Member: {Member.Name} (ID: {Member.MemberId})");
            Console.WriteLine($"Book Returned: {BookReturned.Title} (ISBN: {BookReturned.ISBN})");
            Console.WriteLine($"Updated Copies Available: {BookReturned.CopiesAvailable}");
        }
    }
}
