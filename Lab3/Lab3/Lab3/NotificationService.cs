using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    internal class NotificationService
    {
        private string _serviceName;

        public NotificationService(string serviceName = "Default")
        {
            _serviceName = serviceName;
        }

        // Method to subscribe to library events
        public void SubscribeToLibraryEvents(Library library)
        {
            if (library == null)
                throw new ArgumentNullException(nameof(library));

            // Subscribe multiple handlers to demonstrate multiple subscribers
            library.OnBookBorrowed += SendBorrowNotification;
            library.OnBookBorrowed += LogBorrowingActivity;
            library.OnBookBorrowed += UpdateBorrowingStatistics;
        }

        // Handler for sending notifications
        private void SendBorrowNotification(Book book, Member member)
        {
            Console.WriteLine($"\n[{_serviceName} - Notification]");
            Console.WriteLine($"Member {member.Name} has borrowed '{book.Title}'");
            if (member is PremiumMember)
            {
                Console.WriteLine("Premium member benefits applied!");
            }
        }

        // Handler for logging
        private void LogBorrowingActivity(Book book, Member member)
        {
            Console.WriteLine($"\n[{_serviceName} - Log]");
            Console.WriteLine($"Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"Book: {book.Title} (ISBN: {book.ISBN})");
            Console.WriteLine($"Member: {member.Name} (ID: {member.MemberId})");
        }

        // Handler for statistics
        private void UpdateBorrowingStatistics(Book book, Member member)
        {
            Console.WriteLine($"\n[{_serviceName} - Statistics]");
            Console.WriteLine($"Remaining copies of '{book.Title}': {book.CopiesAvailable}");
            Console.WriteLine($"Borrowed by member type: {(member is PremiumMember ? "Premium" : "Regular")}");
        }

        public virtual void SendNotification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty or null");
            }
            Console.WriteLine($"[{_serviceName}] Sending notification: {message}");
        }

        public void SendNotification(string message, string recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentException("Recipient cannot be empty or null");
            }
            Console.WriteLine($"[{_serviceName}] Sending notification to {recipient}: {message}");
        }

        public void SendNotification(string message, List<string> recipients)
        {
            if (recipients == null || recipients.Count == 0)
            {
                throw new ArgumentException("Recipients list cannot be null or empty");
            }

            foreach (var recipient in recipients)
            {
                if (string.IsNullOrWhiteSpace(recipient))
                {
                    throw new ArgumentException("Recipient cannot be empty or null");
                }
            }

            Console.WriteLine($"[{_serviceName}] Sending notification to {recipients.Count} recipients: {message}");
            foreach (var recipient in recipients)
            {
                Console.WriteLine($"- {recipient}");
            }
        }
    }
}
