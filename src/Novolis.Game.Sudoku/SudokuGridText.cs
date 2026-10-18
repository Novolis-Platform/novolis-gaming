using System.Text;

namespace Novolis.Game.Sudoku;

/// <summary>Compact text import and export for 81-cell Sudoku grids.</summary>
public static class SudokuGridText
{
    /// <summary>Serializes a board as digits and dots.</summary>
    public static string Serialize(
        SudokuBoard board,
        bool multiline = false,
        char emptyCell = '.')
    {
        ArgumentNullException.ThrowIfNull(board);
        if (emptyCell is >= '0' and <= '9')
            throw new ArgumentOutOfRangeException(nameof(emptyCell), "The empty marker cannot be a digit.");

        var builder = new StringBuilder(
            multiline
                ? SudokuConstants.CellCount + SudokuConstants.Size - 1
                : SudokuConstants.CellCount);
        for (var row = 0; row < SudokuConstants.Size; row++)
        {
            if (row > 0 && multiline)
                builder.AppendLine();
            for (var column = 0; column < SudokuConstants.Size; column++)
            {
                var value = board[row, column];
                builder.Append(value == SudokuConstants.Empty ? emptyCell : (char)('0' + value));
            }
        }

        return builder.ToString();
    }

    /// <summary>Parses digits, zeroes, dots, and whitespace into a board.</summary>
    public static SudokuBoard Parse(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        var values = new List<byte>(SudokuConstants.CellCount);
        foreach (var character in text)
        {
            if (char.IsWhiteSpace(character))
                continue;
            if (character is '.' or '0')
            {
                values.Add(SudokuConstants.Empty);
                continue;
            }
            if (character is >= '1' and <= '9')
            {
                values.Add((byte)(character - '0'));
                continue;
            }

            throw new FormatException($"Unexpected Sudoku grid character '{character}'.");
        }

        if (values.Count != SudokuConstants.CellCount)
            throw new FormatException($"A Sudoku grid must contain exactly 81 cells, not {values.Count}.");
        return new SudokuBoard(values);
    }

    /// <summary>Attempts to parse a grid without throwing.</summary>
    public static bool TryParse(string? text, out SudokuBoard? board, out string? error)
    {
        try
        {
            board = Parse(text ?? string.Empty);
            error = null;
            return true;
        }
        catch (FormatException exception)
        {
            board = null;
            error = exception.Message;
            return false;
        }
        catch (ArgumentException exception)
        {
            board = null;
            error = exception.Message;
            return false;
        }
    }
}
