namespace Gomoku.Logic;

public interface IGomokuBoard
{
    public int Height { get; }
    public int Width { get; }
    public Cell this[int x, int y] { get; }
    public void IterateCells(int x, int y, Directions direction, Predicate<Cell> predicate, bool iterateSelf = false);
}

