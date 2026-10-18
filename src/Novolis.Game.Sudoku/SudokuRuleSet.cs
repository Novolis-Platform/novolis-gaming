namespace Novolis.Game.Sudoku;

/// <summary>
/// Constraint topology used by the solver and generator. Classic Sudoku is
/// represented by all-different row, column, and box units.
/// </summary>
public class SudokuRuleSet
{
    private readonly int[][] _peers;
    private readonly SudokuUnit[][] _unitsByCell;

    /// <summary>Creates a ruleset over the current 9×9 board.</summary>
    public SudokuRuleSet(
        string id,
        IEnumerable<SudokuUnit> units,
        IEnumerable<ISudokuConstraint>? constraints = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(units);

        Id = id.Trim();
        Units = units.ToArray();
        if (Units.Count == 0)
            throw new ArgumentException("A ruleset must contain at least one unit.", nameof(units));

        Constraints = (constraints ?? []).ToArray();
        if (Constraints.Any(constraint => constraint is null))
            throw new ArgumentException("A ruleset cannot contain a null constraint.", nameof(constraints));

        _peers = new int[SudokuConstants.CellCount][];
        _unitsByCell = new SudokuUnit[SudokuConstants.CellCount][];
        for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
        {
            var unitsForCell = Units
                .Where(unit => unit.Cells.Contains(cellIndex))
                .ToArray();
            _unitsByCell[cellIndex] = unitsForCell;
            _peers[cellIndex] = unitsForCell
                .SelectMany(unit => unit.Cells)
                .Where(peer => peer != cellIndex)
                .Distinct()
                .Order()
                .ToArray();
        }
    }

    /// <summary>Stable identifier persisted with a game.</summary>
    public string Id { get; }

    /// <summary>All-different units in this topology.</summary>
    public IReadOnlyList<SudokuUnit> Units { get; }

    /// <summary>Optional non-unit constraints.</summary>
    public IReadOnlyList<ISudokuConstraint> Constraints { get; }

    /// <summary>Returns peer cells for a cell in deterministic order.</summary>
    public IReadOnlyList<int> GetPeers(int cellIndex)
    {
        SudokuConstants.ValidateCellIndex(cellIndex);
        return _peers[cellIndex];
    }

    /// <summary>Returns units containing a cell.</summary>
    public IReadOnlyList<SudokuUnit> GetUnitsForCell(int cellIndex)
    {
        SudokuConstants.ValidateCellIndex(cellIndex);
        return _unitsByCell[cellIndex];
    }

    /// <summary>Checks all unit and registered constraint rules.</summary>
    public SudokuValidationResult Validate(SudokuBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var errors = new List<string>();

        foreach (var unit in Units)
        {
            var seen = new Dictionary<byte, int>();
            foreach (var cellIndex in unit.Cells)
            {
                var value = board.Get(cellIndex);
                if (value == SudokuConstants.Empty)
                    continue;

                if (seen.TryGetValue(value, out var previousCell))
                {
                    errors.Add(
                        $"{unit.Id} repeats {value} at cells {previousCell} and {cellIndex}.");
                }
                else
                {
                    seen[value] = cellIndex;
                }
            }
        }

        foreach (var constraint in Constraints)
        {
            for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
            {
                var value = board.Get(cellIndex);
                if (value != SudokuConstants.Empty
                    && !constraint.Allows(board, cellIndex, value))
                {
                    errors.Add(
                        $"{constraint.Id} rejects value {value} at cell {cellIndex}.");
                }
            }
        }

        return errors.Count == 0
            ? SudokuValidationResult.Valid
            : new SudokuValidationResult(false, errors);
    }
}
