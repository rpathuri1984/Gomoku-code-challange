using Gomoku.Logic;

namespace Gomoku.Logic;

public class PlayResult
{
    public Player? CurrentPlayer { get; set; }

    public bool IsGameOver { get; set; }

    public string? Message { get; set; }

    public IList<Cell>? WinningLine { get; set; }
}
