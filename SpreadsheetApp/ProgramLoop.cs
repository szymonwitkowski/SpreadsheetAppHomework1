using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SpreadsheetApp.Data;

namespace SpreadsheetApp
{
    internal class ProgramLoop
    {
        public void Run()
        {
            var exit = false;
            while (!exit)
            {
                PrintMenu();
                var menuOption = Console.ReadLine();
                switch (menuOption)
                {
                    case "1":
                        Console.WriteLine("values:\n");
                        InputData();
                        break;
                    case "2":
                        Console.WriteLine("input arithmetic op");
                        GetArithmeticOperation();
                        break;
                }
            }
        }

        private void GetArithmeticOperation()
        {
            var input = Console.ReadLine();
            var inputList = SplitStringToArrayByOperator(input);
            var numbersList = GetNumbersFromInput(inputList);
        }

        private double ReversePolishNotation(List<string> numbersList)
        {
            var stack = new Stack<string>();
            var output = new List<string>();
            Dictionary<string, int> dict = new Dictionary<string, int>
                {
                    { "+", 2},
                    { "-", 2},
                    { "*", 3},
                    { "/", 3}
                };

            foreach (var t in numbersList)
            {
                if (double.TryParse(t, out double result))
                {
                    output.Add(result.ToString());
                }
                else if (stack.Count == 0)
                {
                    stack.Push(t);
                }
                else if (dict[t] <= dict[stack.Peek()])
                {
                    output.Add(stack.Peek());
                    stack.Pop();
                    if (dict[t] <= dict[stack.Peek()] || stack.Count == 0)
                    {
                        output.Add(stack.Peek());
                        stack.Pop();
                    }
                    stack.Push(t);
                }
                else
                {
                    stack.Push(t);
                }
            }

            if (stack.Count > 0)
            {
                foreach (var mathOperator in stack)
                {
                    output.Add(mathOperator);
                }
            }

            for (int i = 0; i < output.Count; i++)
            {
                double result;

                if (output[i] == "+" || output[i] == "-" || output[i] == "*" || output[i] == "/")
                {
                    switch (output[i])
                    {
                        case "+":
                            result = Double.Parse(output[i - 2]) + Double.Parse(output[i - 1]);
                            output[i - 2] = result.ToString();
                            output.Remove(output[i - 1]);
                            i--;
                            output.Remove(output[i]);
                            i--;
                            break;
                        case "-":
                            result = Double.Parse(output[i - 2]) - Double.Parse(output[i - 1]);
                            output[i - 2] = result.ToString();
                            output.Remove(output[i - 1]);
                            i--;
                            output.Remove(output[i]);
                            i--;
                            break;
                        case "*":
                            result = Double.Parse(output[i - 2]) * Double.Parse(output[i - 1]);
                            output[i - 2] = result.ToString();
                            output.Remove(output[i - 1]);
                            i--;
                            output.Remove(output[i]);
                            i--;
                            break;
                        case "/":
                            result = Double.Parse(output[i - 2]) / Double.Parse(output[i - 1]);
                            output[i - 2] = result.ToString();
                            output.Remove(output[i - 1]);
                            i--;
                            output.Remove(output[i]);
                            i--;
                            break;
                    }
                }
            }
            var calculationResult = Double.Parse(output[0]);

            return calculationResult;
        }

        private List<string> SplitStringToArrayByOperator(string input)
        {
            string pattern = @"([+]|[*]|[-]|[\/])";
            var inputList = new List<string>();

            string[] substrings = Regex.Split(input, pattern);

            foreach (string match in substrings)
            {
                inputList.Add(match);
            }

            return inputList;
        }

        private List<string> GetNumbersFromInput(List<string> inputList)
        {
            var numbersList = new List<string>();

            for (var i = 0; i < inputList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    numbersList.Add(ConvertToNumber(inputList[i]));
                }
                else
                {
                    numbersList.Add(inputList[i]);
                }
            }

            return numbersList;
        }
        
        private string ConvertToNumber(string input)
        {
            if (Double.TryParse(input, out double value))
            {
                return input;
            }

            var arraySplitByOperators = SplitStringToArrayByOperator(input);
            var arithmeticExpressionArray = GetNumbersFromInput(arraySplitByOperators);

            return ReversePolishNotation(arithmeticExpressionArray).ToString();
        }

        private void InputData()
        {
            var inputString = Console.ReadLine();
            SeparateValues(inputString);
        }

        private void SeparateValues(string inputString)
        {
            var tempDoubleList = new List<string>();


            if (inputString.EndsWith("|") || inputString.EndsWith(";"))
            {
                var inputStringWithoutEndingChars = inputString.Remove(inputString.Length - 1);

                var separator = '|';

                var separatedInput = inputStringWithoutEndingChars.Split(separator).ToList();

                foreach (var input in separatedInput)
                {

                    tempDoubleList.Add(input);
                }

                ProgramData.Spreadsheet.Add(tempDoubleList);

            }
            else
            {
                Console.WriteLine("Wrong input");
                return;
            }

            if (inputString[inputString.Length - 1] != ';')
            {
                InputData();
            }
        }

        public void PrintMenu()
        {
            Console.WriteLine("Menu");
            Console.WriteLine("1. Input values");
            Console.WriteLine("2. Input operation");
        }
    }
}
