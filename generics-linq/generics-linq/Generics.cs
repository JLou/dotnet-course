namespace generics_linq;

file class LinkedListNoGeneric
{
    
    private Node? _head;

    public void AddHead(object t)
    {
        Node n = new(t)
        {
            Next = _head
        };
        _head = n;
    }
    
    public Node? GetHead()
    {
        return _head;
    }
    
}

file class Node(object t)
{
    public object Data { get; } = t;

    public Node? Next { get; init; }
}


public static class LinkedListNoGenericUsage
{
    public static void Run()
    {
        var  list = new LinkedListNoGeneric();
        for (int i = 0; i < 10; i++)
        {
            list.AddHead(i);
        }

        var current = list.GetHead();
        while (current != null)
        {
            Console.WriteLine((int)current.Data * 2);
            current = current.Next;
        }
        
        
    }
}

/// Erreur de compilation si on ajoute autre chose que des ints
/// Ajouter une string a la liste
/// Transformer en generic