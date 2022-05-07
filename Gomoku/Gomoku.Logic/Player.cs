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
        SetName(name);
        Stone = stone;
    }
    #endregion

    #region Public methods
    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"{nameof(name)} must not be null or empty.");
        }

        Name = name;
    }
    public override string ToString()
    {
        return $"{nameof(Name)}={Name}, {nameof(Stone)}={Stone}";
    }

    #endregion
}

