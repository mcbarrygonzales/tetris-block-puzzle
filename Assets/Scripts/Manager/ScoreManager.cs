using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Configuration")]
    public ScoreConfig config;

    private int currentScore = 0;
    private int highScore = 0;

    private const string HighScoreKey = "HighScore";

    private void Awake()
    {
        Instance = this;
        LoadHighScore();
    }

    private void OnEnable()
    {
        GameEvents.OnLinesCleared += HandleLinesCleared;
        GameEvents.OnBlockPlaced += HandleBlockPlaced;
    }

    private void OnDisable()
    {
        GameEvents.OnLinesCleared -= HandleLinesCleared;
        GameEvents.OnBlockPlaced -= HandleBlockPlaced;
    }

    private void HandleLinesCleared(int clearedCount)
    {
        if (clearedCount <= 0 || config == null || config.lineClearScores.Length == 0)
            return;

        int index = Mathf.Clamp(clearedCount - 1, 0, config.lineClearScores.Length - 1);
        int score = config.lineClearScores[index];

        AddScore(score);
    }

    private void HandleBlockPlaced()
    {
        if (config != null)
            AddScore(config.basePlacementScore);
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

        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
            UIManager.Instance.UpdateHighScore(highScore);
        }
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        UIManager.Instance.UpdateHighScore(highScore);
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HighScoreKey, highScore);
        PlayerPrefs.Save();
    }
}