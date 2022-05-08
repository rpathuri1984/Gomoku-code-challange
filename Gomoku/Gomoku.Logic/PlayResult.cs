using Gomoku.Logic;
using System.Text;

namespace Gomoku.Logic;

public class PlayResult
{
    public string? CurrentPlayer { get; set; }

    public bool IsGameOver { get; set; }

    public string? Message { get; set; }

    public IList<Cell>? WinningLine { get; set; }

    [Obsolete("added this for debugging purpose and should be removed before merging to release")]
    public override string ToString()
    {
        if (WinningLine == null)
            return string.Empty;

        StringBuilder WinningLineText = new StringBuilder();

        WinningLineText.Append($"Current Player: {CurrentPlayer};");

        WinningLineText.Append($"IsGameOver: {IsGameOver};");
        WinningLineText.Append($"Message: {Message};");
        WinningLineText.Append($"WinningLine: {Message};");

        foreach (var cell in WinningLine)
        {
            string stone = cell.Stone.Type == Stones.X ? "X" : "O";
            WinningLineText.Append($"[{cell.X}|{cell.Y}|{stone}]; ");
        }
        return WinningLineText.ToString();

    }

}

