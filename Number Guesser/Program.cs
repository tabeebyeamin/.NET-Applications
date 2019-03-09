using System;


namespace NumberGuesser
{
    // Main Class
    class Program
    {
        // Entry Point Method
        static void Main(string[] args)
        {

            // Get and display application info
            GetAppInfo();

            // Ask for the user's name and greet them
            GreetUser();

            bool play = true;

            while (play)
            {
                // Create a new random object
                Random random = new Random();
                // Generates random number from 1 to 10
                int correctNumber = random.Next(1, 10);
                // Initial guess var
                int guess = 0;
                // Ask for the number
                Console.WriteLine("Guess a number between 1 and 10");

                // Loop until user gets it right
                while (guess != correctNumber)
                {
                    // Get user's input
                    string input = Console.ReadLine();

                    // Make sure the input is a number
                    if (!int.TryParse(input, out guess))
                    {
                        // Print error message
                        PrintColourMessage(ConsoleColor.Red, "That's not a number");

                        // Keep going
                        continue;
                    }

                    // Cast the guess to an integer and put it to guess var
                    guess = Int32.Parse(input);

                    // Match guess to correct number
                    if (guess != correctNumber)
                    {
                        // Say that they are wrong
                        PrintColourMessage(ConsoleColor.Red, "Wrong number please try again!");
                    }
                }

                // Output Success message
                PrintColourMessage(ConsoleColor.Yellow, "Congratulations! Work so hard forget how to vacation!");

                // Ask user if they want to play again
                PrintColourMessage(ConsoleColor.Blue, "Play Again? [Y or N]");

                string playAgain = Console.ReadLine().ToUpper();

                if (playAgain != "Y")
                {
                    play = false;
                }
            }            
        }

        static void GetAppInfo()
        {
            // Set app vars
            string appName = "Number Guesser";
            string appVersion = "1.0.0";
            string appAuthor = "Tabeeb Yeamin";

            // Write out application info
            PrintColourMessage(ConsoleColor.Blue,
                appName + ": Version " + appVersion + " by " + appAuthor);

        }

        static void GreetUser()
        {
            // Ask users name
            Console.WriteLine("What is your name?");
            // Get user input
            string inputName = Console.ReadLine();
            Console.WriteLine("Hello {0}, let's play a game.", inputName);

        }

        // Printing to console in a colour

        static void PrintColourMessage(ConsoleColor colour, string message)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
