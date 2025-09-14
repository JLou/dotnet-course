using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace generics_linq;

public class Collections
{
    private readonly List<int> _backingList = [1,2,3,4,5,6,7,8];
    
    private IEnumerable<int> Enumerable =>  _backingList;

    private ICollection<int> Collection => _backingList;
    
    private IList<int> List => _backingList;

    public void RunLists()
    {
        foreach (var item in Enumerable)
        {
            Console.WriteLine($"item: {item}");
        }
        
        Console.WriteLine("");

        Console.WriteLine($"count in collection before: {this.Collection.Count}");
        this.Collection.Add(9);
        Console.WriteLine($"count in collection before after: {this.Collection.Count}");
        
        Console.WriteLine("");

        this.List.Add(10);
        Console.WriteLine($"count in list: {this.List.Count}");
        Console.WriteLine($"item at index 3: {this.List[3]}");
        //System.Index
        Console.WriteLine($"item at index ^1: {this.List[^1]}");
        Console.WriteLine($"Index of 10: {List.IndexOf(10)}");

        Console.WriteLine("");

        //System.Range
        var array = _backingList.ToArray();
        Range range = 1..^1;
        var subarray = array[range];
        Console.WriteLine($"length of array for range {range}: {subarray.Length}");
        Console.WriteLine($"content of array for range {range}: {string.Join(", " ,subarray)}");

        range = 5..;
        Console.WriteLine($"content of array for range {range}: {string.Join(", " ,subarray)}");

        int endRange = 5;
        range = ..^endRange;
        Console.WriteLine($"content of array for range {range}: {string.Join(", " ,subarray)}");

    }

    public void RunDictionaries()
    {
        Dictionary<int, string> numberNames =  new Dictionary<int, string>();
        numberNames.Add(0, "zero");
        numberNames.Add(1, "one");
        numberNames.Add(2, "two");
        numberNames.Add(3, "three");
        numberNames.Add(4, "four");
        numberNames.Add(5, "five");
        numberNames.Add(6, "six");
        numberNames.Add(7, "seven");
        numberNames.Add(8, "eight");
        numberNames.Add(9, "nine");
        numberNames.Add(10, "ten");
        
        Console.WriteLine($"number names count: {numberNames.Count}");
        Console.WriteLine($"name of 3: {numberNames[3]}");
    }

    public void RunSets()
    {
        HashSet<int> odds = new HashSet<int>();
        HashSet<int> evens = [];
        // Collection initializer
        HashSet<int> primes = [2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47];
        for (int i = 1; i < 50; i+= 2)
        {
            odds.Add(i);
            evens.Add(i-1);
        }
        
        Console.WriteLine($"odds contains 4: {odds.Contains(4)}");
        Console.WriteLine($"evens contains 4: {evens.Contains(4)}");
        
        Console.WriteLine($"odds overlaps primes: {odds.Overlaps(primes)}");
        Console.WriteLine($"odds overlaps evens: {odds.Overlaps(evens)}");
        
        odds.IntersectWith(primes);
        Console.WriteLine($"intersection of odds and prime is: {{ {string.Join(", ", odds)} }}" );
    }
}