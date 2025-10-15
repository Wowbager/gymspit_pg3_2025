using System;

string[] students = new string[1];

string userInput;
int userIntent;

while (true)
{
    Console.Clear();
    Console.WriteLine("vyberte si jednu možnost:");
    Console.WriteLine("1: přidat studenta");
    Console.WriteLine("2: vypsat všechny studenty");
    Console.WriteLine("3: vyhledat studenta");
    Console.WriteLine("4: odstranění studenta");
    Console.WriteLine("5: spočítat studenty");
    Console.WriteLine("pro ukončení stikněte enter");
    Console.Write("vaše volba: ");

    userInput = Console.ReadLine();

    if (userInput == "")
    {
        break;
    }

    if (!int.TryParse(userInput, out userIntent)) 
    {
       continue;
    }

    if (userIntent < 1 && userIntent > 5)
    {
        continue;
    }

    switch (userIntent)
    {
        case 1:
            AddUser(out students);
            break;
        case 2:
            PrintAllStudents(students);
            break;
        case 3:
            FindUser(students);
            break;
        case 4:
            RemoveUser(out students);
            break;
        case 5:
            CountUsers(students);
            break;
    }
}

void AddUser(out string[] studentArray)
{
    studentArray = students;
    Console.Clear();
    Console.Write("Zadejte jméno studenta: ");
    string newStudent = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(newStudent))
    {
        Console.WriteLine("Jméno nesmí být prázdné!");
        Console.WriteLine("Stiskněte Enter pro pokračování...");
        Console.ReadLine();
        return;
    }
    
    // Find first empty slot or resize array
    bool added = false;
    for (int i = 0; i < studentArray.Length; i++)
    {
        if (string.IsNullOrEmpty(studentArray[i]))
        {
            studentArray[i] = newStudent;
            added = true;
            Console.WriteLine($"Student {newStudent} byl přidán.");
            break;
        }
    }
    
    if (!added)
    {
        // Resize array
        Array.Resize(ref studentArray, studentArray.Length + 1);
        studentArray[studentArray.Length - 1] = newStudent;
        Console.WriteLine($"Student {newStudent} byl přidán.");
    }
    
    students = studentArray;
    Console.WriteLine("Stiskněte Enter pro pokračování...");
    Console.ReadLine();
}

void PrintAllStudents(string[] studentArray)
{
    Console.Clear();
    Console.WriteLine("Seznam všech studentů:");
    Console.WriteLine("=====================");
    
    int count = 0;
    for (int i = 0; i < studentArray.Length; i++)
    {
        if (!string.IsNullOrEmpty(studentArray[i]))
        {
            Console.WriteLine($"{i + 1}. {studentArray[i]}");
            count++;
        }
    }
    
    if (count == 0)
    {
        Console.WriteLine("Žádní studenti nejsou v seznamu.");
    }
    
    Console.WriteLine("=====================");
    Console.WriteLine("Stiskněte Enter pro pokračování...");
    Console.ReadLine();
}

void FindUser(string[] studentArray)
{
    Console.Clear();
    Console.Write("Zadejte jméno studenta k vyhledání: ");
    string searchName = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(searchName))
    {
        Console.WriteLine("Jméno nesmí být prázdné!");
        Console.WriteLine("Stiskněte Enter pro pokračování...");
        Console.ReadLine();
        return;
    }
    
    bool found = false;
    for (int i = 0; i < studentArray.Length; i++)
    {
        if (!string.IsNullOrEmpty(studentArray[i]) && 
            studentArray[i].Equals(searchName, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"Student nalezen na pozici {i + 1}: {studentArray[i]}");
            found = true;
        }
    }
    
    if (!found)
    {
        Console.WriteLine($"Student '{searchName}' nebyl nalezen.");
    }
    
    Console.WriteLine("Stiskněte Enter pro pokračování...");
    Console.ReadLine();
}

void RemoveUser(out string[] studentArray)
{
    studentArray = students;
    Console.Clear();
    Console.Write("Zadejte jméno studenta k odstranění: ");
    string removeName = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(removeName))
    {
        Console.WriteLine("Jméno nesmí být prázdné!");
        Console.WriteLine("Stiskněte Enter pro pokračování...");
        Console.ReadLine();
        return;
    }
    
    bool removed = false;
    for (int i = 0; i < studentArray.Length; i++)
    {
        if (!string.IsNullOrEmpty(studentArray[i]) && 
            studentArray[i].Equals(removeName, StringComparison.OrdinalIgnoreCase))
        {
            studentArray[i] = null;
            Console.WriteLine($"Student '{removeName}' byl odstraněn.");
            removed = true;
            break;
        }
    }
    
    if (!removed)
    {
        Console.WriteLine($"Student '{removeName}' nebyl nalezen.");
    }
    
    students = studentArray;
    Console.WriteLine("Stiskněte Enter pro pokračování...");
    Console.ReadLine();
}

void CountUsers(string[] studentArray)
{
    Console.Clear();
    int count = 0;
    
    for (int i = 0; i < studentArray.Length; i++)
    {
        if (!string.IsNullOrEmpty(studentArray[i]))
        {
            count++;
        }
    }
    
    Console.WriteLine($"Celkový počet studentů: {count}");
    Console.WriteLine("Stiskněte Enter pro pokračování...");
    Console.ReadLine();
}
