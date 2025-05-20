using System;

namespace EmployeeApp
{
    public static class Helper
    {
        public static string? ReadNonEmptyString(string prompt)
        {
            string? input;
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(input));

            return input;
        }

        public static int ReadInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
            return value;
        }

        public static double ReadDouble(string prompt)
        {
            double value;
            while (true)
            {
                Console.WriteLine(prompt);
                if (double.TryParse(Console.ReadLine(), out value))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
            return value;
        }
    }
}
