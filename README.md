# Code Breaker

A Mastermind-style console game built with C# and .NET 10.
Guess the hidden 4-digit code (digits 1-6) in 10 tries. After each guess you
get the number of **exact** matches (right digit, right place) and
**misplaced** digits (right digit, wrong place).

## Run it

    dotnet run --project CodeBreaker

## Run the tests

    dotnet test

## Project structure

- `CodeBreaker` - console UI
- `CodeBreaker.Core` - game rules (no UI dependency)
- `CodeBreaker.Tests` - xUnit tests

## What I learned

- Separating game logic from the UI with a class library
- Encapsulation, records, LINQ, and the Try-pattern
- Unit testing with xUnit (`[Fact]`, `[Theory]`)