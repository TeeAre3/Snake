namespace Snake
{
    public enum GameMode
    {
        WallsKill,
        WallsWrap,
        SwapEnds
    }


    public enum UIState
    {
        MainMenu,
        Running,
        GameOver
    }

    public enum GridValue
    {
        Empty,
        Snake,
        Food,
        Outside
    }
}
