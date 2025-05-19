using System;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\nSelect a task to execute:");
            Console.WriteLine("1. Greet User");
            Console.WriteLine("2. Find Largest of Two Numbers");
            Console.WriteLine("3. Perform Arithmetic Operation");
            Console.WriteLine("4. Authenticate User");
            Console.WriteLine("5. Count Numbers Divisible by 7");
            Console.WriteLine("6. Frequency of numbers in an Array");
            Console.WriteLine("7. Array Rotation");
            Console.WriteLine("8. Merging Arrays");
            Console.WriteLine("9. Bulls And Cows Game");
            Console.WriteLine("10. Validate Sudoku Row");
            Console.WriteLine("11. Validate Sudoku Board");
            Console.WriteLine("12. Encrypt Decrypt Message");
            Console.WriteLine("0. Exit");

            Console.Write("Enter your choice (0-12): ");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 12.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    GreetUser();
                    break;
                case 2:
                    FindAndPrintLargest();
                    break;
                case 3:
                    CalculateAndPrintResult();
                    break;
                case 4:
                    AuthenticateUser();
                    break;
                case 5:
                    CountDivisibleBy7();
                    break;
                case 6:
                    CountFrequencyOfNumbers();
                    break;
                case 7:
                    ArrayRotation();
                    break;
                case 8:
                    MergeArray();
                    break;
                case 9:
                    BullsAndCowsGame();
                    break;
                case 10:
                    ValidateSudokuRow();
                    break;
                case 11:
                    ValidateSudokuBoard();
                    break;
                case 12:
                    EncryptDecryptMessage();
                    break;
                case 0:
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 0 and 12.");
                    break;
            }
        }
    }

    // Helper to confirm yes/no
    static bool ConfirmContinue(string taskName)
    {
        Console.Write($"Do you want to continue with {taskName}? (yes/no): ");
        return Console.ReadLine()?.Trim().ToLower() == "yes";
    }

    // Helper to read non-empty input
    static string ReadNonEmptyInput(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim();
        } while (string.IsNullOrEmpty(input));
        return input;
    }

    // Task 1: Greet User
    static void GreetUser()
    {
        string name = ReadNonEmptyInput("Enter your name: ");
        Console.WriteLine($"Hello, {name}! Welcome to the program.");
    }

    // Task 2: Find largest of two numbers
    static void FindAndPrintLargest()
    {
        do
        {
            int num1 = ReadNumber("Enter the first number: ");
            int num2 = ReadNumber("Enter the second number: ");

            if (num1 == num2)
            {
                Console.WriteLine("Both numbers are the same.");
            }
            else
            {
                int largest = (num1 > num2) ? num1 : num2;
                Console.WriteLine($"The largest number is: {largest}");
            }

        } while (ConfirmContinue("Task 2"));
    }

    // Helper to read an integer with validation
    static int ReadNumber(string prompt)
    {
        int number;
        Console.Write(prompt);

        while (!int.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid input! Please enter a valid integer.");
            Console.Write(prompt);
        }

        return number;
    }

    // Task 3: Perform arithmetic operation
    static void CalculateAndPrintResult()
    {
        do
        {
            double num1 = ReadDouble("Enter the first number: ");
            double num2 = ReadDouble("Enter the second number: ");

            Console.Write("Enter the operation to perform (+, -, *, /): ");
            string? operation = Console.ReadLine();

            while (operation != "+" && operation != "-" && operation != "*" && operation != "/")
            {
                Console.WriteLine("Invalid operation! Please enter one of (+, -, *, /).");
                Console.Write("Enter the operation to perform (+, -, *, /): ");
                operation = Console.ReadLine();
            }

            double result;

            switch (operation)
            {
                case "+":
                    result = num1 + num2;
                    break;
                case "-":
                    result = num1 - num2;
                    break;
                case "*":
                    result = num1 * num2;
                    break;
                case "/":
                    if (num2 == 0)
                    {
                        Console.WriteLine("Error: Division by zero is not allowed.");
                        continue;
                    }
                    result = num1 / num2;
                    break;
                default:
                    Console.WriteLine("Unexpected error.");
                    return;
            }

            Console.WriteLine($"Result: {num1} {operation} {num2} = {result}");

        } while (ConfirmContinue("Task 3"));
    }

    // Helper to read double with validation
    static double ReadDouble(string prompt)
    {
        double number;
        Console.Write(prompt);

        while (!double.TryParse(Console.ReadLine(), out number))
        {
            Console.WriteLine("Invalid input! Please enter a valid number.");
            Console.Write(prompt);
        }

        return number;
    }

    // Task 4: Authenticate User
    static void AuthenticateUser()
    {
        const string correctUsername = "Admin";
        const string correctPassword = "pass";
        int attemptsLeft = 3;

        while (attemptsLeft > 0)
        {
            string username = ReadNonEmptyInput("Enter username: ");
            Console.Write("Enter password: ");
            string? password = Console.ReadLine();

            if (username == correctUsername && password == correctPassword)
            {
                Console.WriteLine("Login successful! Welcome Admin.");
                return;
            }
            else
            {
                attemptsLeft--;
                Console.WriteLine($"Invalid credentials. Attempts left: {attemptsLeft}");
            }
        }

        Console.WriteLine("Login failed after 3 attempts. Access denied.");
    }

    // Task 5: Count numbers divisible by 7
    static void CountDivisibleBy7()
    {
        do
        {
            int countDivBy7 = 0;

            for (int i = 1; i <= 10; i++)
            {
                int num = ReadNumber($"Enter number {i}: ");

                if (num % 7 == 0)
                {
                    countDivBy7++;
                }
            }

            Console.WriteLine($"Total numbers divisible by 7: {countDivBy7}");

        } while (ConfirmContinue("Task 5"));
    }
    // Task 6
    static void CountFrequencyOfNumbers()
    {
        do
        {
            int count = ReadNumber("Enter how many numbers you want to input: ");
            int[] numbers = new int[count];

            for (int i = 0; i < count; i++)
            {
                numbers[i] = ReadNumber($"Enter number {i + 1}: ");
            }

            Dictionary<int, int> frequencyMap = new Dictionary<int, int>();
            foreach (int num in numbers)
            {
                if (frequencyMap.ContainsKey(num))
                    frequencyMap[num]++;
                else
                    frequencyMap[num] = 1;
            }

            Console.WriteLine("\nNumber Frequencies:");
            foreach (var kvp in frequencyMap)
            {
                Console.WriteLine($"Number {kvp.Key} occurred {kvp.Value} times.");
            }

        } while (ConfirmContinue("Task 6"));
    }

    // Task 7
    static void ArrayRotation()
    {
        int[] arr = null!;
        bool hasArray = false;

        do
        {
           if (!hasArray)
        {
    int size;
    do
    {
        size = ReadNumber("Enter number of elements (must be > 0): ");
        if (size <= 0)
            Console.WriteLine("Array size must be greater than 0. Please try again.");
    } while (size <= 0);

    arr = new int[size];

    Console.WriteLine("Enter the elements:");
    for (int i = 0; i < size; i++)
    {
        arr[i] = ReadNumber($"Element {i + 1}: ");
    }
    hasArray = true;
}

            Console.WriteLine("\nCurrent Array: " + string.Join(", ", arr));

            Console.Write("Enter direction to rotate (left/right): ");
            string? direction = Console.ReadLine()?.Trim().ToLower();

            while (direction != "left" && direction != "right")
            {
                Console.Write("Invalid direction! Enter 'left' or 'right': ");
                direction = Console.ReadLine()?.Trim().ToLower();
            }

            int steps = ReadNumber("Enter number of steps to rotate: ");
            steps %= arr.Length;

            if (direction == "left")
                RotateLeft(arr, steps);
            else
                RotateRight(arr, steps);

            Console.WriteLine("Rotated Array: " + string.Join(", ", arr));

            Console.Write("Do you want to continue rotating? (yes/no): ");
            if (Console.ReadLine()?.Trim().ToLower() != "yes")
                break;

            Console.Write("Do you want to continue with the same array? (yes/no): ");
            string? sameArrayAnswer = Console.ReadLine()?.Trim().ToLower();

            if (sameArrayAnswer == "no")
                hasArray = false;

        } while (true);
    }

    static void RotateLeft(int[] arr, int steps)
    {
        Reverse(arr, 0, steps - 1);
        Reverse(arr, steps, arr.Length - 1);
        Reverse(arr, 0, arr.Length - 1);
    }

    static void RotateRight(int[] arr, int steps)
    {
        Reverse(arr, 0, arr.Length - 1);
        Reverse(arr, 0, steps - 1);
        Reverse(arr, steps, arr.Length - 1);
    }

    static void Reverse(int[] arr, int start, int end)
    {
        while (start < end)
        {
            int temp = arr[start];
            arr[start] = arr[end];
            arr[end] = temp;
            start++;
            end--;
        }
    }

    // Task 8
    static void MergeArray()
    {
        do
        {
            int n1 = ReadNumber("Enter number of elements in first array: ");
            int[] arr1 = new int[n1];
            Console.WriteLine("Enter elements for first array:");
            for (int i = 0; i < n1; i++)
            {
                arr1[i] = ReadNumber($"Element {i + 1}: ");
            }

            int n2 = ReadNumber("Enter number of elements in second array: ");
            int[] arr2 = new int[n2];
            Console.WriteLine("Enter elements for second array:");
            for (int i = 0; i < n2; i++)
            {
                arr2[i] = ReadNumber($"Element {i + 1}: ");
            }

            Console.Write("First Array: ");
            PrintArray(arr1);

            Console.Write("Second Array: ");
            PrintArray(arr2);

            int[] merged = new int[n1 + n2];
            Array.Copy(arr1, 0, merged, 0, n1);
            Array.Copy(arr2, 0, merged, n1, n2);

            Console.Write("Merged Array: ");
            PrintArray(merged);

        } while (ConfirmContinue("merging arrays"));
    }

    // Helper function to print Array
    static void PrintArray(int[] arr)
    {
        Console.WriteLine(string.Join(" ", arr));
    }

    // Task 9
    static void BullsAndCowsGame()
    {
        const string secretWord = "GAME";
        int attempts = 0;

        Console.WriteLine("Welcome to Bulls and Cows Game!");
        Console.WriteLine("Guess the 4-letter secret word.");

        while (true)
        {
            string guess = ReadGuess("Enter your 4-letter guess: ");
            attempts++;

            int bulls = 0, cows = 0;
            bool[] secretUsed = new bool[4];
            bool[] guessUsed = new bool[4];
            string[] result = new string[4];

            // Step 1: Find Bulls
            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == secretWord[i])
                {
                    bulls++;
                    secretUsed[i] = true;
                    guessUsed[i] = true;
                    result[i] = $"{guess[i]} - Correct";
                }
            }

            // Step 2: Find Cows
            for (int i = 0; i < 4; i++)
            {
                if (guessUsed[i]) continue;

                for (int j = 0; j < 4; j++)
                {
                    if (!secretUsed[j] && guess[i] == secretWord[j])
                    {
                        cows++;
                        secretUsed[j] = true;
                        guessUsed[i] = true;
                        result[i] = $" {guess[i]} - Misplaced";
                        break;
                    }
                }
            }

            // Step 3: Mark Wrong letters
            for (int i = 0; i < 4; i++)
            {
                if (!guessUsed[i])
                {
                    result[i] = $" {guess[i]} - Wrong";
                }
            }

            // Display results
            Console.WriteLine("\nLetter Feedback:");
            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            Console.WriteLine($"\nResult: {bulls} Bulls, {cows} Cows\n");

            if (bulls == 4)
            {
                Console.WriteLine($" You guessed the word '{secretWord}' in {attempts} attempts!");
                break;
            }
        }
    }

    static string ReadGuess(string prompt)
    {
        string? input;
        do
        {
            Console.Write(prompt);
            input = Console.ReadLine()?.Trim().ToUpper();
        } while (string.IsNullOrEmpty(input) || input.Length != 4 || !IsAlpha(input));

        return input;
    }

    static bool IsAlpha(string s)
    {
        foreach (char c in s)
        {
            if (!char.IsLetter(c)) return false;
        }
        return true;
    }

    // Task 10
    static void ValidateSudokuRow()
    {
        do
        {
            Console.WriteLine("Enter 9 numbers (1 to 9) for the Sudoku row:");

            int[] row = new int[9];
            for (int i = 0; i < 9; i++)
            {
                row[i] = ReadNumberInRange($"Element {i + 1}: ", 1, 9);
            }

            // Check for uniqueness using a HashSet
            HashSet<int> seen = new HashSet<int>();
            foreach (int num in row)
            {
                seen.Add(num);
            }

            if (seen.Count == 9)
            {
                Console.WriteLine("The Sudoku row is VALID.");
            }
            else
            {
                Console.WriteLine("The Sudoku row is INVALID (contains duplicates or missing numbers).");
            }

        } while (ConfirmContinue("Sudoku Row Validation"));
    }

    // Helper method to read a number between min and max (inclusive)
    static int ReadNumberInRange(string prompt, int min, int max)
    {
        int number;
        bool validInput;
        do
        {
            Console.Write(prompt);
            validInput = int.TryParse(Console.ReadLine(), out number);
        } while (!validInput || number < min || number > max);

        return number;
    }

    // Task 11
    static void ValidateSudokuBoard()
    {
        do
        {
            // Console.WriteLine("Enter the 9x9 Sudoku board (only numbers 1 to 9):");
            // int[,] board = new int[9, 9];
                    int[,] board = new int[9, 9] {
                {5, 3, 4, 6, 7, 8, 9, 1, 2},
                {6, 7, 2, 1, 9, 5, 3, 4, 8},
                {1, 9, 8, 3, 4, 2, 5, 6, 7},
                {8, 5, 9, 7, 6, 1, 4, 2, 3},
                {4, 2, 6, 8, 5, 3, 7, 9, 1},
                {7, 1, 3, 9, 2, 4, 8, 5, 6},
                {9, 6, 1, 5, 3, 7, 2, 8, 4},
                {2, 8, 7, 4, 1, 9, 6, 3, 5},
                {3, 4, 5, 2, 8, 6, 1, 7, 9}
            };
            // Input
            // for (int row = 0; row < 9; row++)
            // {
            //     Console.WriteLine($"Enter 9 numbers for Row {row + 1}:");
            //     for (int col = 0; col < 9; col++)
            //     {
            //         board[row, col] = ReadNumberInRange($"Element {col + 1}: ", 1, 9);
            //     }
            // }
            PrintSudokuBoard(board);

            bool isValid = true;

            // Validate Rows
            for (int i = 0; i < 9; i++)
            {
                int[] row = new int[9];
                for (int j = 0; j < 9; j++) row[j] = board[i, j];

                if (!ValidateGroup(row, $"Row {i + 1}")) isValid = false;
            }

            // Validate Columns
            for (int i = 0; i < 9; i++)
            {
                int[] col = new int[9];
                for (int j = 0; j < 9; j++) col[j] = board[j, i];

                if (!ValidateGroup(col, $"Column {i + 1}")) isValid = false;
            }

            // Validate 3x3 Subgrids
            int boxNumber = 1;
            for (int boxRow = 0; boxRow < 3; boxRow++)
            {
                for (int boxCol = 0; boxCol < 3; boxCol++)
                {
                    int[] box = new int[9];
                    int index = 0;
                    for (int i = boxRow * 3; i < boxRow * 3 + 3; i++)
                    {
                        for (int j = boxCol * 3; j < boxCol * 3 + 3; j++)
                        {
                            box[index++] = board[i, j];
                        }
                    }

                    if (!ValidateGroup(box, $"Subgrid {boxNumber}")) isValid = false;
                    boxNumber++;
                }
            }

            Console.WriteLine(isValid
                ? "\n The Sudoku board is VALID!"
                : "\n The Sudoku board is INVALID.");

        } while (ConfirmContinue("Sudoku Board Validation"));
    }
    // Group Validator
    static bool ValidateGroup(int[] group, string label)
    {
        HashSet<int> seen = new HashSet<int>();
        foreach (int num in group)
        {
            seen.Add(num);
        }

        if (seen.Count == 9)
        {
            Console.WriteLine($" {label} is VALID.");
            return true;
        }
        else
        {
            Console.WriteLine($" {label} is INVALID (duplicates or missing numbers).");
            return false;
        }
    }
    // Helper to print the Sudoku
    static void PrintSudokuBoard(int[,] board)
    {
        Console.WriteLine("\nEntered Sudoku Board:");
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0 && i != 0)
            {
                Console.WriteLine("------+-------+------");
            }

            for (int j = 0; j < 9; j++)
            {
                if (j % 3 == 0 && j != 0)
                {
                    Console.Write("| ");
                }

                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    // Task 12
    static void EncryptDecryptMessage()
    {
        do
        {
            Console.Write("Enter a message (only lowercase letters, no spaces/symbols): ");
            string? message = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(message) || !IsLowercaseOnly(message))
            {
                Console.WriteLine("Invalid input. Use only lowercase letters with no spaces or symbols.");
                return;
            }

            string encrypted = Encrypt(message);
            string decrypted = Decrypt(encrypted);

            Console.WriteLine($"\nEncrypted Message: {encrypted}");
            Console.WriteLine($"Decrypted Message: {decrypted}");
           } while (ConfirmContinue("Task 12"));
    }

    // Encrypt by shifting characters forward by 3
    static string Encrypt(string input)
    {
        char[] result = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            result[i] = (char)('a' + (input[i] - 'a' + 3) % 26);
        }
        return new string(result);
    }

    // Decrypt by shifting characters backward by 3
    static string Decrypt(string input)
    {
        char[] result = new char[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            result[i] = (char)('a' + (input[i] - 'a' - 3 + 26) % 26);
        }
        return new string(result);
    }

    // Helper to ensure only lowercase letters
    static bool IsLowercaseOnly(string s)
    {
        foreach (char c in s)
        {
            if (c < 'a' || c > 'z')
                return false;
        }
        return true;
    }
  
}
