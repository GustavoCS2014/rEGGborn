using System;

[Flags]
public enum GameState
{
    None = 0,
    MainMenu = 1,
    TickCooldown = 2,
    Playing = 4,
    Paused = 8,
    NextOrRetryScene = 16,
    GameOver = 32,
}