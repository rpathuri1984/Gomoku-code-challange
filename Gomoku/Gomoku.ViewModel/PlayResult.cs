using Gomoku.Logic;

namespace Gomoku.ViewModel;

public class PlayResult
{
    public Player? CurrentPlayer { get; set; }

    public bool IsGameOver { get; set; }

    public string? Message { get; set; }
}
