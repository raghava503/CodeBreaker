using CodeBreaker.Core;

var game = new Game();
Console.WriteLine("=== CODE BREAKER ===");
Console.WriteLine($"Guess the {Game.CodeLength}-digit code (digits 1-6). You have {Game.MaxAttempts} tries.");

while (!game.IsOver)
{
    Console.Write($"Attempt {game.Attempts + 1}/{Game.MaxAttempts}: ");

    if (!Game.TryParseGuess(Console.ReadLine(), out int[] guess))
    {
        Console.WriteLine("Enter exactly 4 digits, each between 1 and 6.");
        continue;
    }

    var result = game.Guess(guess);

    if (game.IsWon)
        Console.WriteLine($"You cracked it in {game.Attempts} tries!");
    else
        Console.WriteLine($"Exact: {result.Exact} | Misplaced: {result.Misplaced}");
}

if (!game.IsWon)
    Console.WriteLine($"Out of tries! The code was {game.RevealSecret()}");
const int MaxAttempts = 10;

var random = new Random();
int[] secret = Enumerable.Range(0, 4)
                         .Select(_ => random.Next(1, 7))
                         .ToArray();

Console.WriteLine("=== CODE BREAKER ===");
Console.WriteLine($"Guess the 4-digit code (digits 1-6). You have {MaxAttempts} tries.");

int attempt = 1;
while (attempt <= MaxAttempts)
{
    Console.Write($"Attempt {attempt}/{MaxAttempts}: ");
    string? input = Console.ReadLine();

    if (!TryParseGuess(input, out int[] guess))
    {
        Console.WriteLine("Enter exactly 4 digits, each between 1 and 6.");
        continue; // invalid input doesn't cost an attempt
    }

    var (exact, misplaced) = Score(secret, guess);

    if (exact == 4)
    {
        Console.WriteLine($"You cracked it in {attempt} tries!");
        return;
    }

    Console.WriteLine($"Exact: {exact} | Misplaced: {misplaced}");
    attempt++;
}

Console.WriteLine($"Out of tries! The code was {string.Join("", secret)}");

static bool TryParseGuess(string? input, out int[] guess)
{
    guess = Array.Empty<int>();
    if (input is null || input.Length != 4) return false;

    int[] digits = input.Select(c => c - '0').ToArray();
    if (digits.Any(d => d < 1 || d > 6)) return false;

    guess = digits;
    return true;
}

static (int Exact, int Misplaced) Score(int[] secret, int[] guess)
{
    int exact = secret.Zip(guess).Count(pair => pair.First == pair.Second);

    int commonDigits = Enumerable.Range(1, 6)
        .Sum(d => Math.Min(secret.Count(x => x == d), guess.Count(x => x == d)));

    return (exact, commonDigits - exact);
}