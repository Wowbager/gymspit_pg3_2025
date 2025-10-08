using System.Diagnostics;

bool valid_input = false;
ulong size;
uint chunk_size = 1000000000;

do
{
    Console.WriteLine("Enter ulong: ");
    if (ulong.TryParse(Console.ReadLine(), out size))
    {
        valid_input = true;
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
    }
} while (!valid_input);

Console.WriteLine($"\nStarting calculation for numbers up to {size:N0}...");
Console.WriteLine($"Using chunk size: {chunk_size:N0}\n");

var stopwatch = Stopwatch.StartNew();

(ulong count, ulong largest) = find_primes(size, chunk_size);

stopwatch.Stop();

Console.WriteLine("\n" + new string('=', 50));
Console.WriteLine($"Calculation completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds");
Console.WriteLine($"Number of primes: {count:N0}");
Console.WriteLine($"Largest prime: {largest:N0}");
Console.WriteLine(new string('=', 50));

static (ulong count, ulong largest) find_primes(ulong size, uint chunk_size)
{
    ulong count = 0;
    ulong largest = 0;
    
    // Potřebujeme pouze prvočísla do sqrt(size) pro síto
    ulong sqrt_size = (ulong)Math.Sqrt(size) + 1;
    List<ulong> small_primes = new List<ulong>();
    
    ulong num_chunks = (size + chunk_size - 1) / chunk_size;
    
    Console.WriteLine($"Processing {num_chunks} chunk(s)...\n");
    
    var chunkStopwatch = Stopwatch.StartNew();
    double timeComplexityConstant = 0; // Konstanta pro výpočet složitosti
    
    for (ulong chunk_idx = 0; chunk_idx < num_chunks; chunk_idx++)
    {
        ulong chunk_start = chunk_idx * chunk_size;
        ulong chunk_end = Math.Min(chunk_start + chunk_size, size);
        uint current_chunk_size = (uint)(chunk_end - chunk_start);
        
        // Zobrazit průběh
        Console.Write($"\rChunk {chunk_idx + 1}/{num_chunks} ({(chunk_idx + 1) * 100.0 / num_chunks:F1}%) - ");
        Console.Write($"Range: {chunk_start:N0} to {chunk_end:N0}");
        
        bool[] sieve = new bool[current_chunk_size];
        
        // Označit násobky malých prvočísel z předchozích chunků
        foreach (var prime in small_primes)
        {
            ulong first_multiple = ((chunk_start + prime - 1) / prime) * prime;
            if (first_multiple < prime * prime) first_multiple = prime * prime;
            
            for (ulong j = first_multiple; j < chunk_end; j += prime)
            {
                sieve[j - chunk_start] = true;
            }
        }
        
        // Projít chunk a najít prvočísla
        ulong chunk_primes = 0;
        for (ulong i = Math.Max(2, chunk_start); i < chunk_end; i++)
        {
            ulong local_idx = i - chunk_start;
            
            if (!sieve[local_idx])
            {
                count++;
                chunk_primes++;
                largest = i;
                
                // Uložit pouze prvočísla potřebná pro síto (do sqrt(size))
                if (i <= sqrt_size)
                {
                    small_primes.Add(i);
                    
                    // Označit násobky v tomto chunku
                    for (ulong j = i * i; j < chunk_end; j += i)
                    {
                        if (j >= chunk_start)
                        {
                            sieve[j - chunk_start] = true;
                        }
                    }
                }
            }
        }
        
        // Zobrazit statistiky pro chunk
        Console.Write($" - Found: {chunk_primes:N0} primes");
        
        // Zobrazit odhadovaný zbývající čas s přesnějším výpočtem
        if (chunk_idx > 0)
        {
            // Výpočet časové složitosti pro zpracované chunky
            double processedComplexity = CalculateComplexity(chunk_end);
            double elapsedSeconds = chunkStopwatch.Elapsed.TotalSeconds;
            
            // Odhadnout konstantu složitosti na základě dosavadního výkonu
            timeComplexityConstant = elapsedSeconds / processedComplexity;
            
            // Vypočítat zbývající složitost
            double totalComplexity = CalculateComplexity(size);
            double remainingComplexity = totalComplexity - processedComplexity;
            
            // Odhadnout zbývající čas
            double estimatedRemaining = timeComplexityConstant * remainingComplexity;
            
            Console.Write($" - ETA: {TimeSpan.FromSeconds(estimatedRemaining):hh\\:mm\\:ss}");
        }
    }
    
    Console.WriteLine(); // Nový řádek po dokončení všech chunků
    
    return (count, largest);
}

// Výpočet časové složitosti O(n log n log log n)
static double CalculateComplexity(ulong n)
{
    if (n <= 2) return 0;
    
    double logN = Math.Log(n);
    double logLogN = Math.Log(logN);
    
    return n * logN * logLogN;
}