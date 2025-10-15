// generted with https://huggingface.co/spaces/ibm-granite/Granite-4.0-WebGPU

using System;

class Fibonacci
{
    static int FibonacciNumber(int n)
    {
        if (n <= 1)
            return n;

        int a = 0, b = 1, c;
        for (int i = 2; i <= n; i++)
        {
            c = a + b;
            a = b;
            b = c;
        }

        return b;
    }

    static void Main(string[] args)
    {
        Console.Write("Enter the value of n: ");
        int n = int.Parse(Console.ReadLine());

        int result = FibonacciNumber(n);
        Console.WriteLine($"The {n}th Fibonacci number is: {result}");
    }
}