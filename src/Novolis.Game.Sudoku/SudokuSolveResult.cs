namespace Novolis.Game.Sudoku;

/// <summary>Result of solving or logically inspecting a Sudoku board.</summary>
public sealed record SudokuSolveResult
{
    /// <summary>Solver outcome.</summary>
    public required SudokuSolveStatus Status { get; init; }

    /// <summary>First solution found, when one exists.</summary>
    public SudokuBoard? Solution { get; init; }

    /// <summary>Number of solutions found, capped by the requested bound.</summary>
    public int SolutionCount { get; init; }

    /// <summary>Deduction and search counters.</summary>
    public required SudokuSolveStatistics Statistics { get; init; }

    /// <summary>Captured steps in deterministic order.</summary>
    public IReadOnlyList<SudokuSolveStep> Steps { get; init; } = [];

    /// <summary>Whether one complete solution was found.</summary>
    public bool IsSolved => Status == SudokuSolveStatus.Solved;
}
