namespace Novolis.Game.Sudoku;

/// <summary>One deterministic logical or search step from a solver trace.</summary>
public sealed record SudokuSolveStep(
    int CellIndex,
    byte Value,
    SudokuTechnique Technique,
    string Reason);
