namespace Gomoku.Logic;


/// <summary>
/// Defines a stone.
/// </summary>
public struct Stone
{
    public Stone(byte typeIndex)
    {
        TypeIndex = typeIndex;
    }

    public Stone(Stones stones) : this((byte)stones)
    {
    }

    public Stones Type => this;

    public byte TypeIndex { get; }

    public static explicit operator Stone(byte i)
    {
        return new Stone(i);
    }

    public static explicit operator Stone(Stones p)
    {
        return new Stone(p);
    }

    public static implicit operator byte(Stone p)
    {
        return p.TypeIndex;
    }

    public static implicit operator Stones(Stone p)
    {
        return (Stones)p.TypeIndex;
    }

    public static bool operator !=(Stone p1, Stone p2)
    {
        return p1.TypeIndex != p2.TypeIndex;
    }

    public static bool operator !=(Stone p1, Stones p2)
    {
        return p1.TypeIndex != (byte)p2;
    }

    public static bool operator ==(Stone p1, Stone p2)
    {
        return p1.TypeIndex == p2.TypeIndex;
    }

    public static bool operator ==(Stone p1, Stones p2)
    {
        return p1.TypeIndex == (byte)p2;
    }

    public override bool Equals(object obj)
    {
        return obj is Stone piece
          && TypeIndex == piece.TypeIndex;
    }

    public override int GetHashCode()
    {
        return 1970110603 + TypeIndex.GetHashCode();
    }

    public override string ToString()
    {
        return TypeIndex.ToString();
    }
}

