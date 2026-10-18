namespace Novolis.Game.Sudoku;

/// <summary>
/// Optional rule beyond all-different units. Constraints are evaluated while
/// candidates are calculated and when a board is validated.
/// </summary>
public interface ISudokuConstraint
{
    /// <summary>Stable identifier used in diagnostics and persistence.</summary>
    string Id { get; }

    /// <summary>
    /// Returns whether a value is allowed at a cell in the current partial board.
    /// The cell may still be empty in <paramref name="board"/>.
    /// </summary>
    bool Allows(SudokuBoard board, int cellIndex, byte value);
}
