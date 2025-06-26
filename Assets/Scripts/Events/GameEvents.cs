using System;
using JetBrains.Annotations;

public static class GameEvents
{
    public static Action<int> OnLinesCleared;
    public static Action OnGameOver;
    public static Action OnBlockPlaced;
}
