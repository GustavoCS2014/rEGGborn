using System;

[Flags]
public enum GameState{
    None = 0,
    MainMenu = 1,
    Playing = 2,
    Paused = 4,
    NextOrRetryScene = 8,
    GameOver = 16,
}