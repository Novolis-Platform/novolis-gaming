namespace Novolis.Game.Sudoku;

/// <summary>Search and deduction counters collected during solving.</summary>
public sealed record SudokuSolveStatistics
{
    /// <summary>Number of naked and hidden single placements.</summary>
    public int LogicalSteps { get; init; }

    /// <summary>Number of speculative branches visited.</summary>
    public int SearchNodes { get; init; }

    /// <summary>Number of branches that ended in contradiction.</summary>
    public int Backtracks { get; init; }

    /// <summary>Deepest speculative branch.</summary>
    public int MaxSearchDepth { get; init; }
}
