using System.Collections.Immutable;

namespace Gomoku.Logic;

public class PlayerManager
{
    #region Public Props

    public ImmutableArray<Player> Players { get; }
    public Turn Turn { get; }
    public Player PreviousPlayer => Players[Turn.Previous];
    public Player CurrentPlayer => Players[Turn.Current];
    public Player NextPlayer => Players[Turn.Next];
    public Player this[int index] => Players[index];
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

    public PlayerManager(int numberofPlayers) : this(new Player[numberofPlayers])
    {
    }

    private PlayerManager(PlayerManager playerManager)
    {
        Players = ImmutableArray.Create(playerManager.Players, 0, playerManager.Players.Length);
        Turn = playerManager.Turn.ShallowClone();
    }
    #endregion
}

