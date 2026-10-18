namespace Novolis.Game.Sudoku;

/// <summary>Small player-facing helpers built on a ruleset.</summary>
public static class SudokuHelpers
{
    /// <summary>Returns filled peers that conflict with the selected cell.</summary>
    public static IReadOnlyList<int> FindConflicts(
        SudokuBoard board,
        int cellIndex,
        SudokuRuleSet? ruleset = null)
    {
        ArgumentNullException.ThrowIfNull(board);
        SudokuConstants.ValidateCellIndex(cellIndex);
        var value = board.Get(cellIndex);
        if (value == SudokuConstants.Empty)
            return [];

        var peers = (ruleset ?? ClassicSudokuRuleSet.Default).GetPeers(cellIndex);
        return peers
            .Where(peer => board.Get(peer) == value)
            .ToArray();
    }

    /// <summary>Returns candidate masks for empty cells and empty masks for filled cells.</summary>
    public static CandidateMask[] GetCandidates(
        SudokuBoard board,
        SudokuRuleSet? ruleset = null)
    {
        ArgumentNullException.ThrowIfNull(board);
        return new SudokuSolver(ruleset).GetCandidateGrid(board);
    }

    /// <summary>Returns a compact count of filled values.</summary>
    public static int CountFilled(SudokuBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var count = 0;
        for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
            if (board.Get(cellIndex) != SudokuConstants.Empty)
                count++;
        return count;
    }
}
