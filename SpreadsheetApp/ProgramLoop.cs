using System;
using System.Collections.Generic;
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
            };
        }

        private void GetArithmeticOperation()
        {
            var input = Console.ReadLine();
            var inputList = SplitStringToArrayByOperator(input);
            var numbersList = GetNumbersFromInput(inputList);
            var reversedList = ReversePolishNotation(numbersList);
        }

        private List<string> ReversePolishNotation(List<string> numbersList)
        {
            var output = new List<string>();
            var stack = new List<string>();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("+", 2);
            dict.Add("-", 2);
            dict.Add("*", 3);
            dict.Add("/", 3);

            for (var i = 0; i < numbersList.Count; i++)
            {
                if (i % 2 == 0)
                {
                    output.Add(numbersList[i]);
                }
                else if (dict[numbersList[i]] <= dict[stack[stack.Count]])
                {
                    output.Add(stack[stack.Count]);
                }
                else
                {
                    stack.Add(numbersList[i]);
                }
            }

            if (stack.Count > 0)
            {
                foreach (var mathOp in stack)
                {
                    output.Add(mathOp);
                }
            }
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

        //wersja szymona:
        private string ConvertToNumber(string input)
        {
            if (Double.TryParse(input, out double value))
            {
                return input;
            }


            var dupa = SplitStringToArrayByOperator(input);
            var notdupa = GetNumbersFromInput(dupa);

            ReversePolishNotation(notdupa);
            return "";
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

        public void ConvertListToTokens()
        {
           

            
        }
    }
}
