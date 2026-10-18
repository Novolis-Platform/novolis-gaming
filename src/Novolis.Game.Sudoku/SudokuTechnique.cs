namespace Novolis.Game.Sudoku;

/// <summary>Solving technique represented in a trace or hint.</summary>
public enum SudokuTechnique
{
    /// <summary>No logical technique.</summary>
    None,

    /// <summary>A cell has exactly one candidate.</summary>
    NakedSingle,

    /// <summary>A value has one possible cell in a unit.</summary>
    HiddenSingle,

    /// <summary>A search branch was selected by minimum remaining values.</summary>
    Guess,
}
