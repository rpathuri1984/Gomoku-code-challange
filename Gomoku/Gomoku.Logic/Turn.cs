namespace Gomoku.Logic;

public class Turn
{

    #region Public Props

    public int Max { get; }
    public bool IsReverse { get; set; }
    public int Start { get; private set; }
    public int Current { get; private set; }
    public int Next => GetNext(Current, IsReverse);
    public int Previous => GetNext(Current, IsReverse);
    #endregion


    #region Constructor
    public Turn(int maxTurn, int start = 0)
    {
        if (maxTurn < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxTurn));
        }

        Max = maxTurn;
        IsReverse = false;
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

    public void SetCurrent(int turn)
    {
        if (turn < 0 || turn >= Max)
        {
            throw new ArgumentOutOfRangeException($"{nameof(turn)} must be zero-based and less than Max.");
        }

        Current = turn;
    }

    public Turn ShallowClone()
    {
        return (Turn)MemberwiseClone();
    }
    #endregion

    #region Private Methods
    private int GetNext(int from, bool isReverse)
    {
        var OrderModifier = isReverse ? -1 : 1;
        return ((from + 1) * OrderModifier + Max) % Max;
    }
    #endregion
}

