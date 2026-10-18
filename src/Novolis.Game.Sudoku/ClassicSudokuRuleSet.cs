namespace Novolis.Game.Sudoku;

/// <summary>Classic Sudoku topology: nine rows, nine columns, and nine boxes.</summary>
public sealed class ClassicSudokuRuleSet : SudokuRuleSet
{
    private ClassicSudokuRuleSet()
        : base("classic", CreateUnits())
    {
    }

    /// <summary>Shared immutable classic ruleset.</summary>
    public static SudokuRuleSet Default { get; } = new ClassicSudokuRuleSet();

    private static IReadOnlyList<SudokuUnit> CreateUnits()
    {
        var units = new List<SudokuUnit>(27);

        for (var row = 0; row < SudokuConstants.Size; row++)
        {
            units.Add(new SudokuUnit(
                $"row-{row + 1}",
                SudokuUnitKind.Row,
                Enumerable.Range(0, SudokuConstants.Size)
                    .Select(column => SudokuConstants.CellIndex(row, column))));
        }

        for (var column = 0; column < SudokuConstants.Size; column++)
        {
            units.Add(new SudokuUnit(
                $"column-{column + 1}",
                SudokuUnitKind.Column,
                Enumerable.Range(0, SudokuConstants.Size)
                    .Select(row => SudokuConstants.CellIndex(row, column))));
        }

        for (var boxRow = 0; boxRow < SudokuConstants.BoxSize; boxRow++)
        for (var boxColumn = 0; boxColumn < SudokuConstants.BoxSize; boxColumn++)
        {
            var cells = new List<int>(SudokuConstants.Size);
            for (var row = 0; row < SudokuConstants.BoxSize; row++)
            for (var column = 0; column < SudokuConstants.BoxSize; column++)
            {
                cells.Add(SudokuConstants.CellIndex(
                    boxRow * SudokuConstants.BoxSize + row,
                    boxColumn * SudokuConstants.BoxSize + column));
            }

            units.Add(new SudokuUnit(
                $"box-{boxRow * SudokuConstants.BoxSize + boxColumn + 1}",
                SudokuUnitKind.Box,
                cells));
        }

        return units;
    }
}
