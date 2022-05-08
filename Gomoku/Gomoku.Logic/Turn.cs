namespace Gomoku.Logic;

public class Turn
{

    #region Public Props

    public int MaxTurns { get; }
    public int Start { get; private set; }
    public int Current { get; private set; }
    public int Next => GetNext(Current);
    #endregion


    #region Constructor
    public Turn(int maxPlayers, int start = 0)
    {
        if (maxPlayers < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxPlayers));
        }

        MaxTurns = maxPlayers;
        Start = start;
        Current = start;
    }
    #endregion

    #region Public Methods

    public void MoveNext()
    {
        Current = Next;
    }

    public void Reset()
    {
        Current = Start;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// turn will be swtiched between players. 
    /// </summary>
    /// <param name="currentTurn"></param>
    /// <returns></returns>
    private int GetNext(int currentTurn)
    {
        return (MaxTurns + currentTurn + 1) % MaxTurns;
    }
    #endregion
}

