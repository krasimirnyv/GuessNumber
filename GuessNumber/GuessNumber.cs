namespace GuessNumber;

class GuessNumber
{
    static void Main(string[] args)
    {
        // Define levels and tries
        uint tries = 10;
        uint level = 1;
        
        List<ulong> guessedNumbers = new List<ulong>(); // List of player's guessed numbers
        while (true)
        {
            PlayGame(ref tries, ref level, guessedNumbers); // Let's Play!
            
            Console.Write($"Sorry, you lost :(\n" + 
                          $"Do you want to try again? [y / n]: ");

            string answerAfterLost = PlayerAnswer(tries);
        }
    }
    
    private static void PlayGame(ref uint tries, ref uint level, List<ulong> guessedNumbers)
    {
        ulong computerNumber = GenerateComputerNumber(level); // Generate randomly the computer number
        
        while (tries > 0) // until tries are more than 0...
        {
            try
            {
                string playerInput = GetPlayerInput(level); // Gets the player's input
                ulong playerNumber = InputValidator(playerInput); // Validates the player's input as an integer number
                
                bool isLevelUp = GameActon(ref tries, ref level, playerNumber, computerNumber, guessedNumbers); // Let's dive into the game
                
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
        Console.Write($"Guess a number (1-{level * 100}): ");
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
    
    private static bool GameActon(ref uint tries, ref uint level, ulong playerNumber, ulong computerNumber, List<ulong> guessedNumbers)
    {
        tries--;
        if (playerNumber == computerNumber)
        {
            level++;
            tries = 10 + (level * 2);
            guessedNumbers.Add(playerNumber);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Congratulations, you guessed it!\n" +
                              $"Do you want to level up - next level: {level}? [y / n]: ");
            Console.ResetColor();

            string answerLevelUp = PlayerAnswer(tries);
            
            
            Console.WriteLine($"Guessed numbers so far: {string.Join(", ", guessedNumbers)}");
            
            return true;
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
        return false;
    }
    
    private static string PlayerAnswer(uint tries)
    {
        string answer = default;
        while ((answer = Console.ReadLine().ToLower()) != "yes" && answer != "y" 
                                                                && answer != "no" && answer != "n")
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; 
            if (tries == 0)
            {
                Console.Write("Incorrect answer! Type [y/yes] to Play Again or [n/no] to Quit: "); 
                continue;
            }
            
            Console.Write("Incorrect answer! Type [y/yes] to Level Up or [n/no] to Quit: "); 
        }
        
        if (answer == "no")
        {
            Console.WriteLine($"Thanks for playing Guess a Number!\n" +
                              $"See you soon! :)");
        }
        
        Console.ResetColor();
        return answer;
    }
}
