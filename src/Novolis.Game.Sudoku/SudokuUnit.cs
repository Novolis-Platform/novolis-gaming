namespace Novolis.Game.Sudoku;

/// <summary>A named collection of cells whose values must be unique.</summary>
public sealed class SudokuUnit
{
    /// <summary>Creates a unit from distinct row-major cell indexes.</summary>
    public SudokuUnit(string id, SudokuUnitKind kind, IEnumerable<int> cells)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(cells);

        Id = id;
        Kind = kind;
        Cells = cells.ToArray();
        if (Cells.Count == 0)
            throw new ArgumentException("A Sudoku unit must contain at least one cell.", nameof(cells));
        if (Cells.Any(cell => (uint)cell >= SudokuConstants.CellCount))
            throw new ArgumentException("A Sudoku unit contains an invalid cell index.", nameof(cells));
        if (Cells.Distinct().Count() != Cells.Count)
            throw new ArgumentException("A Sudoku unit may not contain duplicate cells.", nameof(cells));
    }

    /// <summary>Stable ruleset-local identifier.</summary>
    public string Id { get; }

    /// <summary>Unit classification.</summary>
    public SudokuUnitKind Kind { get; }

    /// <summary>Cells in row-major order.</summary>
    public IReadOnlyList<int> Cells { get; }
}
