namespace Novolis.Game.Sudoku;

/// <summary>Player-facing classic Sudoku difficulty bands.</summary>
public enum SudokuDifficulty
{
    /// <summary>Usually solved with straightforward singles.</summary>
    Easy,

    /// <summary>More deductions, but little or no search.</summary>
    Medium,

    /// <summary>Meaningful search effort or sparse deductions.</summary>
    Hard,

    /// <summary>High search effort or a puzzle that resists simple singles.</summary>
    Expert,
}
