using UnityEngine;

[CreateAssetMenu(fileName = "NewBlockShape", menuName = "Block Puzzle/Block Shape")]
public class BlockShapeData : ScriptableObject
{
   public Vector2Int[] occupiedCells;
   public Color shapeColor;
}
