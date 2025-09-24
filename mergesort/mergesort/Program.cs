using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

// logika je z mojí hlavy, meznám syntax c#, tam jsem si nechal pomoct
unsafe void mergesort(IntPtr listToSort, int lenght)
{
    if (lenght < 2)
    {
        return;
    }

    int lenght1 = lenght / 2;
    int lenght2 = lenght - lenght1;

    IntPtr ptr1 = Marshal.AllocHGlobal(lenght1 * sizeof(int));
    IntPtr ptr2 = Marshal.AllocHGlobal(lenght2 * sizeof(int));

    // rozdělení do dvou listů

    Buffer.MemoryCopy(listToSort.ToPointer(), ptr1.ToPointer(), lenght1 * sizeof(int), lenght1 * sizeof(int));

    IntPtr sourceOffset = IntPtr.Add(listToSort, lenght1 * sizeof(int));
    Buffer.MemoryCopy(sourceOffset.ToPointer(), ptr2.ToPointer(), lenght2 * sizeof(int), lenght2 * sizeof(int));

    mergesort(ptr1, lenght1);
    mergesort(ptr2, lenght2);

    int position1 = 0;
    int position2 = 0;

    int* p1 = (int*)ptr1.ToPointer();
    int* p2 = (int*)ptr2.ToPointer();
    int* dest = (int*)listToSort.ToPointer();

    for (int i = 0; i < lenght; i++)
    {
        if (position1 < lenght1 && (position2 >= lenght2 || p1[position1] < p2[position2]))
        {
            dest[i] = p1[position1];
            position1++;
        }
        else
        {
            dest[i] = p2[position2];
            position2++;
        }
    }

    Marshal.FreeHGlobal(ptr1);
    Marshal.FreeHGlobal(ptr2);
}

// vygenerováno s "udělej test s čím dál většími listy"

unsafe void TestMergeSortWithIncreasingSize()
{
    Random random = new Random(42); // Fixní seed pro reprodukovatelnost

    // Velikosti polí pro testování
    int[] sizes = { 10, 100, 1000, 5000, 10000, 50000, 100000, 500000, 1000000, 5000000, 10000000, 50000000, 100000000};

    Console.WriteLine("=== Test s postupně se zvětšujícími poli ===\n");
    Console.WriteLine($"{"Velikost",-10} {"Čas (ms)",-12} {"Rychlost (prvků/ms)",-20} {"Status",-15}");
    Console.WriteLine(new string('-', 65));

    foreach (int size in sizes)
    {
        try
        {
            // Vytvoření náhodného pole
            int[] testArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                testArray[i] = random.Next(1, 10000);
            }

            // Alokace paměti pro unsafe operace
            IntPtr ptr = Marshal.AllocHGlobal(size * sizeof(int));
            Marshal.Copy(testArray, 0, ptr, size);

            // Měření času
            Stopwatch stopwatch = Stopwatch.StartNew();
            mergesort(ptr, size);
            stopwatch.Stop();

            // Kopírování výsledku zpět
            int[] sortedArray = new int[size];
            Marshal.Copy(ptr, sortedArray, 0, size);

            // Ověření správnosti řazení
            bool isSorted = true;
            for (int i = 1; i < size; i++)
            {
                if (sortedArray[i] < sortedArray[i - 1])
                {
                    isSorted = false;
                    break;
                }
            }

            // Výpočet rychlosti
            double elementsPerMs = stopwatch.ElapsedMilliseconds > 0
                ? size / (double)stopwatch.ElapsedMilliseconds
                : size;

            string status = isSorted ? "✓ Správně" : "✗ Chyba";

            Console.WriteLine($"{size,-10} {stopwatch.ElapsedMilliseconds,-12} {elementsPerMs,-20:F2} {status,-15}");

            // Uvolnění paměti
            Marshal.FreeHGlobal(ptr);

            // Pro menší pole zobrazíme také obsah
            if (size <= 20)
            {
                Console.WriteLine($"   Původní: [{string.Join(", ", testArray.Take(10))}{(size > 10 ? "..." : "")}]");
                Console.WriteLine($"   Seřazené: [{string.Join(", ", sortedArray.Take(10))}{(size > 10 ? "..." : "")}]");
                Console.WriteLine();
            }

            if (!isSorted)
            {
                Console.WriteLine("   CHYBA: Pole není správně seřazeno!");
                break;
            }

        }
        catch (OutOfMemoryException)
        {
            Console.WriteLine($"{size,-10} {"N/A",-12} {"N/A",-20} {"✗ Nedostatek paměti",-15}");
            break;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{size,-10} {"N/A",-12} {"N/A",-20} {"✗ Chyba: " + ex.GetType().Name,-15}");
            break;
        }
    }
}

TestMergeSortWithIncreasingSize();