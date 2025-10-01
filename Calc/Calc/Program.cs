using System.Collections.Generic;

string input;

bool validInput = false;

string parsedNum;

string validOperators = "+-/*";

string validNums = "0123456789";

int tmpNum;
int sum;
char operand;

LinkedList<string> parsed_input = new LinkedList<string>();

Console.Clear(); // to clear console if it was closed with Ctrl C last time

do
{
    Console.Write("enter operation with whole numbers and following operands +, -, /, *: ");

    parsed_input.Clear();

    input = Console.ReadLine() ?? string.Empty;

    parsedNum = "";

    for (int i = 0; i < input.Length; i++)
    {
        if ((parsedNum == "" && input[i] == '-') || (validNums.Contains(input[i])))
        {
            parsedNum += input[i];
        }
        else if (validOperators.Contains(input[i]))
        {
            if (parsedNum == "")
            {
                Console.WriteLine("found operand {0} without number before it at position {1}", input[i], i);
            }

            if (int.TryParse(parsedNum, out tmpNum))
            {
                parsed_input.AddLast(parsedNum);
                parsed_input.AddLast(input[i].ToString());
                parsedNum = "";
            }
        }
    }

    if (parsed_input.Count() == 0) {
        Console.WriteLine("couldn't parse equation");
    }
    else
    {
        if (parsedNum == "" || !(int.TryParse(parsedNum, out tmpNum)))
        {
            Console.WriteLine("found trailing operand, ignoring it");
            parsed_input.RemoveLast();
        }
        else
        {
            parsed_input.AddLast(parsedNum);
        }

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
                int.TryParse(element, out tmpNum);
                switch (operand)
                {
                    case '+':
                        sum += tmpNum;
                        break;
                    case '-':
                        sum -= tmpNum;
                        break;
                    case '*':
                        sum *= tmpNum;
                        break;
                    case '/':
                        sum = sum / tmpNum;
                        break;
                }
                operand = ' ';
            }
        }
        Console.WriteLine("your output is {0}", sum);
        validInput = true;
        Console.WriteLine("to exit this program press Ctrl C");
    }
}
while (true); // originaly it was "while (!validInput);" but requirements exist
