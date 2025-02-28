using System;
using Lab5.Models;

namespace Lab5.Strategies
{
    // Strategy Pattern Implementation
    
    // Strategy interface
    public interface ILoanFeeStrategy
    {
        decimal CalculateFee(Document document, int daysLoaned);
    }
    
    // Concrete Strategies
    public class BookLoanFeeStrategy : ILoanFeeStrategy
    {
        private const decimal BaseRate = 0.10m; // $0.10 per day
        private const int StandardLoanPeriod = 14; // 14 days standard loan period
        private const decimal LateFeeMultiplier = 1.5m; // 50% more for late returns
        
        public decimal CalculateFee(Document document, int daysLoaned)
        {
            if (document is not Book)
                throw new ArgumentException("This strategy can only be used with books");
                
            // Standard fee calculation
            decimal fee = daysLoaned * BaseRate;
            
            // Apply late fee if applicable
            if (daysLoaned > StandardLoanPeriod)
            {
                int lateDays = daysLoaned - StandardLoanPeriod;
                fee += lateDays * BaseRate * LateFeeMultiplier;
            }
            
            return Math.Round(fee, 2);
        }
    }
    
    public class MagazineLoanFeeStrategy : ILoanFeeStrategy
    {
        private const decimal BaseRate = 0.20m; // $0.20 per day
        private const int StandardLoanPeriod = 7; // 7 days standard loan period
        private const decimal LateFeeMultiplier = 2.0m; // Double for late returns
        
        public decimal CalculateFee(Document document, int daysLoaned)
        {
            if (document is not Magazine)
                throw new ArgumentException("This strategy can only be used with magazines");
                
            // Standard fee calculation
            decimal fee = daysLoaned * BaseRate;
            
            // Apply late fee if applicable
            if (daysLoaned > StandardLoanPeriod)
            {
                int lateDays = daysLoaned - StandardLoanPeriod;
                fee += lateDays * BaseRate * LateFeeMultiplier;
            }
            
            return Math.Round(fee, 2);
        }
    }
    
    public class NewspaperLoanFeeStrategy : ILoanFeeStrategy
    {
        private const decimal BaseRate = 0.30m; // $0.30 per day
        private const int StandardLoanPeriod = 2; // 2 days standard loan period
        private const decimal LateFeeMultiplier = 3.0m; // Triple for late returns
        
        public decimal CalculateFee(Document document, int daysLoaned)
        {
            if (document is not Newspaper)
                throw new ArgumentException("This strategy can only be used with newspapers");
                
            // Standard fee calculation
            decimal fee = daysLoaned * BaseRate;
            
            // Apply late fee if applicable
            if (daysLoaned > StandardLoanPeriod)
            {
                int lateDays = daysLoaned - StandardLoanPeriod;
                fee += lateDays * BaseRate * LateFeeMultiplier;
            }
            
            return Math.Round(fee, 2);
        }
    }
    
    // Context class that uses the strategy
    public class LoanFeeCalculator
    {
        private ILoanFeeStrategy _strategy;
        
        public LoanFeeCalculator(ILoanFeeStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public void SetStrategy(ILoanFeeStrategy strategy)
        {
            _strategy = strategy;
        }
        
        public decimal CalculateFee(Document document, int daysLoaned)
        {
            return _strategy.CalculateFee(document, daysLoaned);
        }
        
        // Factory method to get the appropriate strategy based on document type
        public static ILoanFeeStrategy GetStrategyForDocument(Document document)
        {
            return document.GetDocumentType() switch
            {
                "Book" => new BookLoanFeeStrategy(),
                "Magazine" => new MagazineLoanFeeStrategy(),
                "Newspaper" => new NewspaperLoanFeeStrategy(),
                _ => throw new ArgumentException($"No strategy available for document type: {document.GetDocumentType()}")
            };
        }
    }
}