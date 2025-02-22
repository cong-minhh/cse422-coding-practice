using Services;

class Program
{
    static void Main(string[] args)
    {
        // Initialize repositories and services
        var bookRepository = new BookRepository();
        var readerRepository = new ReaderRepository();
        var libraryService = new LibraryService(bookRepository, readerRepository);
        var reportService = new ReportService(readerRepository);

        // Add sample books
        Console.WriteLine("1. Adding sample books...");
        AddSampleBooks(libraryService);

        // Add sample readers
        Console.WriteLine("\n2. Adding sample readers...");
        AddSampleReaders(readerRepository);

        // Demonstrate search functionality
        Console.WriteLine("\n3. Testing search functionality...");
        TestSearchFunctionality(libraryService);

        // Demonstrate lending functionality
        Console.WriteLine("\n4. Testing lending functionality...");
        TestLendingFunctionality(libraryService, readerRepository);

        // Generate and display report
        Console.WriteLine("\n5. Generating borrowing report...");
        DisplayBorrowingReport(reportService);

        // Demonstrate system constraints
        Console.WriteLine("\n6. Testing system constraints...");
        TestSystemConstraints(libraryService, readerRepository);
    }

    private static void AddSampleBooks(ILibraryService libraryService)
    {
        libraryService.AddBook("The Great Gatsby", "F. Scott Fitzgerald", "Fiction", 3);
        libraryService.AddBook("Clean Code", "Robert C. Martin", "Technical", 2);
        libraryService.AddBook("Design Patterns", "Gang of Four", "Technical", 4);
        libraryService.AddBook("Pride and Prejudice", "Jane Austen", "Fiction", 3);
        Console.WriteLine("Sample books added successfully.");
    }

    private static void AddSampleReaders(IReaderRepository readerRepository)
    {
        var readers = new[]
        {
            new Reader("John Doe"),
            new Reader("Jane Smith"),
            new Reader("Bob Wilson")
        };

        foreach (var reader in readers)
        {
            readerRepository.AddReader(reader);
            Console.WriteLine($"Added reader: {reader.Name} (ID: {reader.Id})");
        }
    }

    private static void TestSearchFunctionality(ILibraryService libraryService)
    {
        // Search by category
        Console.WriteLine("\nSearching for Technical books:");
        var technicalBooks = libraryService.SearchBooks("Technical", SearchType.Category);
        foreach (var book in technicalBooks)
        {
            Console.WriteLine($"Found: {book.Title} by {book.Author}");
        }

        // Search by title
        Console.WriteLine("\nSearching for books containing 'Great':");
        var booksWithGreat = libraryService.SearchBooks("Great", SearchType.Title);
        foreach (var book in booksWithGreat)
        {
            Console.WriteLine($"Found: {book.Title} by {book.Author}");
        }
    }

    private static void TestLendingFunctionality(ILibraryService libraryService, IReaderRepository readerRepository)
    {
        try
        {
            // Get first reader and book for testing
            var reader = readerRepository.GetAllReaders().First();
            var books = libraryService.SearchBooks("Technical", SearchType.Category).ToList();

            // Borrow books
            Console.WriteLine($"\nReader {reader.Name} borrowing books:");
            foreach (var book in books.Take(2))
            {
                libraryService.LendBook(reader.Id, book.Id);
                Console.WriteLine($"Borrowed: {book.Title}");
            }

            // Return a book
            Console.WriteLine("\nReturning a book:");
            libraryService.ReturnBook(reader.Id, books[0].Id);
            Console.WriteLine($"Returned: {books[0].Title}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during lending test: {ex.Message}");
        }
    }

    private static void DisplayBorrowingReport(IReportService reportService)
    {
        var report = reportService.GenerateBorrowingReport();
        Console.WriteLine(report);
    }

    private static void TestSystemConstraints(ILibraryService libraryService, IReaderRepository readerRepository)
    {
        try
        {
            var reader = readerRepository.GetAllReaders().First();
            var books = libraryService.SearchBooks("Fiction", SearchType.Category).ToList();

            Console.WriteLine("\nTesting maximum books constraint (3 books):");
            // Try to borrow more than 3 books
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    libraryService.LendBook(reader.Id, books[i].Id);
                    Console.WriteLine($"Borrowed book {i + 1}: {books[i].Title}");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Expected error: {ex.Message}");
                }
            }

            // Test borrowing unavailable books
            Console.WriteLine("\nTesting book availability constraint:");
            var anotherReader = readerRepository.GetAllReaders().Skip(1).First();
            try
            {
                // Try to borrow a book that's already fully borrowed
                libraryService.LendBook(anotherReader.Id, books[0].Id);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Expected error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during constraints test: {ex.Message}");
        }
    }
}