// Program pro práci s uživatelským vstupem
Console.WriteLine("=== Program pro práci s uživatelským vstupem ===");
Console.WriteLine();

// Jednoduchý vstup textu
Console.Write("Zadejte své jméno: ");
string? jmeno = Console.ReadLine();
Console.WriteLine($"Ahoj, {jmeno}!");
Console.WriteLine();

// Vstup číselné hodnoty s ověřením
Console.Write("Zadejte svůj věk: ");
string? vekInput = Console.ReadLine();
if (int.TryParse(vekInput, out int vek))
{
    Console.WriteLine($"Váš věk je {vek} let.");
    if (vek >= 18)
    {
        Console.WriteLine("Jste plnoletý.");
    }
    else
    {
        Console.WriteLine("Jste nezletilý.");
    }
}
else
{
    Console.WriteLine("Neplatný věk!");
}
Console.WriteLine();

// Jednoduchá kalkulačka
Console.WriteLine("=== Jednoduchá kalkulačka ===");
Console.Write("Zadejte první číslo: ");
string? cislo1Input = Console.ReadLine();
Console.Write("Zadejte druhé číslo: ");
string? cislo2Input = Console.ReadLine();

if (double.TryParse(cislo1Input, out double cislo1) && 
    double.TryParse(cislo2Input, out double cislo2))
{
    Console.WriteLine("Vyberte operaci:");
    Console.WriteLine("1. Sčítání (+)");
    Console.WriteLine("2. Odčítání (-)");
    Console.WriteLine("3. Násobení (*)");
    Console.WriteLine("4. Dělení (/)");
    Console.Write("Vaše volba (1-4): ");
    
    string? volba = Console.ReadLine();
    double vysledek = 0;
    string operace = "";
    bool validniOperace = true;
    
    switch (volba)
    {
        case "1":
            vysledek = cislo1 + cislo2;
            operace = "+";
            break;
        case "2":
            vysledek = cislo1 - cislo2;
            operace = "-";
            break;
        case "3":
            vysledek = cislo1 * cislo2;
            operace = "*";
            break;
        case "4":
            if (cislo2 != 0)
            {
                vysledek = cislo1 / cislo2;
                operace = "/";
            }
            else
            {
                Console.WriteLine("Chyba: Nelze dělit nulou!");
                validniOperace = false;
            }
            break;
        default:
            Console.WriteLine("Neplatná volba operace!");
            validniOperace = false;
            break;
    }
    
    if (validniOperace)
    {
        Console.WriteLine($"{cislo1} {operace} {cislo2} = {vysledek}");
    }
}
else
{
    Console.WriteLine("Neplatná číselná hodnota!");
}

Console.WriteLine();
Console.WriteLine("Stiskněte libovolnou klávesu pro ukončení...");
Console.ReadKey();
