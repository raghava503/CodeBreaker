namespace CodeBreaker.Core;

public class Game
{
    public const int CodeLength = 4;
    public const int MaxAttempts = 10;

    private readonly int[] _secret;

    public int Attempts { get; private set; }
    public bool IsWon { get; private set; }
    public bool IsOver => IsWon || Attempts >= MaxAttempts;

    public Game() : this(GenerateSecret()) { }

    public Game(int[] secret)
    {
        if (secret.Length != CodeLength || secret.Any(d => d < 1 || d > 6))
            throw new ArgumentException("Secret must be 4 digits between 1 and 6.", nameof(secret));

        _secret = secret;
    }

    public GuessResult Guess(int[] guess)
    {
        if (IsOver)
            throw new InvalidOperationException("The game is already over.");
        if (guess.Length != CodeLength)
            throw new ArgumentException("Guess must be 4 digits.", nameof(guess));

        Attempts++;
        var result = Score(_secret, guess);
        IsWon = result.Exact == CodeLength;
        return result;
    }

    public string RevealSecret() => string.Join("", _secret);

    public static bool TryParseGuess(string? input, out int[] guess)
    {
        guess = Array.Empty<int>();
        if (input is null || input.Length != CodeLength) return false;

        int[] digits = input.Select(c => c - '0').ToArray();
        if (digits.Any(d => d < 1 || d > 6)) return false;

        guess = digits;
        return true;
    }

    private static int[] GenerateSecret() =>
        Enumerable.Range(0, CodeLength)
                  .Select(_ => Random.Shared.Next(1, 7))
                  .ToArray();

    private static GuessResult Score(int[] secret, int[] guess)
    {
        int exact = secret.Zip(guess).Count(p => p.First == p.Second);

        int common = Enumerable.Range(1, 6)
            .Sum(d => Math.Min(secret.Count(x => x == d), guess.Count(x => x == d)));

        return new GuessResult(exact, common - exact);
    }
}