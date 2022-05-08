namespace Gomoku.Logic;

public class Player
{
    #region Public Props
    public string Name { get; private set; }
    public Stone Stone { get; }

    #endregion

    #region Constructor
    public Player(string name, Stone stone)
    {
        Name = name;
        Stone = stone;
    }
    #endregion

}

