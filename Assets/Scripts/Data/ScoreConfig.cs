using UnityEngine;

[CreateAssetMenu(fileName = "ScoreConfig", menuName = "Game/ScoreConfig")]
public class ScoreConfig : ScriptableObject
{
    [Tooltip("Points awarded for placing a block even if no lines are cleared.")]
    public int basePlacementScore = 5;

    [Tooltip("Points for clearing N lines at once (index 0 = 1 line, 4 = 5 lines).")]
    public int[] lineClearScores = new int[5] { 10, 25, 50, 100, 200 };
}