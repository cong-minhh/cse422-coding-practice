using System;
using System.Linq;
using System.Reflection;

public class Logger
{
    public void Log(object data)
    {
        var type = data.GetType();
        var properties = type.GetProperties();
        var message = string.Join(", ", properties.Select(p => $"{p.Name}={p.GetValue(data)}"));
        Console.WriteLine($"LOG: {message}");
    }
}


