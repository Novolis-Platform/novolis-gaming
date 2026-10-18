namespace Novolis.Game.Sudoku;

/// <summary>Rates classic puzzles using deduction and bounded-search evidence.</summary>
public sealed class SudokuDifficultyAnalyzer
{
    private readonly SudokuSolver _solver;

    /// <summary>Creates an analyzer for a ruleset.</summary>
    public SudokuDifficultyAnalyzer(SudokuRuleSet? ruleset = null)
    {
        Ruleset = ruleset ?? ClassicSudokuRuleSet.Default;
        _solver = new SudokuSolver(Ruleset);
    }

    /// <summary>Rules used by this analyzer.</summary>
    public SudokuRuleSet Ruleset { get; }

    /// <summary>Produces a stable report for a puzzle.</summary>
    public SudokuDifficultyReport Analyze(SudokuBoard puzzle)
    {
        ArgumentNullException.ThrowIfNull(puzzle);
        var givenCount = SudokuHelpers.CountFilled(puzzle);
        var validation = Ruleset.Validate(puzzle);
        if (!validation.IsValid)
        {
            return new SudokuDifficultyReport
            {
                IsValidAndUnique = false,
                Difficulty = SudokuDifficulty.Expert,
                Score = int.MaxValue,
                GivenCount = givenCount,
            };
        }

        var logical = _solver.Solve(
            puzzle,
            new SudokuSolveOptions
            {
                LogicOnly = true,
                CaptureSteps = true,
            });
        var complete = _solver.Solve(
            puzzle,
            new SudokuSolveOptions
            {
                MaximumSolutions = 2,
                CaptureSteps = true,
            });

        var unique = complete.Status == SudokuSolveStatus.Solved
            && complete.SolutionCount == 1;
        var solvedByLogic = logical.Status == SudokuSolveStatus.Solved;
        var logicalSteps = logical.Statistics.LogicalSteps;
        var searchNodes = complete.Statistics.SearchNodes;
        var backtracks = complete.Statistics.Backtracks;
        var maxDepth = complete.Statistics.MaxSearchDepth;
        var score = unique
            ? CalculateScore(
                givenCount,
                logicalSteps,
                searchNodes,
                backtracks,
                maxDepth,
                solvedByLogic)
            : int.MaxValue;

        return new SudokuDifficultyReport
        {
            IsValidAndUnique = unique,
            Difficulty = unique ? ToDifficulty(score) : SudokuDifficulty.Expert,
            Score = score,
            GivenCount = givenCount,
            LogicalSteps = logicalSteps,
            SearchNodes = searchNodes,
            Backtracks = backtracks,
            MaxSearchDepth = maxDepth,
            HighestTechnique = GetHighestTechnique(complete.Steps),
            SolvedByLogic = solvedByLogic,
        };
    }

    private static int CalculateScore(
        int givenCount,
        int logicalSteps,
        int searchNodes,
        int backtracks,
        int maxDepth,
        bool solvedByLogic)
    {
        var score = (SudokuConstants.CellCount - givenCount) * 3
                    + logicalSteps
                    + searchNodes * 10
                    + backtracks * 20
                    + maxDepth * 40;
        if (!solvedByLogic)
            score += 300;
        return score;
    }

    private static SudokuDifficulty ToDifficulty(int score) => score switch
    {
        <= 180 => SudokuDifficulty.Easy,
        <= 500 => SudokuDifficulty.Medium,
        <= 2_000 => SudokuDifficulty.Hard,
        _ => SudokuDifficulty.Expert,
    };

    private static SudokuTechnique GetHighestTechnique(
        IReadOnlyList<SudokuSolveStep> steps)
    {
        if (steps.Any(step => step.Technique == SudokuTechnique.Guess))
            return SudokuTechnique.Guess;
        if (steps.Any(step => step.Technique == SudokuTechnique.HiddenSingle))
            return SudokuTechnique.HiddenSingle;
        if (steps.Any(step => step.Technique == SudokuTechnique.NakedSingle))
            return SudokuTechnique.NakedSingle;
        return SudokuTechnique.None;
    }
}
