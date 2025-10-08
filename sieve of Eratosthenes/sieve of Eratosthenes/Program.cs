bool valid_input = false;
uint size;

do
{
    Console.WriteLine("Enter uint: ");
    if (uint.TryParse(Console.ReadLine(), out size))
    {
        valid_input = true;
    }
} while (!valid_input);

size++;

LinkedList<uint> primes = find_primes(size);

Console.WriteLine($"number of primes {primes.Count}");
Console.WriteLine($"largest prime: {primes.Last()}");

static LinkedList<uint> find_primes(uint size)
{
    LinkedList<uint> list = new LinkedList<uint>();

    bool[] sieve = new bool[size];

    for (uint i = 2; i < size; i++)
    {
        if (!sieve[i])
        {
            list.AddLast(i);
            for (uint j = i * i; j < size; j += i)
            {
                sieve[j] = true;
            }
        }
    }

    return list;
}