using Novolis.Math.Arrays;

namespace Novolis.Game.Sudoku;

/// <summary>
/// Mutable 9×9 Sudoku board. Empty cells contain <see cref="SudokuConstants.Empty"/>.
/// </summary>
public sealed class SudokuBoard
{
    private readonly DenseGrid<byte> _grid;

    /// <summary>Creates an empty board.</summary>
    public SudokuBoard() =>
        _grid = new DenseGrid<byte>(SudokuConstants.Size, SudokuConstants.Size);

    /// <summary>Creates a board from 81 row-major values.</summary>
    public SudokuBoard(IEnumerable<byte> values)
        : this()
    {
        ArgumentNullException.ThrowIfNull(values);
        var array = values.ToArray();
        if (array.Length != SudokuConstants.CellCount)
            throw new ArgumentException("A Sudoku board must contain exactly 81 values.", nameof(values));

        for (var index = 0; index < array.Length; index++)
            Set(index, array[index]);
    }

    /// <summary>Reads or writes a cell using a row-major index.</summary>
    public byte this[int cellIndex]
    {
        get
        {
            SudokuConstants.ValidateCellIndex(cellIndex);
            return Get(cellIndex);
        }
        set => Set(cellIndex, value);
    }

    /// <summary>Reads or writes a cell using row and column coordinates.</summary>
    public byte this[int row, int column]
    {
        get => Get(new SudokuCell(row, column));
        set => Set(new SudokuCell(row, column), value);
    }

    /// <summary>Returns true when every cell contains a value.</summary>
    public bool IsComplete
    {
        get
        {
            for (var index = 0; index < SudokuConstants.CellCount; index++)
                if (Get(index) == SudokuConstants.Empty)
                    return false;
            return true;
        }
    }

    /// <summary>Returns a cell value by row-major index.</summary>
    public byte Get(int cellIndex)
    {
        SudokuConstants.ValidateCellIndex(cellIndex);
        var cell = SudokuCell.FromIndex(cellIndex);
        return _grid.Get((uint)cell.Column, (uint)cell.Row);
    }

    /// <summary>Returns a cell value by coordinate.</summary>
    public byte Get(SudokuCell cell) => Get(cell.Index);

    /// <summary>Sets a value after checking the symbol range.</summary>
    public void Set(int cellIndex, byte value)
    {
        SudokuConstants.ValidateCellIndex(cellIndex);
        SudokuConstants.ValidateValue(value);
        var cell = SudokuCell.FromIndex(cellIndex);
        _grid.Set((uint)cell.Column, (uint)cell.Row, value);
    }

    /// <summary>Sets a value by coordinate.</summary>
    public void Set(SudokuCell cell, byte value) => Set(cell.Index, value);

    /// <summary>Clears a cell.</summary>
    public void Clear(int cellIndex) => Set(cellIndex, SudokuConstants.Empty);

    /// <summary>Copies the board into a row-major array.</summary>
    public byte[] ToArray()
    {
        var values = new byte[SudokuConstants.CellCount];
        for (var index = 0; index < values.Length; index++)
            values[index] = Get(index);
        return values;
    }

    /// <summary>Creates an independent copy.</summary>
    public SudokuBoard Clone() => new(ToArray());

    /// <summary>Copies this board into a math-library dense grid.</summary>
    public DenseGrid<byte> ToDenseGrid()
    {
        var copy = new DenseGrid<byte>(SudokuConstants.Size, SudokuConstants.Size);
        for (var index = 0; index < SudokuConstants.CellCount; index++)
        {
            var cell = SudokuCell.FromIndex(index);
            copy.Set((uint)cell.Column, (uint)cell.Row, Get(index));
        }

        return copy;
    }

    /// <summary>Validates this board against a ruleset.</summary>
    public SudokuValidationResult Validate(SudokuRuleSet? ruleset = null) =>
        (ruleset ?? ClassicSudokuRuleSet.Default).Validate(this);

    /// <summary>Returns true when this board is full and valid.</summary>
    public bool IsSolved(SudokuRuleSet? ruleset = null) =>
        IsComplete && Validate(ruleset).IsValid;

    /// <inheritdoc />
    public override string ToString() => SudokuGridText.Serialize(this);
}
