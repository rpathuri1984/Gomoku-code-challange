namespace Gomoku.Logic;

public interface IGomokuGame
{
    public GomokuBoard GomokuBoard { get; }
    public bool IsTie { get; }
    public bool IsOver { get; }
    public int MaxMove { get; }
    public PlayerManager Manager { get; }
    public Cell? LastMove { get; }

    public PlayResult PlaceStone(int x, int y);
}

