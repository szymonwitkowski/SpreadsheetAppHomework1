using System;
using System.Collections.Generic;
using System.Linq;
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
                var menuOption = Console.ReadLine();
                switch (menuOption)
                {
                    case "1":
                        Console.WriteLine("Ipnut values:\n");
                        InputData();
                        break;
                }
            };
        }

        private void InputData()
        {
            var inputString = Console.ReadLine();
            SeparateValues(inputString);
        }

        private void SeparateValues(string inputString)
        {
            var tempDoubleList = new List<Double>();


            if (inputString.EndsWith("|") || inputString.EndsWith(";"))
            {
                var inputStringWithoutEndingChars = inputString.Remove(inputString.Length - 1);

                var separator = '|';

                var separatedInput = inputStringWithoutEndingChars.Split(separator).ToList();

                foreach (var input in separatedInput)
                {

                    tempDoubleList.Add(Convert.ToDouble(input));
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
    }
}
