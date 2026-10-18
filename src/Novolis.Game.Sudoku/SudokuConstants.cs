namespace Novolis.Game.Sudoku;

/// <summary>Fixed dimensions and coordinate helpers for the first Sudoku ruleset.</summary>
public static class SudokuConstants
{
    /// <summary>Number of rows and columns in a classic board.</summary>
    public const int Size = 9;

    /// <summary>Total number of cells in a classic board.</summary>
    public const int CellCount = Size * Size;

    /// <summary>Width and height of each classic box.</summary>
    public const int BoxSize = 3;

    /// <summary>Number of symbols in a classic board.</summary>
    public const int SymbolCount = 9;

    /// <summary>Value used for an empty cell.</summary>
    public const byte Empty = 0;

    /// <summary>Returns the row-major cell index.</summary>
    public static int CellIndex(int row, int column)
    {
        ValidateCoordinate(row, nameof(row));
        ValidateCoordinate(column, nameof(column));
        return row * Size + column;
    }

    /// <summary>Returns the row for a row-major cell index.</summary>
    public static int RowOf(int cellIndex)
    {
        ValidateCellIndex(cellIndex);
        return cellIndex / Size;
    }

    /// <summary>Returns the column for a row-major cell index.</summary>
    public static int ColumnOf(int cellIndex)
    {
        ValidateCellIndex(cellIndex);
        return cellIndex % Size;
    }

    /// <summary>Returns the classic box index for a row-major cell index.</summary>
    public static int BoxOf(int cellIndex)
    {
        var row = RowOf(cellIndex);
        var column = ColumnOf(cellIndex);
        return (row / BoxSize) * BoxSize + column / BoxSize;
    }

    /// <summary>Validates a row-major cell index.</summary>
    public static void ValidateCellIndex(int cellIndex)
    {
        if ((uint)cellIndex >= CellCount)
            throw new ArgumentOutOfRangeException(nameof(cellIndex), cellIndex, "Cell index must be in [0, 81).");
    }

    internal static void ValidateCoordinate(int coordinate, string parameterName)
    {
        if ((uint)coordinate >= Size)
            throw new ArgumentOutOfRangeException(parameterName, coordinate, "Coordinate must be in [0, 9).");
    }

    /// <summary>Validates an empty-or-symbol value.</summary>
    public static void ValidateValue(byte value, string parameterName = "value")
    {
        if (value > SymbolCount)
            throw new ArgumentOutOfRangeException(parameterName, value, "A Sudoku value must be 0 through 9.");
    }
}
