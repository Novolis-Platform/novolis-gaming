namespace Novolis.Game.Sudoku;

/// <summary>Outcome of a Sudoku solver run.</summary>
public enum SudokuSolveStatus
{
    /// <summary>The input violates the rules.</summary>
    Invalid,

    /// <summary>No solution exists.</summary>
    Unsolvable,

    /// <summary>One solution was found.</summary>
    Solved,

    /// <summary>More than one solution was found within the configured bound.</summary>
    MultipleSolutions,

    /// <summary>Logical propagation stopped before a complete solution.</summary>
    Stalled,
}
