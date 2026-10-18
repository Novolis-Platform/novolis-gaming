namespace Novolis.Game.Sudoku;

/// <summary>Controls a solver run.</summary>
public sealed record SudokuSolveOptions
{
    /// <summary>Stops after logical singles instead of searching.</summary>
    public bool LogicOnly { get; init; }

    /// <summary>Captures placements and guesses in the returned trace.</summary>
    public bool CaptureSteps { get; init; } = true;

    /// <summary>Maximum number of solutions to find; two is enough to prove non-unique.</summary>
    public int MaximumSolutions { get; init; } = 2;
}
