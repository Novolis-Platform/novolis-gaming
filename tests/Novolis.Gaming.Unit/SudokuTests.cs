using Novolis.Game.Sudoku;

namespace Novolis.Gaming.Unit.Sudoku;

public sealed class SudokuTests
{
    private const string Puzzle =
        "530070000"
        + "600195000"
        + "098000060"
        + "800060003"
        + "400803001"
        + "700020006"
        + "060000280"
        + "000419005"
        + "000080079";

    private const string Solution =
        "534678912"
        + "672195348"
        + "198342567"
        + "859761423"
        + "426853791"
        + "713924856"
        + "961537284"
        + "287419635"
        + "345286179";

    [Test]
    public async Task Classic_Rules_Expose_27_Units_And_20_Peers()
    {
        var ruleset = ClassicSudokuRuleSet.Default;

        await Assert.That(ruleset.Units.Count).IsEqualTo(27);
        await Assert.That(ruleset.GetPeers(0).Count).IsEqualTo(20);
        await Assert.That(ruleset.GetUnitsForCell(0).Count).IsEqualTo(3);
    }

    [Test]
    public async Task Solver_Finds_Known_Solution()
    {
        var board = SudokuGridText.Parse(Puzzle);
        var result = new SudokuSolver().Solve(board);

        await Assert.That(result.Status).IsEqualTo(SudokuSolveStatus.Solved);
        await Assert.That(result.SolutionCount).IsEqualTo(1);
        await Assert.That(SudokuGridText.Serialize(result.Solution!)).IsEqualTo(Solution.Replace('0', '.'));
        await Assert.That(result.Statistics.LogicalSteps).IsGreaterThan(0);
    }

    [Test]
    public async Task Invalid_And_Multiple_Boards_Are_Reported()
    {
        var invalid = SudokuGridText.Parse(
            "550000000"
            + "000000000"
            + "000000000"
            + "000000000"
            + "000000000"
            + "000000000"
            + "000000000"
            + "000000000"
            + "000000000");
        var invalidResult = new SudokuSolver().Solve(invalid);
        var multipleResult = new SudokuSolver().Solve(
            new SudokuBoard(),
            new SudokuSolveOptions
            {
                MaximumSolutions = 2,
                CaptureSteps = false,
            });

        await Assert.That(invalidResult.Status).IsEqualTo(SudokuSolveStatus.Invalid);
        await Assert.That(multipleResult.Status).IsEqualTo(SudokuSolveStatus.MultipleSolutions);
        await Assert.That(multipleResult.SolutionCount).IsEqualTo(2);
    }

    [Test]
    public async Task Candidate_And_Hint_Helpers_Do_Not_Mutate_Board()
    {
        var board = SudokuGridText.Parse(Puzzle);
        var before = SudokuGridText.Serialize(board);
        var candidates = SudokuHelpers.GetCandidates(board);
        var hint = new SudokuHintProvider().FindNext(board);

        await Assert.That(candidates[2].Contains(4)).IsTrue();
        await Assert.That(hint).IsNotNull();
        await Assert.That(SudokuGridText.Serialize(board)).IsEqualTo(before);
    }

    [Test]
    public async Task Generator_Is_Deterministic_And_Unique()
    {
        var options = new SudokuGenerationOptions
        {
            Profile = SudokuDifficultyProfile.Easy,
            MaximumAttempts = 8,
            RequireProfileMatch = false,
        };
        var generator = new SudokuGenerator();
        var first = generator.Generate(42, SudokuDifficulty.Easy, options);
        var second = generator.Generate(42, SudokuDifficulty.Easy, options);
        var solve = new SudokuSolver().Solve(
            first.Puzzle,
            new SudokuSolveOptions
            {
                MaximumSolutions = 2,
                CaptureSteps = false,
            });

        await Assert.That(SudokuGridText.Serialize(first.Puzzle)).IsEqualTo(
            SudokuGridText.Serialize(second.Puzzle));
        await Assert.That(SudokuGridText.Serialize(first.Solution)).IsEqualTo(
            SudokuGridText.Serialize(second.Solution));
        await Assert.That(first.Difficulty.IsValidAndUnique).IsTrue();
        await Assert.That(solve.Status).IsEqualTo(SudokuSolveStatus.Solved);
        await Assert.That(solve.SolutionCount).IsEqualTo(1);
    }
}
