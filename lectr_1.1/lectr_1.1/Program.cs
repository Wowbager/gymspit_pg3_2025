using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Starting memory stress test...");
        try
        {
            int result = Nuke(1);
            Console.WriteLine($"Completed with result: {result}");
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine("Out of memory - test successful!");
        }
        catch (StackOverflowException)
        {
            Console.WriteLine("Stack overflow - recursion limit reached!");
        }
    }

    static int Nuke(ulong depth)
    {
        // Safety limit to prevent infinite recursion
        if (depth > 20)
        {
            Console.WriteLine($"Maximum depth {depth} reached");
            return 0;
        }

        Console.WriteLine($"Depth {depth}: Allocating array of size {depth}");
        
        try
        {
            ulong[] tmp = new ulong[depth];
            
            for (ulong j = 0; j < depth; j++)
            {
                tmp[j] = j;
                // Recursive call - this will increase memory usage
                Nuke(depth + 1);
            }
        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine($"Out of memory at depth {depth}");
            throw;
        }
        
        return 0;
    }
}