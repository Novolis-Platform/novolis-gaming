namespace Novolis.Game.Sudoku;

/// <summary>Generation targets for a player-facing difficulty band.</summary>
public sealed record SudokuDifficultyProfile
{
    private SudokuDifficultyProfile(
        SudokuDifficulty difficulty,
        int minimumClues,
        int maximumClues,
        int minimumScore,
        int maximumScore)
    {
        Difficulty = difficulty;
        MinimumClues = minimumClues;
        MaximumClues = maximumClues;
        MinimumScore = minimumScore;
        MaximumScore = maximumScore;
    }

    /// <summary>Target difficulty.</summary>
    public SudokuDifficulty Difficulty { get; }

    /// <summary>Lower clue bound used by generation.</summary>
    public int MinimumClues { get; }

    /// <summary>Upper clue bound used by generation.</summary>
    public int MaximumClues { get; }

    /// <summary>Lower analyzer score accepted by the profile.</summary>
    public int MinimumScore { get; }

    /// <summary>Upper analyzer score accepted by the profile.</summary>
    public int MaximumScore { get; }

    /// <summary>Classic easy profile.</summary>
    public static SudokuDifficultyProfile Easy { get; } =
        new(SudokuDifficulty.Easy, 40, 81, 0, 120);

    /// <summary>Classic medium profile.</summary>
    public static SudokuDifficultyProfile Medium { get; } =
        new(SudokuDifficulty.Medium, 34, 45, 121, 360);

    /// <summary>Classic hard profile.</summary>
    public static SudokuDifficultyProfile Hard { get; } =
        new(SudokuDifficulty.Hard, 28, 39, 361, 1_100);

    /// <summary>Classic expert profile.</summary>
    public static SudokuDifficultyProfile Expert { get; } =
        new(SudokuDifficulty.Expert, 22, 33, 1_101, int.MaxValue);

    /// <summary>Returns the default profile for a difficulty band.</summary>
    public static SudokuDifficultyProfile For(SudokuDifficulty difficulty) => difficulty switch
    {
        SudokuDifficulty.Easy => Easy,
        SudokuDifficulty.Medium => Medium,
        SudokuDifficulty.Hard => Hard,
        SudokuDifficulty.Expert => Expert,
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null),
    };
}
