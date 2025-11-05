using System.Collections.Generic;

string input;

bool validInput = false;

string parsedNum = "";

string validOperators = "+-/*";

string validNums = "0123456789";

int tmpNum;
int sum;
char operand;

LinkedList<string> parsed_input = new LinkedList<string>();

Console.Clear();

do
{
    ClearVariables(ref parsedNum, ref parsed_input);

    input = GetInput();

    ParseInput(input, ref parsedNum, ref parsed_input, validNums, validOperators, out tmpNum);

    if (!ProcessParsedInput(ref parsedNum, ref parsed_input, out tmpNum))
    {
        Console.WriteLine("couldn't parse equation");
    }
    else
    {
        sum = CalculateResult(parsed_input, out operand);
        Console.WriteLine("your output is {0}", sum);
        validInput = true;
        Console.WriteLine("to exit this program press Ctrl C");
    }
}
while (true);


string GetInput()
{
    Console.Write("enter operation with whole numbers and following operands +, -, /, *: ");

    return Console.ReadLine() ?? string.Empty;
}

void ClearVariables(ref string parsedNum, ref LinkedList<string> parsed_input)
{
    parsed_input.Clear();
    parsedNum = "";
}

void ParseInput(string input, ref string parsedNum, ref LinkedList<string> parsed_input, string validNums, string validOperators, out int tmpNum)
{
    tmpNum = 0;

    for (int i = 0; i < input.Length; i++)
    {
        if (IsValidNumberChar(parsedNum, input[i], validNums))
        {
            parsedNum += input[i];
        }
        else if (validOperators.Contains(input[i]))
        {
            HandleOperator(input[i], i, ref parsedNum, ref parsed_input, out tmpNum);
        }
    }
}

bool IsValidNumberChar(string parsedNum, char currentChar, string validNums)
{
    return (parsedNum == "" && currentChar == '-') || validNums.Contains(currentChar);
}

void HandleOperator(char operatorChar, int position, ref string parsedNum, ref LinkedList<string> parsed_input, out int tmpNum)
{
    tmpNum = 0;

    if (parsedNum == "")
    {
        Console.WriteLine("found operand {0} without number before it at position {1}", operatorChar, position);
    }

    if (int.TryParse(parsedNum, out tmpNum))
    {
        parsed_input.AddLast(parsedNum);
        parsed_input.AddLast(operatorChar.ToString());
        parsedNum = "";
    }
}

bool ProcessParsedInput(ref string parsedNum, ref LinkedList<string> parsed_input, out int tmpNum)
{
    tmpNum = 0;

    if (parsed_input.Count() == 0)
    {
        return false;
    }

    if (parsedNum == "" || !(int.TryParse(parsedNum, out tmpNum)))
    {
        Console.WriteLine("found trailing operand, ignoring it");
        parsed_input.RemoveLast();
    }
    else
    {
        parsed_input.AddLast(parsedNum);
    }

    return true;
}

int CalculateResult(LinkedList<string> parsed_input, out char operand)
{
    int sum;
    int tmpNum;

    int.TryParse(parsed_input.First(), out sum);
    parsed_input.RemoveFirst();
    operand = ' ';

    foreach (string element in parsed_input)
    {
        if (operand == ' ')
        {
            operand = element[0];
        }
        else
        {
            sum = ApplyOperation(sum, operand, element, out tmpNum);
            operand = ' ';
        }
    }

    return sum;
}

int ApplyOperation(int sum, char operand, string element, out int tmpNum)
{
    int.TryParse(element, out tmpNum);

    switch (operand)
    {
        case '+':
            return sum + tmpNum;
        case '-':
            return sum - tmpNum;
        case '*':
            return sum * tmpNum;
        case '/':
            return sum / tmpNum;
        default:
            return sum;
    }
}