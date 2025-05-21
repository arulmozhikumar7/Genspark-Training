
namespace RepositoryPattern.Helpers
{
    public static class InputHelper
    {
        public static int ReadNonNegativeInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= 0)
                    return value;
                Console.WriteLine("Invalid input. Please enter a non-negative integer.");
            }
        }

        public static int? ReadOptionalNonNegativeInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    return null;  
                if (int.TryParse(input, out int value) && value >= 0)
                    return value;
                Console.WriteLine("Invalid input. Please enter a non-negative integer or leave blank.");
            }
        }

        public static double ReadNonNegativeDouble(string prompt)
        {
            double value;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out value) && value >= 0)
                    return value;
                Console.WriteLine("Invalid input. Please enter a non-negative number.");
            }
        }

        public static double? ReadOptionalNonNegativeDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    return null;
                if (double.TryParse(input, out double value) && value >= 0)
                    return value;
                Console.WriteLine("Invalid input. Please enter a non-negative number or leave blank.");
            }
        }

        public static string ReadString(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Input cannot be empty.");
            } while (string.IsNullOrWhiteSpace(input));
            return input!;
        }

        public static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? null : input;
        }

        public static (int Min, int Max)? ReadOptionalIntRange(string promptMin, string promptMax)
        {
            int? min = ReadOptionalNonNegativeInt(promptMin);
            int? max = ReadOptionalNonNegativeInt(promptMax);

            if (min == null || max == null)
                return null;

            if (max < min)
            {
                Console.WriteLine("Max cannot be less than Min. Skipping this range filter.");
                return null;
            }

            return (min.Value, max.Value);
        }

        public static (double Min, double Max)? ReadOptionalDoubleRange(string promptMin, string promptMax)
        {
            double? min = ReadOptionalNonNegativeDouble(promptMin);
            double? max = ReadOptionalNonNegativeDouble(promptMax);

            if (min == null || max == null)
                return null;

            if (max < min)
            {
                Console.WriteLine("Max cannot be less than Min. Skipping this range filter.");
                return null;
            }

            return (min.Value, max.Value);
        }
    }
}