namespace Novolis.Game.Sudoku;

/// <summary>Deterministic difficulty evidence for a puzzle.</summary>
public sealed record SudokuDifficultyReport
{
    /// <summary>Whether the puzzle is valid and uniquely solvable.</summary>
    public bool IsValidAndUnique { get; init; }

    /// <summary>Assigned difficulty band.</summary>
    public SudokuDifficulty Difficulty { get; init; }

    /// <summary>Transparent aggregate used to select a difficulty band.</summary>
    public int Score { get; init; }

    /// <summary>Number of given cells.</summary>
    public int GivenCount { get; init; }

    /// <summary>Number of logical single placements.</summary>
    public int LogicalSteps { get; init; }

    /// <summary>Number of search branches explored.</summary>
    public int SearchNodes { get; init; }

    /// <summary>Number of failed search branches.</summary>
    public int Backtracks { get; init; }

    /// <summary>Deepest search branch.</summary>
    public int MaxSearchDepth { get; init; }

    /// <summary>Highest technique observed in the first solution trace.</summary>
    public SudokuTechnique HighestTechnique { get; init; }

    /// <summary>Whether singles alone completed the puzzle.</summary>
    public bool SolvedByLogic { get; init; }

    /// <summary>Checks whether this report fits a generation profile.</summary>
    public bool Matches(SudokuDifficultyProfile profile) =>
        IsValidAndUnique
        && Difficulty == profile.Difficulty
        && GivenCount >= profile.MinimumClues
        && GivenCount <= profile.MaximumClues
        && Score >= profile.MinimumScore
        && Score <= profile.MaximumScore;
}
