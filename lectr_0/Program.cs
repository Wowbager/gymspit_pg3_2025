// See https://aka.ms/new-console-template for more information

string s = "zkusíme, zda to funguje i s dlouhými texty a českými znaky!";
Console.CursorVisible = false; 
for (int i = 30; i > 0; i = i - 5)
{
    FancyWrite(s, i);
}
Console.CursorVisible = true;

void FancyWrite(string s, int sleep_time)
{
    int lenght = s.Length;
    for (int i = - lenght; i <= lenght; i++)
    {
        Console.SetCursorPosition(0, 0); 
        Console.Write(GetString(s, i, i + lenght)); 
        Thread.Sleep(sleep_time);
    }
}


string GetString(string s, int start, int end)
{
    char[] output = new char[s.Length];
    for (int i = 0; i < output.Length; i++)
    {
        output[i] = ' ';
    }
    //cleaning up input
    if (s == null || start > end)
    {
        return new string(output);
    }
    if (start < 0)
    {
        start = 0;
    }
    if (end > s.Length)
    {
        end = s.Length;
    }

    for (int i = start; i < end; i++)
    {
        output[i] = s[i];
    }

    return new string(output);
}