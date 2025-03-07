using System;

// For Exercise 4
namespace Lab6.Payments
{
    public interface IPaymentStrategy
    {
        void ProcessPayment(double amount);
    }

    public class CreditCardStrategy : IPaymentStrategy
    {
        public void ProcessPayment(double amount) =>
            Console.WriteLine($"Processing credit card: {amount}");
    }

    public class PayPalStrategy : IPaymentStrategy
    {
        public void ProcessPayment(double amount) =>
            Console.WriteLine($"Processing PayPal: {amount}");
    }

    public class CryptoStrategy : IPaymentStrategy
    {
        public void ProcessPayment(double amount) =>
            Console.WriteLine($"Processing Crypto: {amount}");
    }

    public class PaymentFactory
    {
        public static IPaymentStrategy CreateStrategy(string method)
        {
            return method switch
            {
                "CreditCard" => new CreditCardStrategy(),
                "PayPal" => new PayPalStrategy(),
                "Crypto" => new CryptoStrategy(),
                _ => throw new ArgumentException("Invalid method")
            };
        }
    }

    public class PaymentService
    {
        public void ProcessPayment(string method, double amount)
        {
            var strategy = PaymentFactory.CreateStrategy(method);
            strategy.ProcessPayment(amount);
        }
    }
}