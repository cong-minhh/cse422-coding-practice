using System;
using System.Collections.Generic;
using Lab5.Database;
using Lab5.Factories;
using Lab5.Models;
using Lab5.Observers;
using Lab5.Strategies;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Library Management System Demo\n");
            
            // Demonstrate Singleton Pattern
            Console.WriteLine("===== Singleton Pattern Demo =====");
            DatabaseConnection db1 = DatabaseConnection.Instance;
            DatabaseConnection db2 = DatabaseConnection.Instance;
            
            Console.WriteLine($"Are both database connections the same instance? {ReferenceEquals(db1, db2)}");
            db1.OpenConnection();
            db1.ExecuteQuery("SELECT * FROM Documents WHERE DocumentType = 'Book'");
            db1.CloseConnection();
            Console.WriteLine();
            
            // Demonstrate Factory Pattern
            Console.WriteLine("===== Factory Pattern Demo =====");
            // Create documents using factories
            DocumentFactory bookFactory = new BookFactory(
                "Design Patterns", "Gang of Four", new DateTime(1994, 10, 21), 
                "978-0201633610", 416);
                
            DocumentFactory magazineFactory = new MagazineFactory(
                "Scientific American", "Various Authors", DateTime.Now.AddMonths(-1), 
                "Vol 326, No 5", "Springer Nature");
                
            DocumentFactory newspaperFactory = new NewspaperFactory(
                "The Daily News", "Editorial Team", DateTime.Today, 
                "Morning Edition");
            
            Document book = bookFactory.GetDocument();
            Document magazine = magazineFactory.GetDocument();
            Document newspaper = newspaperFactory.GetDocument();
            
            Console.WriteLine($"Created {book.GetDocumentType()}: {book.Title} by {book.Author}");
            Console.WriteLine($"Created {magazine.GetDocumentType()}: {magazine.Title} by {magazine.Author}");
            Console.WriteLine($"Created {newspaper.GetDocumentType()}: {newspaper.Title} by {newspaper.Author}");
            Console.WriteLine();
            
            // Demonstrate Observer Pattern
            Console.WriteLine("===== Observer Pattern Demo =====");
            // Create library manager (subject)
            LibraryManager library = new LibraryManager();
            
            // Create users (observers)
            User alice = new User(1, "Alice", "alice@example.com");
            User bob = new User(2, "Bob", "bob@example.com");
            
            // Register observers
            library.RegisterObserver(alice);
            library.RegisterObserver(bob);
            
            // Set user interests
            alice.AddInterest(book);
            bob.AddInterest(magazine);
            
            // Perform library operations that trigger notifications
            Console.WriteLine("Adding documents to library:");
            library.AddDocument(book);
            library.AddDocument(magazine);
            library.AddDocument(newspaper);
            
            Console.WriteLine("\nBorrowing and returning documents:");
            library.BorrowDocument(book, bob);      // Alice will be notified
            library.BorrowDocument(magazine, alice); // Bob will be notified
            library.ReturnDocument(book, bob);      // Alice will be notified
            Console.WriteLine();
            
            // Demonstrate Strategy Pattern
            Console.WriteLine("===== Strategy Pattern Demo =====");
            // Create loan fee calculator with different strategies
            
            // Using the factory method to get appropriate strategies
            ILoanFeeStrategy bookStrategy = LoanFeeCalculator.GetStrategyForDocument(book);
            ILoanFeeStrategy magazineStrategy = LoanFeeCalculator.GetStrategyForDocument(magazine);
            ILoanFeeStrategy newspaperStrategy = LoanFeeCalculator.GetStrategyForDocument(newspaper);
            
            // Calculate fees for different scenarios
            LoanFeeCalculator calculator = new LoanFeeCalculator(bookStrategy);
            
            // Book borrowed for standard period
            Console.WriteLine($"Book fee for 14 days: ${calculator.CalculateFee(book, 14)}");
            
            // Book borrowed for extended period
            Console.WriteLine($"Book fee for 21 days (7 days late): ${calculator.CalculateFee(book, 21)}");
            
            // Change strategy to magazine
            calculator.SetStrategy(magazineStrategy);
            Console.WriteLine($"Magazine fee for 7 days: ${calculator.CalculateFee(magazine, 7)}");
            Console.WriteLine($"Magazine fee for 10 days (3 days late): ${calculator.CalculateFee(magazine, 10)}");
            
            // Change strategy to newspaper
            calculator.SetStrategy(newspaperStrategy);
            Console.WriteLine($"Newspaper fee for 2 days: ${calculator.CalculateFee(newspaper, 2)}");
            Console.WriteLine($"Newspaper fee for 5 days (3 days late): ${calculator.CalculateFee(newspaper, 5)}");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
