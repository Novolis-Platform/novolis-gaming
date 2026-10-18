namespace Novolis.Game.Sudoku;

/// <summary>Generated puzzle, canonical solution, and measured difficulty.</summary>
public sealed record SudokuGeneratedGame
{
    /// <summary>Seed used for this generation attempt.</summary>
    public required ulong Seed { get; init; }

    /// <summary>Unique puzzle shown to a player.</summary>
    public required SudokuBoard Puzzle { get; init; }

    /// <summary>Canonical complete solution.</summary>
    public required SudokuBoard Solution { get; init; }

    /// <summary>Measured report for <see cref="Puzzle"/>.</summary>
    public required SudokuDifficultyReport Difficulty { get; init; }

    /// <summary>Attempt number that produced the returned game.</summary>
    public int GenerationAttempt { get; init; }
}
