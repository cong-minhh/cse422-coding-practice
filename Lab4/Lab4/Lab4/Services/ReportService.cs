using System.Text;
using System.Linq;

public class ReportService : IReportService
{
    private readonly IReaderRepository _readerRepository;

    public ReportService(IReaderRepository readerRepository)
    {
        _readerRepository = readerRepository;
    }

    public string GenerateBorrowingReport()
    {
        var report = new StringBuilder();
        report.AppendLine("Library Borrowing Report");
        report.AppendLine("=======================");

        foreach (var reader in _readerRepository.GetAllReaders())
        {
            report.AppendLine($"\nReader: {reader.Name} (ID: {reader.Id})");
            var borrowedBooks = reader.GetBorrowedBooks();
            
            if (!borrowedBooks.Any())
            {
                report.AppendLine("No books borrowed");
                continue;
            }

            report.AppendLine("Borrowed Books:");
            foreach (var book in borrowedBooks)
            {
                report.AppendLine($"- {book.Title} by {book.Author}");
            }
        }

        return report.ToString();
    }
} 