using System;

string fizzBuzz(uint number)
{
    string output = "";

    if (number % 3 == 0)
    {
        output += "Fizz";
    }

    if (number % 5 == 0)
    {
        output += "Buzz";
    }

    if (output == "")
    {
        output = number.ToString();
    }

    return output;
}

uint repetitions = 0;

int incorectTries = 0;

Console.WriteLine("Napište počet opakování (kladné číslo): ");

while (!uint.TryParse(Console.ReadLine(), out repetitions) || repetitions == 0)
{
    incorectTries++;

    if (incorectTries == 5)
    {
        Console.WriteLine("příliš mnoho nesprávných pokusů!");
        System.Exception ex = new System.Exception("too many tries");

        Console.WriteLine(ex.Message);
        Environment.Exit(1);
    }

    Console.WriteLine("Input neodpovídá požadovanému formátu (zkuste to znovu)");
}
for (uint i = 1; i <= repetitions; i++)
{
    Console.WriteLine(fizzBuzz(i));
}