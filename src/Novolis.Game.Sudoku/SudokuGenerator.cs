using Novolis.Game.Procedural;

namespace Novolis.Game.Sudoku;

/// <summary>
/// Deterministic complete-grid and clue-removal generator. Every accepted
/// removal is checked with a two-solution bound so returned puzzles are unique.
/// </summary>
public sealed class SudokuGenerator
{
    private readonly SudokuSolver _solver;
    private readonly SudokuDifficultyAnalyzer _analyzer;

    /// <summary>Creates a generator for a ruleset.</summary>
    public SudokuGenerator(SudokuRuleSet? ruleset = null)
    {
        Ruleset = ruleset ?? ClassicSudokuRuleSet.Default;
        _solver = new SudokuSolver(Ruleset);
        _analyzer = new SudokuDifficultyAnalyzer(Ruleset);
    }

    /// <summary>Rules used for complete grids and uniqueness checks.</summary>
    public SudokuRuleSet Ruleset { get; }

    /// <summary>Generates a game for a difficulty band.</summary>
    public SudokuGeneratedGame Generate(
        ulong seed,
        SudokuDifficulty difficulty = SudokuDifficulty.Medium,
        SudokuGenerationOptions? options = null)
    {
        var selectedOptions = options ?? new SudokuGenerationOptions();
        var profile = options is null
            ? SudokuDifficultyProfile.For(difficulty)
            : selectedOptions.Profile;
        var maximumAttempts = System.Math.Max(1, selectedOptions.MaximumAttempts);
        SudokuGeneratedGame? last = null;

        for (var attempt = 1; attempt <= maximumAttempts; attempt++)
        {
            var attemptSeed = SeededRng.Mix(seed, attempt, (long)profile.Difficulty);
            var rng = new SeededRng(attemptSeed);
            var solution = GenerateCompleteGrid(ref rng);
            var puzzle = Carve(solution, ref rng, profile, selectedOptions);
            var report = _analyzer.Analyze(puzzle);
            last = new SudokuGeneratedGame
            {
                Seed = seed,
                Puzzle = puzzle,
                Solution = solution,
                Difficulty = report,
                GenerationAttempt = attempt,
            };

            if (report.Matches(profile))
                return last;
        }

        if (selectedOptions.RequireProfileMatch)
        {
            throw new InvalidOperationException(
                $"Could not generate a {profile.Difficulty} Sudoku puzzle within "
                + $"{maximumAttempts} attempts.");
        }

        return last
            ?? throw new InvalidOperationException("Sudoku generation produced no attempt.");
    }

    private SudokuBoard GenerateCompleteGrid(ref SeededRng rng)
    {
        var board = new SudokuBoard();
        if (!Fill(board, ref rng))
            throw new InvalidOperationException("The Sudoku ruleset could not produce a complete grid.");
        return board;
    }

    private bool Fill(SudokuBoard board, ref SeededRng rng)
    {
        var (cellIndex, candidates) = FindMinimumRemainingValue(board);
        if (cellIndex < 0)
            return board.IsSolved(Ruleset);
        if (candidates.IsEmpty)
            return false;

        var values = candidates.Values().ToArray();
        Shuffle(values, ref rng);
        foreach (var value in values)
        {
            board.Set(cellIndex, value);
            if (Fill(board, ref rng))
                return true;
            board.Clear(cellIndex);
        }

        return false;
    }

    private SudokuBoard Carve(
        SudokuBoard solution,
        ref SeededRng rng,
        SudokuDifficultyProfile profile,
        SudokuGenerationOptions options)
    {
        var puzzle = solution.Clone();
        var order = Enumerable.Range(0, SudokuConstants.CellCount).ToArray();
        Shuffle(order, ref rng);

        foreach (var cellIndex in order)
        {
            if (SudokuHelpers.CountFilled(puzzle) <= profile.MinimumClues)
                break;

            var partner = SudokuConstants.CellCount - 1 - cellIndex;
            var cells = options.RotationalSymmetry && partner != cellIndex
                ? new[] { cellIndex, partner }
                : new[] { cellIndex };
            if (cells.Any(cell => puzzle.Get(cell) == SudokuConstants.Empty))
                continue;

            var saved = cells.Select(puzzle.Get).ToArray();
            foreach (var cell in cells)
                puzzle.Clear(cell);

            var result = _solver.Solve(
                puzzle,
                new SudokuSolveOptions
                {
                    MaximumSolutions = 2,
                    CaptureSteps = false,
                });
            if (result.Status != SudokuSolveStatus.Solved || result.SolutionCount != 1)
            {
                for (var index = 0; index < cells.Length; index++)
                    puzzle.Set(cells[index], saved[index]);
            }
        }

        return puzzle;
    }

    private (int CellIndex, CandidateMask Candidates) FindMinimumRemainingValue(
        SudokuBoard board)
    {
        var selectedCell = -1;
        var selectedCandidates = CandidateMask.None;
        var smallestCount = int.MaxValue;
        for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
        {
            if (board.Get(cellIndex) != SudokuConstants.Empty)
                continue;
            var candidates = _solver.GetCandidates(board, cellIndex);
            if (candidates.Count < smallestCount)
            {
                selectedCell = cellIndex;
                selectedCandidates = candidates;
                smallestCount = candidates.Count;
                if (smallestCount <= 1)
                    break;
            }
        }

        return (selectedCell, selectedCandidates);
    }

    private static void Shuffle<T>(T[] values, ref SeededRng rng)
    {
        for (var index = values.Length - 1; index > 0; index--)
        {
            var other = rng.NextInt(0, index + 1);
            (values[index], values[other]) = (values[other], values[index]);
        }
    }
}
