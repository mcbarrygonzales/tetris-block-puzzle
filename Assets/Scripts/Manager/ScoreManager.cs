using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnLinesCleared += HandleLinesCleared;
    }

    private void OnDisable()
    {
        GameEvents.OnLinesCleared -= HandleLinesCleared;
    }

    private void HandleLinesCleared(int lineCount)
    {
        AddScore(lineCount * 10);
    }

    public void ResetScore()
    {
        currentScore = 0;
        UIManager.Instance.UpdateScore(currentScore);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UIManager.Instance.UpdateScore(currentScore);
    }
}