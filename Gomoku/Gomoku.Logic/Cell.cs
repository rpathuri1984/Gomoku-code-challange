namespace Gomoku.Logic;

public class Cell : ICell
{
    #region Public Props
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

    #endregion

    #region Constroctor
    public Cell(int x, int y) : this(x, y, (Stone)Stones.None)
    {
    }

    public Cell(int x, int y, Stone stone)
    {
        X = x;
        Y = y;
        Stone = stone;
    }

    #endregion

}

