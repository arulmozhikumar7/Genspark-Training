using System;

namespace AppointmentMaker.Helpers
{
    public static class InputValidator
    {
        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Input cannot be empty. Please try again.");
            }
        }

        public static int ReadNonNegativeInt(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                if (int.TryParse(Console.ReadLine(), out int val) && val >= 0)
                    return val;

                Console.WriteLine("Please enter a valid non-negative integer.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write($"{prompt} ");
                string input = Console.ReadLine()!;

                if (DateTime.TryParseExact(
                        input,
                        "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out date))
                {
                    if (date.Date < DateTime.Today)
                    {
                        Console.WriteLine(" Appointment date cannot be in the past. Please enter a future or today's date.");
                        continue;
                    }

                    return date;
                }

                Console.WriteLine(" Invalid date format. Use dd-MM-yyyy (e.g., 21-05-2025).");
            }
        }

        public static string ReadOptionalString(string prompt)
        {
            Console.WriteLine(prompt);
            string? input = Console.ReadLine();
            return input?.Trim() ?? string.Empty; 
        }

        public static string ReadOption(string prompt, string[] validOptions)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (input != null && Array.Exists(validOptions, opt => opt == input))
                    return input;

                Console.WriteLine($"Invalid option. Valid options are: {string.Join(", ", validOptions)}");
            }
        }
    }
}
