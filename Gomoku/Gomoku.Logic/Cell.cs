namespace Gomoku.Logic;

public class Cell : ICell
{

    /// <summary>
    /// The <see cref="Stone"/> this <see cref="Cell"/> currently holds
    /// </summary>
    public Stone Stone { get; set; }

    /// <summary>
    /// The X coordinate of the <see cref="Cell"/>
    /// </summary>
    public int X { get; }

    /// <summary>
    /// The Y coordinate of the <see cref="Cell"/>
    /// </summary>
    public int Y { get; }

    public Cell(int x, int y) : this(x, y, (Stone)Stones.None)
    {
    }

    public Cell(int x, int y, Stone stone)
    {
        X = x;
        Y = y;
        Stone = stone;
    }


    public static bool operator !=(Cell t1, Cell t2)
    {
        return !(t1 == t2);
    }

    public static bool operator ==(Cell t1, Cell t2)
    {
        if (t1 is null && t2 is null)
        {
            return true;
        }

        if (t1 is null || t2 is null)
        {
            return false;
        }

        return t1.X == t2.X
          && t1.Y == t2.Y
          && t1.Stone == t2.Stone;
    }
    public override bool Equals(object obj)
    {
        return obj is Cell cell
          && EqualityComparer<Stone>.Default.Equals(Stone, cell.Stone)
          && X == cell.X
          && Y == cell.Y;
    }
}

