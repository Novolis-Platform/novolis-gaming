using System.Numerics;

namespace Novolis.Game.Sudoku;

/// <summary>Compact set of the nine possible values for a classic cell.</summary>
public readonly struct CandidateMask : IEquatable<CandidateMask>
{
    /// <summary>Bits for values 1 through 9.</summary>
    public const ushort AllBits = 0x01FF;

    /// <summary>Creates a candidate set from its nine low bits.</summary>
    public CandidateMask(ushort bits)
    {
        if ((bits & ~AllBits) != 0)
            throw new ArgumentOutOfRangeException(nameof(bits), bits, "Candidate masks may only use nine bits.");
        Bits = bits;
    }

    /// <summary>Raw nine-bit representation.</summary>
    public ushort Bits { get; }

    /// <summary>All nine candidate values.</summary>
    public static CandidateMask All => new(AllBits);

    /// <summary>Empty candidate set.</summary>
    public static CandidateMask None => new(0);

    /// <summary>Number of values in the set.</summary>
    public int Count => BitOperations.PopCount((uint)Bits);

    /// <summary>Whether the set contains no values.</summary>
    public bool IsEmpty => Bits == 0;

    /// <summary>Whether the set contains exactly one value.</summary>
    public bool IsSingle => Bits != 0 && (Bits & (Bits - 1)) == 0;

    /// <summary>Creates a set containing one value.</summary>
    public static CandidateMask FromValue(byte value)
    {
        SudokuConstants.ValidateValue(value);
        if (value == SudokuConstants.Empty)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Empty is not a candidate.");
        return new((ushort)(1 << (value - 1)));
    }

    /// <summary>Whether the set contains <paramref name="value"/>.</summary>
    public bool Contains(byte value) =>
        value is >= 1 and <= SudokuConstants.SymbolCount
        && (Bits & (1 << (value - 1))) != 0;

    /// <summary>Returns a set with <paramref name="value"/> added.</summary>
    public CandidateMask Add(byte value) => new((ushort)(Bits | FromValue(value).Bits));

    /// <summary>Returns a set with <paramref name="value"/> removed.</summary>
    public CandidateMask Remove(byte value)
    {
        if (value is < 1 or > SudokuConstants.SymbolCount)
            return this;
        return new((ushort)(Bits & ~(1 << (value - 1))));
    }

    /// <summary>Returns the only value in a single-value set.</summary>
    public byte SingleValue
    {
        get
        {
            if (!IsSingle)
                throw new InvalidOperationException("The candidate mask is not a single value.");
            return (byte)(BitOperations.TrailingZeroCount((uint)Bits) + 1);
        }
    }

    /// <summary>Enumerates values in ascending order.</summary>
    public IEnumerable<byte> Values()
    {
        var remaining = Bits;
        while (remaining != 0)
        {
            var bit = (ushort)(remaining & (remaining - 1));
            var index = BitOperations.TrailingZeroCount((uint)(remaining ^ bit));
            yield return (byte)(index + 1);
            remaining = bit;
        }
    }

    /// <inheritdoc />
    public bool Equals(CandidateMask other) => Bits == other.Bits;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is CandidateMask other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Bits;

    /// <inheritdoc />
    public override string ToString() => string.Join(string.Empty, Values());

    /// <summary>Tests two candidate sets for equality.</summary>
    public static bool operator ==(CandidateMask left, CandidateMask right) => left.Equals(right);

    /// <summary>Tests two candidate sets for inequality.</summary>
    public static bool operator !=(CandidateMask left, CandidateMask right) => !left.Equals(right);
}
