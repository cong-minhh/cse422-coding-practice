using System;
public class Question2
{
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Input the dividend: ");
            int dividend = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Input the divisor: ");
            int divisor = int.Parse(Console.ReadLine() ?? "0");
            if (dividend == 0)
            {
                Console.WriteLine("Cannot divide by zero");
                return;
            }
            int answer = 0;
            bool isNegative = divisor < 0 || dividend < 0;
            divisor = Math.Abs(divisor);
            dividend = Math.Abs(dividend);
            while (dividend > divisor)
            {
                dividend -= divisor;
                answer++;
            }
            Console.WriteLine("The answer is: " + (isNegative ? -answer : answer));

        }
        catch (FormatException)
        {
            Console.WriteLine("Please input the valid number!");
        }
    }


}