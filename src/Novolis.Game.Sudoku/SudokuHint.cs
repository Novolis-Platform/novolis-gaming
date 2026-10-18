namespace Novolis.Game.Sudoku;

/// <summary>Explainable next move for a player.</summary>
public sealed record SudokuHint(
    int CellIndex,
    byte Value,
    SudokuTechnique Technique,
    string Reason);
