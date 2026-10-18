namespace Novolis.Game.Sudoku;

/// <summary>Zero-based row and column coordinate for a Sudoku cell.</summary>
public readonly record struct SudokuCell
{
    /// <summary>Creates a coordinate after validating both axes.</summary>
    public SudokuCell(int row, int column)
    {
        SudokuConstants.ValidateCoordinate(row, nameof(row));
        SudokuConstants.ValidateCoordinate(column, nameof(column));
        Row = row;
        Column = column;
    }

    /// <summary>Zero-based row.</summary>
    public int Row { get; }

    /// <summary>Zero-based column.</summary>
    public int Column { get; }

    /// <summary>Row-major index.</summary>
    public int Index => SudokuConstants.CellIndex(Row, Column);

    /// <summary>Classic 3×3 box index.</summary>
    public int Box => SudokuConstants.BoxOf(Index);

    /// <summary>Creates a coordinate from a row-major cell index.</summary>
    public static SudokuCell FromIndex(int cellIndex) =>
        new(SudokuConstants.RowOf(cellIndex), SudokuConstants.ColumnOf(cellIndex));
}
