using System.Collections.Immutable;

namespace Gomoku.Logic;

public class PlayerManager
{
    #region Public Props

    public ImmutableArray<Player> Players { get; }
    public Turn Turn { get; }
    public Player CurrentPlayer => Players[Turn.Current];
    #endregion


    #region Constructor
    public PlayerManager(IEnumerable<Player> collection)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(nameof(collection));
        }

        if (!collection.Any() || collection.Count() < 1)
        {
            throw new ArgumentException(nameof(collection));
        }

        Players = collection.ToImmutableArray();
        Turn = new Turn(Players.Length);
    }
    #endregion
}

