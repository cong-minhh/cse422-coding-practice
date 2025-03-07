using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Lab6.Models;
using Lab6.Repositories;
using Lab6.Controllers;
using Lab6.Payments;
using Lab6.Services;

class Program
{
    static void Main()
    {
        // Test Exercise 1
        Console.WriteLine("Testing Exercise 1:");
        var logger = new Logger();
        logger.Log(new { Username = "Alice", Action = "Login" });
        logger.Log(new { TransactionId = 123, Amount = 49.99 });

        // Test Exercise 2
        // Note: We're skipping the actual database connection for this demo
        Console.WriteLine("\nTesting Exercise 2 (Simulated):");
        Console.WriteLine("Student ID: 1, Name: John Doe");
        Console.WriteLine("Student ID: 2, Name: Jane Smith");
        Console.WriteLine("Teacher ID: 1, Name: Professor Johnson");
        Console.WriteLine("Teacher ID: 2, Name: Dr. Williams");

        // Test Exercise 3
        Console.WriteLine("\nTesting Exercise 3:");
        var studentController = new StudentController();
        var validStudent = new Student { Id = 1, Name = "John" };
        var studentResult = studentController.Create(validStudent);
        Console.WriteLine(studentResult);

        var teacherController = new TeacherController();
        var validTeacher = new Teacher { Id = 1, Name = "Professor Smith" };
        var teacherResult = teacherController.Create(validTeacher);
        Console.WriteLine(teacherResult);

        // Test Exercise 4
        Console.WriteLine("\nTesting Exercise 4:");
        var paymentService = new PaymentService();
        paymentService.ProcessPayment("CreditCard", 100.50);
        paymentService.ProcessPayment("PayPal", 75.25);
        paymentService.ProcessPayment("Crypto", 200.00);

        // Test Exercise 5
        Console.WriteLine("\nTesting Exercise 5:");
        IDataService service = new ProductService();
        IDataService cachedService = new CacheDecorator(service);
        
        Console.WriteLine("First call (not cached):");
        Console.WriteLine(cachedService.GetData(1));
        
        Console.WriteLine("Second call (cached):");
        Console.WriteLine(cachedService.GetData(1));
        
        Console.WriteLine("Different ID (not cached):");
        Console.WriteLine(cachedService.GetData(2));
    }
}