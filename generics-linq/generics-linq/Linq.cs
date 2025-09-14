namespace generics_linq;

public class Linq
{
    public static IEnumerable<string> GetDays()
    {
        Console.WriteLine("returning monday");
        yield return "Monday";
        Console.WriteLine("returning Tuesday");
        yield return "Tuesday";
        Console.WriteLine("returning Wednesday");
        yield return "Wednesday";
        Console.WriteLine("returning Thursday");
        yield return "Thursday";
        Console.WriteLine("returning Friday");
        yield return "Friday";
        Console.WriteLine("returning Saturday");
        yield return "Saturday";
        Console.WriteLine("returning Sunday");
        yield return "Sunday";
    }

    public static void Run()
    {
        var days = GetDays();
        Console.WriteLine($"First: {days.First()}");
        
        Console.WriteLine();
        
        Console.WriteLine($"Last: {days.Last()}");
        Console.WriteLine();
        
        Console.WriteLine($"Count: {days.Count()}");
        Console.WriteLine();
        
        Console.WriteLine($"Take 3: {days.Take(3).PrintCollection()}");
        Console.WriteLine();

        Console.WriteLine($"Skip 3: {days.Skip(3).PrintCollection()}");
        Console.WriteLine();

        Console.WriteLine($"Select: {days.Select(day => day.ToUpper()).PrintCollection()}");
        Console.WriteLine();

        Console.WriteLine($"Where: {days.Where(day => day.Contains('s', StringComparison.InvariantCultureIgnoreCase)).PrintCollection()}");
        Console.WriteLine();
        
        

    }
}

public static class Extensions
{
    public static string PrintCollection<T>(this IEnumerable<T> collection)
    {
        return string.Join(", ",  collection);
    }
}