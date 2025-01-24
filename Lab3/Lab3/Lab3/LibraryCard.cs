using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class LibraryCard
    {
        // Read-only property - can only be set in constructor
        public string CardNumber { get; }

        // Read-write property for Owner
        private Member _owner = null!;
        public Member Owner
        {
            get { return _owner; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Owner cannot be null");
                }
                _owner = value;
            }
        }

        // Read-only externally, but can be modified internally
        private DateTime _issueDate;
        public DateTime IssueDate
        {
            get { return _issueDate; }
            private set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("Issue date cannot be in the future");
                }
                _issueDate = value;
            }
        }

        public LibraryCard(string cardNumber, Member owner)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                throw new ArgumentException("Card number cannot be empty or null");
            }

            CardNumber = cardNumber;
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            IssueDate = DateTime.Now;
        }

        public void RenewCard()
        {
            // Update the issue date to current date
            IssueDate = DateTime.Now;
            Console.WriteLine($"Library card {CardNumber} has been renewed.");
            Console.WriteLine($"New issue date: {IssueDate:d}");
        }

        public void DisplayCardInfo()
        {
            Console.WriteLine("Library Card Information:");
            Console.WriteLine($"Card Number: {CardNumber}");
            Console.WriteLine($"Owner: {Owner.Name}");
            Console.WriteLine($"Issue Date: {IssueDate:d}");
            
            if (Owner is PremiumMember premiumMember)
            {
                Console.WriteLine("Card Type: Premium");
                Console.WriteLine($"Membership Expires: {premiumMember.MembershipExpiry:d}");
            }
            else
            {
                Console.WriteLine("Card Type: Standard");
            }
        }
    }
}
