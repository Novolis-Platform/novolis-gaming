namespace Novolis.Game.Sudoku;

/// <summary>Controls deterministic puzzle carving.</summary>
public sealed record SudokuGenerationOptions
{
    /// <summary>Difficulty target used for clue bounds and acceptance.</summary>
    public SudokuDifficultyProfile Profile { get; init; } = SudokuDifficultyProfile.Medium;

    /// <summary>Whether clue removal should preserve 180-degree symmetry.</summary>
    public bool RotationalSymmetry { get; init; }

    /// <summary>Number of complete-grid/carving attempts before fallback.</summary>
    public int MaximumAttempts { get; init; } = 32;

    /// <summary>Whether failure to match the profile should throw.</summary>
    public bool RequireProfileMatch { get; init; }
}
