using System;

public class MedianCalculator
{
    public static void Main()
    {
        var calculator = new MedianCalculator();
        calculator.Run();
    }

    private void Run()
    {
        try
        {
            var (array1, array2) = GetInputArrays();
            var median = CalculateMedianOfSortedArrays(array1, array2);
            DisplayResult(median);
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    private (int[] first, int[] second) GetInputArrays()
    {
        const int MaxTotalLength = 2000;
        const int MinTotalLength = 1;

        var length1 = ReadArrayLength("first");
        var length2 = ReadArrayLength("second");

        ValidateTotalLength(length1 + length2, MinTotalLength, MaxTotalLength);

        var array1 = ReadArrayElements("first", length1);
        var array2 = ReadArrayElements("second", length2);

        return (array1, array2);
    }

    private int ReadArrayLength(string arrayName)
    {
        Console.WriteLine($"Enter the length of {arrayName} array:");
        return int.Parse(Console.ReadLine() ?? "0");
    }

    private void ValidateTotalLength(int totalLength, int min, int max)
    {
        if (totalLength < min || totalLength > max)
        {
            throw new ArgumentException($"Combined length must be between {min} and {max}");
        }
    }

    private int[] ReadArrayElements(string arrayName, int length)
    {
        Console.WriteLine($"Enter {length} space-separated numbers for {arrayName} array:");
        var input = (Console.ReadLine() ?? string.Empty).Split(' ');

        if (input.Length != length)
        {
            throw new ArgumentException($"Expected {length} numbers, received {input.Length}");
        }

        return ParseAndValidateNumbers(input);
    }

    private int[] ParseAndValidateNumbers(string[] inputs)
    {
        const int ValueLimit = 1000000;
        var result = new int[inputs.Length];

        for (var i = 0; i < inputs.Length; i++)
        {
            if (!int.TryParse(inputs[i], out result[i]))
            {
                throw new FormatException($"Invalid number at position {i + 1}");
            }

            if (Math.Abs(result[i]) > ValueLimit)
            {
                throw new ArgumentException($"Number at position {i + 1} exceeds ±{ValueLimit}");
            }
        }

        return result;
    }

    private double CalculateMedianOfSortedArrays(int[] first, int[] second)
    {
        if (first.Length > second.Length)
        {
            return CalculateMedianOfSortedArrays(second, first);
        }

        var low = 0;
        var high = first.Length;
        var combinedLength = first.Length + second.Length;

        while (low <= high)
        {
            var partitionFirst = (low + high) / 2;
            var partitionSecond = (combinedLength + 1) / 2 - partitionFirst;

            var leftFirst = GetPartitionValue(first, partitionFirst - 1, int.MinValue);
            var rightFirst = GetPartitionValue(first, partitionFirst, int.MaxValue);
            var leftSecond = GetPartitionValue(second, partitionSecond - 1, int.MinValue);
            var rightSecond = GetPartitionValue(second, partitionSecond, int.MaxValue);

            if (leftFirst <= rightSecond && leftSecond <= rightFirst)
            {
                if (combinedLength % 2 == 0)
                {
                    return (Math.Max(leftFirst, leftSecond) +
                           Math.Min(rightFirst, rightSecond)) / 2.0;
                }
                return Math.Max(leftFirst, leftSecond);
            }

            if (leftFirst > rightSecond)
            {
                high = partitionFirst - 1;
            }
            else
            {
                low = partitionFirst + 1;
            }
        }

        throw new ArgumentException("Arrays are not properly sorted");
    }

    private int GetPartitionValue(int[] array, int index, int defaultValue)
    {
        return (index >= 0 && index < array.Length) ? array[index] : defaultValue;
    }

    private void DisplayResult(double median)
    {
        Console.WriteLine($"Median: {median:F5}");
    }

    private void HandleError(Exception ex)
    {
        var message = ex switch
        {
            FormatException => "Please enter valid numbers.",
            ArgumentException => ex.Message,
            _ => "An unexpected error occurred."
        };
        Console.WriteLine($"Error: {message}");
    }
}