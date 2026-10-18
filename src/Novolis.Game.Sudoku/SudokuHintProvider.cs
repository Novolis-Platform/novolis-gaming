namespace Novolis.Game.Sudoku;

/// <summary>Finds the next deterministic classic deduction without mutating a board.</summary>
public sealed class SudokuHintProvider
{
    private readonly SudokuSolver _solver;

    /// <summary>Creates a hint provider for a ruleset.</summary>
    public SudokuHintProvider(SudokuRuleSet? ruleset = null) =>
        _solver = new SudokuSolver(ruleset);

    /// <summary>Returns the first naked or hidden single, if available.</summary>
    public SudokuHint? FindNext(SudokuBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var result = _solver.Solve(
            board,
            new SudokuSolveOptions
            {
                LogicOnly = true,
                CaptureSteps = true,
            });
        var step = result.Steps.FirstOrDefault(item =>
            item.Technique is SudokuTechnique.NakedSingle or SudokuTechnique.HiddenSingle);
        return step is null
            ? null
            : new SudokuHint(step.CellIndex, step.Value, step.Technique, step.Reason);
    }
}
