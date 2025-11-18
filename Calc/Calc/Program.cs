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

ClearConsole();

do
{
    ResetParsedNumber(ref parsedNum);
    ClearParsedInput(ref parsed_input);

    input = GetInput();

    ParseInput(input, ref parsedNum, ref parsed_input, validNums, validOperators, out tmpNum);

    if (!ValidateParsedInput(parsed_input))
    {
        DisplayParseError();
    }
    else if (ShouldProcessFinalNumber(parsedNum, out tmpNum))
    {
        AddFinalNumber(ref parsed_input, parsedNum);
        sum = CalculateResult(parsed_input, out operand);
        DisplayResult(sum);
        validInput = true;
        DisplayExitMessage();
    }
    else
    {
        HandleTrailingOperator(ref parsed_input);
        sum = CalculateResult(parsed_input, out operand);
        DisplayResult(sum);
        validInput = true;
        DisplayExitMessage();
    }
}
while (true);


// Console output functions
void ClearConsole()
{
    Console.Clear();
}

void DisplayParseError()
{
    Console.WriteLine("couldn't parse equation");
}

void DisplayResult(int result)
{
    Console.WriteLine("your output is {0}", result);
}

void DisplayExitMessage()
{
    Console.WriteLine("to exit this program press Ctrl C");
}

void DisplayOperatorError(char operatorChar, int position)
{
    Console.WriteLine("found operand {0} without number before it at position {1}", operatorChar, position);
}

void DisplayTrailingOperatorWarning()
{
    Console.WriteLine("found trailing operand, ignoring it");
}

// Input functions
string GetInput()
{
    Console.Write("enter operation with whole numbers and following operands +, -, /, *: ");

    return Console.ReadLine() ?? string.Empty;
}

// Variable management functions
void ResetParsedNumber(ref string parsedNum)
{
    parsedNum = "";
}

void ClearParsedInput(ref LinkedList<string> parsed_input)
{
    parsed_input.Clear();
}

// Character validation functions
bool IsValidNumberChar(string parsedNum, char currentChar, string validNums)
{
    return IsNegativeSign(parsedNum, currentChar) || IsDigit(currentChar, validNums);
}

bool IsNegativeSign(string parsedNum, char currentChar)
{
    return parsedNum == "" && currentChar == '-';
}

bool IsDigit(char currentChar, string validNums)
{
    return validNums.Contains(currentChar);
}

bool IsOperator(char currentChar, string validOperators)
{
    return validOperators.Contains(currentChar);
}

// Input parsing functions
void ParseInput(string input, ref string parsedNum, ref LinkedList<string> parsed_input, string validNums, string validOperators, out int tmpNum)
{
    tmpNum = 0;

    for (int i = 0; i < GetInputLength(input); i++)
    {
        ProcessCharacter(input[i], i, ref parsedNum, ref parsed_input, validNums, validOperators, out tmpNum);
    }
}

int GetInputLength(string input)
{
    return input.Length;
}

void ProcessCharacter(char currentChar, int position, ref string parsedNum, ref LinkedList<string> parsed_input, string validNums, string validOperators, out int tmpNum)
{
    tmpNum = 0;

    if (IsValidNumberChar(parsedNum, currentChar, validNums))
    {
        AppendCharacterToNumber(ref parsedNum, currentChar);
    }
    else if (IsOperator(currentChar, validOperators))
    {
        ProcessOperator(currentChar, position, ref parsedNum, ref parsed_input, out tmpNum);
    }
}

void AppendCharacterToNumber(ref string parsedNum, char currentChar)
{
    parsedNum += currentChar;
}

// Operator processing functions
void ProcessOperator(char operatorChar, int position, ref string parsedNum, ref LinkedList<string> parsed_input, out int tmpNum)
{
    tmpNum = 0;

    if (IsParsedNumberEmpty(parsedNum))
    {
        DisplayOperatorError(operatorChar, position);
    }

    if (TryParseNumber(parsedNum, out tmpNum))
    {
        AddNumberToList(ref parsed_input, parsedNum);
        AddOperatorToList(ref parsed_input, operatorChar);
        ResetParsedNumber(ref parsedNum);
    }
}

bool IsParsedNumberEmpty(string parsedNum)
{
    return parsedNum == "";
}

bool TryParseNumber(string parsedNum, out int tmpNum)
{
    return int.TryParse(parsedNum, out tmpNum);
}

void AddNumberToList(ref LinkedList<string> parsed_input, string number)
{
    parsed_input.AddLast(number);
}

void AddOperatorToList(ref LinkedList<string> parsed_input, char operatorChar)
{
    parsed_input.AddLast(ConvertCharToString(operatorChar));
}

string ConvertCharToString(char character)
{
    return character.ToString();
}

// Parsed input validation functions
bool ValidateParsedInput(LinkedList<string> parsed_input)
{
    return !IsListEmpty(parsed_input);
}

bool IsListEmpty(LinkedList<string> parsed_input)
{
    return parsed_input.Count() == 0;
}

bool ShouldProcessFinalNumber(string parsedNum, out int tmpNum)
{
    tmpNum = 0;
    return !IsParsedNumberEmpty(parsedNum) && TryParseNumber(parsedNum, out tmpNum);
}

void AddFinalNumber(ref LinkedList<string> parsed_input, string parsedNum)
{
    parsed_input.AddLast(parsedNum);
}

void HandleTrailingOperator(ref LinkedList<string> parsed_input)
{
    DisplayTrailingOperatorWarning();
    RemoveLastElement(ref parsed_input);
}

void RemoveLastElement(ref LinkedList<string> parsed_input)
{
    parsed_input.RemoveLast();
}

// Calculation functions
int CalculateResult(LinkedList<string> parsed_input, out char operand)
{
    int sum = ExtractFirstNumber(parsed_input);
    RemoveFirstElement(parsed_input);
    operand = InitializeOperand();

    sum = ProcessElements(parsed_input, sum, ref operand);

    return sum;
}

int ExtractFirstNumber(LinkedList<string> parsed_input)
{
    int number;
    int.TryParse(GetFirstElement(parsed_input), out number);
    return number;
}

string GetFirstElement(LinkedList<string> parsed_input)
{
    return parsed_input.First();
}

void RemoveFirstElement(LinkedList<string> parsed_input)
{
    parsed_input.RemoveFirst();
}

char InitializeOperand()
{
    return ' ';
}

int ProcessElements(LinkedList<string> parsed_input, int sum, ref char operand)
{
    foreach (string element in parsed_input)
    {
        if (IsOperandEmpty(operand))
        {
            operand = ExtractOperandFromElement(element);
        }
        else
        {
            sum = PerformOperation(sum, operand, element);
            operand = InitializeOperand();
        }
    }

    return sum;
}

bool IsOperandEmpty(char operand)
{
    return operand == ' ';
}

char ExtractOperandFromElement(string element)
{
    return GetFirstCharacter(element);
}

char GetFirstCharacter(string text)
{
    return text[0];
}

int PerformOperation(int sum, char operand, string element)
{
    int tmpNum;
    return ApplyOperation(sum, operand, element, out tmpNum);
}

// Arithmetic operation functions
int ApplyOperation(int sum, char operand, string element, out int tmpNum)
{
    tmpNum = ParseElement(element);

    return ExecuteOperation(sum, operand, tmpNum);
}

int ParseElement(string element)
{
    int tmpNum;
    int.TryParse(element, out tmpNum);
    return tmpNum;
}

int ExecuteOperation(int sum, char operand, int number)
{
    switch (operand)
    {
        case '+':
            return PerformAddition(sum, number);
        case '-':
            return PerformSubtraction(sum, number);
        case '*':
            return PerformMultiplication(sum, number);
        case '/':
            return PerformDivision(sum, number);
        default:
            return sum;
    }
}

int PerformAddition(int a, int b)
{
    return a + b;
}

int PerformSubtraction(int a, int b)
{
    return a - b;
}

int PerformMultiplication(int a, int b)
{
    return a * b;
}

int PerformDivision(int a, int b)
{
    return a / b;
}