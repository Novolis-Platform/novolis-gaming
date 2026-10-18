namespace Novolis.Game.Sudoku;

/// <summary>Validation result with stable human-readable diagnostics.</summary>
public sealed class SudokuValidationResult
{
    /// <summary>Creates a validation result.</summary>
    public SudokuValidationResult(bool isValid, IEnumerable<string>? errors = null)
    {
        IsValid = isValid;
        Errors = (errors ?? []).Where(error => !string.IsNullOrWhiteSpace(error)).ToArray();
        if (IsValid && Errors.Count != 0)
            throw new ArgumentException("A valid result cannot contain errors.", nameof(errors));
        if (!IsValid && Errors.Count == 0)
            throw new ArgumentException("An invalid result must contain an error.", nameof(errors));
    }

    /// <summary>True when the board satisfies all rules.</summary>
    public bool IsValid { get; }

    /// <summary>Validation errors in deterministic order.</summary>
    public IReadOnlyList<string> Errors { get; }

    /// <summary>Creates a successful result.</summary>
    public static SudokuValidationResult Valid { get; } = new(true);

    /// <summary>Creates an invalid result.</summary>
    public static SudokuValidationResult Invalid(params string[] errors) => new(false, errors);
}
