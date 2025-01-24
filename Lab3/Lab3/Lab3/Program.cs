using System;
using System.Collections.Generic;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            TestExercise1();
            TestExercise2();
            TestExercise3();
            TestExercise4();
            TestExercise5();
            TestExercise6();
            TestExercise7();
            TestExercise8();
            TestExercise9();
            TestExercise10();
        }

        static void TestExercise1()
        {
            Console.WriteLine("\nExercise 1: Testing Book Class (Encapsulation)");
            Console.WriteLine(new string('=', 60));

            // Create and display a book
            var book = new Book("ISBN001", "The Great Gatsby", "F. Scott Fitzgerald", 1925, 3);
            book.DisplayInfo();
        }

        static void TestExercise2()
        {
            Console.WriteLine("\nExercise 2: Testing Member and PremiumMember Classes (Inheritance)");
            Console.WriteLine(new string('=', 60));

            // Create and display a premium member
            var premiumMember = new PremiumMember("PM001", "Jane Smith", "jane@email.com");
            premiumMember.DisplayInfo();
        }

        static void TestExercise3()
        {
            Console.WriteLine("\nExercise 3: Testing Transaction Classes (Abstraction)");
            Console.WriteLine(new string('=', 60));

            // Create and execute a borrow transaction
            var book = new Book("ISBN002", "1984", "George Orwell", 1949, 1);
            var member = new Member("M001", "John Doe", "john@email.com");
            var transaction = new BorrowTransaction
            {
                TransactionId = "BT001",
                TransactionDate = DateTime.Now,
                Member = member,
                BookBorrowed = book
            };
            transaction.Execute();
        }

        static void TestExercise4()
        {
            Console.WriteLine("\nExercise 4: Testing Transaction Handling (Polymorphism)");
            Console.WriteLine(new string('=', 60));

            // Create and execute a transaction polymorphically
            var book = new Book("ISBN003", "The Hobbit", "J.R.R. Tolkien", 1937, 1);
            var member = new Member("M002", "Alice Johnson", "alice@email.com");
            Transaction transaction = new BorrowTransaction
            {
                TransactionId = "BT002",
                TransactionDate = DateTime.Now,
                Member = member,
                BookBorrowed = book
            };
            Console.WriteLine($"Executing {transaction.GetType().Name}:");
            transaction.Execute();
        }

        static void TestExercise5()
        {
            Console.WriteLine("\nExercise 5: Testing Interfaces (IPrintable and IMemberActions)");
            Console.WriteLine(new string('=', 60));

            // Test IPrintable with a book
            var book = new Book("ISBN004", "Dune", "Frank Herbert", 1965, 2);
            Console.WriteLine("Testing IPrintable interface:");
            book.PrintDetails();
        }

        static void TestExercise6()
        {
            Console.WriteLine("\nExercise 6: Testing Library Class (Constructors)");
            Console.WriteLine(new string('=', 60));

            // Test parameterized constructor
            var books = new List<Book>
            {
                new Book("ISBN005", "Neuromancer", "William Gibson", 1984, 2)
            };
            var library = new Library("Sci-Fi Library", books);
            library.DisplayLibraryInfo();
        }

        static void TestExercise7()
        {
            Console.WriteLine("\nExercise 7: Testing NotificationService (Overloading and Overriding)");
            Console.WriteLine(new string('=', 60));

            // Test advanced notification service
            var notificationService = new AdvancedNotificationService("Test Service");
            notificationService.SendNotification("Testing advanced notification with timestamp");
        }

        static void TestExercise8()
        {
            Console.WriteLine("\nExercise 8: Testing LibraryCard Class (Access Modifiers)");
            Console.WriteLine(new string('=', 60));

            // Create and display a library card
            var member = new Member("M003", "Eve Brown", "eve@email.com");
            var card = new LibraryCard("LC001", member);
            card.DisplayCardInfo();
        }

        static void TestExercise9()
        {
            Console.WriteLine("\nExercise 9: Testing BookClass vs BookRecord");
            Console.WriteLine(new string('=', 60));

            // Test record with-expression
            var bookRecord = new BookRecord("ISBN006", "Original Title", "Author Name");
            var modifiedRecord = bookRecord with { Title = "Modified Title" };
            Console.WriteLine("Testing record with-expression:");
            Console.WriteLine($"Original: {bookRecord}");
            Console.WriteLine($"Modified: {modifiedRecord}");
        }

        static void TestExercise10()
        {
            Console.WriteLine("\nExercise 10: Testing Library Events and NotificationService");
            Console.WriteLine(new string('=', 60));

            // Test library events
            var library = new Library("Event Test Library",null);
            var book = new Book("ISBN007", "Event-Driven Design", "Test Author", 2023, 1);
            var member = new Member("M004", "Grace Lee", "grace@email.com");

            library.AddBook(book);
            library.AddMember(member);

            var notificationService = new NotificationService("Event Service");
            notificationService.SubscribeToLibraryEvents(library);

            Console.WriteLine("Testing book borrowing event:");
            library.BorrowBook(book, member);
        }
    }
}
