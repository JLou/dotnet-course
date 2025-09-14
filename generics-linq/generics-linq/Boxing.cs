namespace generics_linq;

public class Boxing
{
    public static void Example()
    {
        int valueType = 123;

        object boxedObject = valueType;
        
        Console.WriteLine(boxedObject);
        Console.WriteLine(boxedObject.GetType().FullName);
        Console.WriteLine((int)boxedObject * 2);
        
    }
}

//Voir l'IL box/unbox