using CodeBreaker.Core;

namespace CodeBreaker.Tests;

public class GameTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 2, 3 }, new[] { 2, 2, 1, 4 }, 1, 2)]
    [InlineData(new[] { 1, 1, 1, 1 }, new[] { 1, 2, 3, 4 }, 1, 0)]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2, 1 }, 0, 4)]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 4 }, 4, 0)]
    public void Guess_ReturnsCorrectScore(int[] secret, int[] guess, int exact, int misplaced)
    {
        var game = new Game(secret);

        var result = game.Guess(guess);

        Assert.Equal(exact, result.Exact);
        Assert.Equal(misplaced, result.Misplaced);
    }

    [Fact]
    public void Guess_CorrectCode_WinsGame()
    {
        var game = new Game(new[] { 1, 2, 3, 4 });

        game.Guess(new[] { 1, 2, 3, 4 });

        Assert.True(game.IsWon);
        Assert.True(game.IsOver);
    }

    [Fact]
    public void Guess_AfterTenWrongTries_GameIsOver()
    {
        var game = new Game(new[] { 1, 1, 1, 1 });

        for (int i = 0; i < Game.MaxAttempts; i++)
            game.Guess(new[] { 2, 2, 2, 2 });

        Assert.True(game.IsOver);
        Assert.False(game.IsWon);
        Assert.Throws<InvalidOperationException>(() => game.Guess(new[] { 1, 1, 1, 1 }));
    }
}