namespace GuessNumber;

class GuessNumber
{
    static void Main(string[] args)
    {
        List<ulong> guessedNumbers = new List<ulong>(); // List of player's guessed numbers
        while (true)
        {
            // Define levels and tries
            uint tries = 10;
            uint level = 1;
            
            bool isQuit = false;
            PlayGame(ref tries, ref level, guessedNumbers, ref isQuit); // Let's Play!

            if (guessedNumbers.Count > 0)
            {
                Console.WriteLine($"Guessed numbers: {string.Join(", ", guessedNumbers)}.");
            }
            
            if (isQuit)
            {
                Console.WriteLine($"Thanks for playing Guess a Number!\n" +
                                  $"See you soon! :)");
                return;
            }
            
            Console.Write($"Sorry, you lost :(\n" + 
                          $"Do you want to try again? [y / n]: ");

            bool answerAfterLost = PlayerAnswer(guessedNumbers);
            if (answerAfterLost)
            {
                return;
            }
        }
    }
    
    private static void PlayGame(ref uint tries, ref uint level, List<ulong> guessedNumbers, ref bool isQuit)
    {
        ulong computerNumber = GenerateComputerNumber(level); // Generate randomly the computer number

        while (tries > 0) // until tries are more than 0...
        {
            try
            {
                string playerInput = GetPlayerInput(level); // Gets the player's input
                ulong playerNumber = InputValidator(playerInput); // Validates the player's input as an integer number
                
                GameActon(ref tries, ref level, playerNumber, ref computerNumber, guessedNumbers, ref isQuit); // Let's dive into the game

                if (isQuit)
                {
                    return;
                }
            }
            catch (Exception exceptionInput) // After getting invalid answer from the player, he tries again
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"{exceptionInput.Message} You need to enter a number!");
                Console.ResetColor();
            }
        }
    }
    
    private static ulong GenerateComputerNumber(uint level)
    {
        Random random = new Random();
        ulong computerNumber = (ulong)random.Next(1, (int)(level * 100 + 1));
        
        return computerNumber;
    }
    
    private static string GetPlayerInput(uint level)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Guess a number (1 - {level * 100}): ");
        Console.ResetColor();
        
        string playerInput = Console.ReadLine();
        return playerInput;
    }
    
    private static ulong InputValidator(string playerInput)
    {
        bool isValid = ulong.TryParse(playerInput, out ulong playerNumber);
        if (!isValid)
        {
            throw new ArgumentException("Invalid input.");
        }
        
        return playerNumber;
    }
    
    private static void GameActon(ref uint tries, ref uint level, ulong playerNumber, ref ulong computerNumber, List<ulong> guessedNumbers, ref bool isQuit)
    {
        tries--;
        if (playerNumber == computerNumber)
        {
            guessedNumbers.Add(playerNumber);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Congratulations, you guessed it");
            Console.ResetColor();
            Console.Write($"Do you want to level up - next level: {++level}? [y / n]: ");

            bool answer = PlayerAnswer();
            if (answer)
            {
                isQuit = true;
                return;
            }
            
            tries = 10 + (level * 2);
            computerNumber = GenerateComputerNumber(level);
            
            Console.WriteLine($"Guessed numbers so far: {string.Join(", ", guessedNumbers)}");
        }
        else if (playerNumber > computerNumber)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Too High!");
        }
        else // playerNumber < computerNumber
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Too Low!");
        }
        
        Console.ResetColor();
    }
    
    private static bool PlayerAnswer() // before level up
    {
        string answer = default;
        while ((answer = Console.ReadLine().ToLower()) != "yes" && answer != "y" 
                                                                && answer != "no" && answer != "n")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; 
            Console.Write("Incorrect answer! Type [y/yes] to Level Up or [n/no] to Quit: ");
        }
        
        if (AnswerIsNo(answer))
        {
            return true;
        }
        
        Console.ResetColor();
        return false;
    }
    
    private static bool PlayerAnswer(List<ulong> guessedNumbers) // after losing the game
    {
        string answer = default;
        while ((answer = Console.ReadLine().ToLower()) != "yes" && answer != "y" 
                                                                && answer != "no" && answer != "n")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; 
            Console.Write("Incorrect answer! Type [y/yes] to Play Again or [n/no] to Quit: "); 
        }

        if (AnswerIsNo(answer, guessedNumbers))
        {
            return true;
        }
        
        Console.ResetColor();
        return false;
    }

    private static bool AnswerIsNo(string answer, List<ulong> guessedNumbers)
    {
        if (answer is "no" or "n")
        {
            Console.WriteLine($"Thanks for playing Guess a Number!");
            if (guessedNumbers.Count > 0)
            {
                Console.WriteLine($"Guessed numbers: {string.Join(", ", guessedNumbers)}.");
            }

            Console.WriteLine("See you soon! :)");
            return true;
        }

        return false;
    }
    
    private static bool AnswerIsNo(string answer)
    {
        if (answer is "no" or "n")
        {
            return true;
        }

        return false;
    }
}
