namespace Novolis.Game.Sudoku;

/// <summary>Describes the kind of peer unit used by a Sudoku ruleset.</summary>
public enum SudokuUnitKind
{
    /// <summary>A horizontal row.</summary>
    Row,

    /// <summary>A vertical column.</summary>
    Column,

    /// <summary>A classic 3×3 box.</summary>
    Box,

    /// <summary>A future named region or shape.</summary>
    Region,

    /// <summary>A future custom unit.</summary>
    Custom,
}
